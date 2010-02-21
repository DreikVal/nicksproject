using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AlienShooterGame
{
    /// <summary>
    /// The entity class is the base class of all game objects. Spaceships, guns, missiles, items, walls, trees and background
    /// stars should all extend from the entity class.
    /// </summary>
    public class Entity
    {
        #region Members

        /// <summary>
        /// An entities ID is a unique identifier for that entity. It is used to distinguish which entity a message refers 
        /// to in client <--> server communication.
        /// </summary>
        public UInt64 ID { get { return _ID; } set { _ID = value; } }
        protected UInt64 _ID;
        private static UInt64 _EntityCount = 0;

        /// <summary>
        /// When an entity is flagged as disposed it will no longer be updated or rendered. Sometimes it is not possible to
        /// remove these entities instantaneously due to multi-threaded shared-data issues, hence the reason for this variable.
        /// See EntityManager for more information on multi-threading issues.
        /// </summary>
        public bool Disposed { get { return _Disposed; } }
        private bool _Disposed = false;

        /// <summary>
        /// The entity manager that this entity belongs to. All entities must have an entity manager so that they are updated
        /// and rendered as expected.
        /// </summary>
        public Screen Parent { get { return _Parent; } }
        protected Screen _Parent;

        /// <summary>
        /// Gets the class name of this entity, as used in network communication.
        /// </summary>
        public String EntityClass { get { return _EntityClass; } }
        protected String _EntityClass = "Entity";

        /// <summary>
        /// Gets or sets an object that defines the polygon geometry of this entity.
        /// </summary>
        public Geometry Geometry { get { return _Geometry; } set { _Geometry = value; } }
        protected Geometry _Geometry = null;

        /// <summary>
        /// Gets a set of animations for this entity.
        /// </summary>
        public AnimationSet Animations { get { return _Animations; } }
        protected AnimationSet _Animations = new AnimationSet();

        /// <summary>
        /// Gets or sets the depth of this entity (layer level on the screen, 0.0f = in front, 1.0f = at back)
        /// </summary>
        public float Depth { get { return _Depth; } set { _Depth = value; } }
        protected float _Depth = 0.5f;

        /// <summary>
        /// Gets or sets the overlay colour for this entity
        /// </summary>
        public Color ColourOverlay { get { return _ColourOverlay; } set { _ColourOverlay = value; } }
        protected Color _ColourOverlay = Color.White;

        /// <summary>
        /// Gets or sets whether this entity requires light sources to become illuminated
        /// </summary>
        public bool DynamicLighting { get { return _DynamicLighting; } set { _DynamicLighting = value; } }
        protected bool _DynamicLighting = false;

        public bool Hide { get { return _Hide; } set { _Hide = value; } }
        protected bool _Hide = false;

        public CollisionType CollisionType { get { return _CollisionType; } set { _CollisionType = value; } }
        protected CollisionType _CollisionType = CollisionType.None;

        protected Color _ActualColour = Color.White;

        private Animation _Draw_Animation;
        private Vector2 _Draw_Origin, _Draw_Position, _Draw_Size;
        private Rectangle _Draw_Destination, _Draw_Source;
        private bool _Draw_OnScreen = false;

        public bool Opaque { get { return _Opaque; } set { _Opaque = value; } }
        protected bool _Opaque = false;

        public List<ShadowRegion> Shadows { get { return _Shadows; } }
        protected List<ShadowRegion> _Shadows = new List<ShadowRegion>();
        

        #endregion


        #region Initialisation and Disposal

        /// <summary>
        /// Constructs a new entity.
        /// </summary>
        /// <param name="id">The unique ID for the entity. (Optional parameter.)</param>
        /// <param name="manager">The entity manager for this entity.</param>
        public Entity(Screen parent)
        {
            // Retrieve entity manager
            if (parent == null)
                throw new Exception("Cannot create entity with a null Parent screen.");
            _Parent = parent;

            _ID = _EntityCount++;

            // Allow inherited classes to initialize entity settings before finalizing construction
            _EntityClass = Initialize();

            // Register this entity with the entity manager
            _Parent.Entities.Add(_ID, this);
        }

        /// <summary>
        /// This method is used to define the characteristics of an Entity.
        /// </summary>
        /// <returns>It must return a string which is unique to this class of entity. This string is used to create an instance of this entity.</returns>
        public virtual String Initialize()
        {
            return "Entity";
        }

        /// <summary>
        /// This destroys the entity, it will be de-registered with the entity manager and no longer updated.
        /// </summary>
        public virtual void Dispose()
        {
            _Disposed = true;
            _Parent.Entities.Remove(_ID);
        }

        #endregion


        #region Update and Draw

        public virtual void Update(GameTime time)
        {
            if (_Disposed) return;

            // Collision updates only done onscreen as well
            if (_CollisionType == CollisionType.Active) CheckCollisions();

            // Graphics updates, bail now if the entity isn't on screen
            if (!isOnScreen()) { _Draw_OnScreen = false; return; }
            if (_Hide) return;

            _Draw_OnScreen = true;
            _Draw_Animation = _Animations.Current;
            _Draw_Position = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Geometry.Position);
            _Draw_Size = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(_Geometry.Size);
            _Draw_Origin = new Vector2(_Animations.Current.WidthPerCell / 2, _Animations.Current.HeightPerCell / 2);
            _Draw_Destination = new Rectangle((int)(_Draw_Position.X), (int)(_Draw_Position.Y), (int)_Draw_Size.X, (int)_Draw_Size.Y);
            _Draw_Source = _Draw_Animation.UpdateSource(time);
        }

        public virtual void BackgroundUpdate()
        {
            if (Application.AppReference.DynamicLighting && _DynamicLighting)
            {
                _Lighting = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                _Parent.Lights.ForEach(ForEachLight, null, null, null);
                Vector4 _VectorOverlay = _ColourOverlay.ToVector4();

                _ActualColour = new Color(_VectorOverlay * _Lighting);
            }
            else
                _ActualColour = _ColourOverlay;
        }

        private bool ForEachLight(LightSource light, object p1, object p2, object p3)
        {
            if (!light.Active) return true;

            if (!TraceRay(light)) return true;

            float x_diff = _Geometry.Position.X - light.Geometry.Position.X;
            float y_diff = _Geometry.Position.Y - light.Geometry.Position.Y;
            float dist = (float)Math.Sqrt((x_diff * x_diff) + (y_diff * y_diff));
            float val = 1.0f - (dist / light.Range);
            if (val < 0.0f) val = 0.0f;

            float angle = (float)Math.Atan2(y_diff, x_diff);
            float angle_diff = (float)Math.Abs(light.Geometry.Direction - Math.PI / 2 - angle);
            if (angle_diff > Math.PI)
                angle_diff = 2 * (float)Math.PI - angle_diff;
            float angle_val = 1.0f - (angle_diff / light.Radius);

            Vector4 pre = light.ColourOverlay.ToVector4();
            Vector4 dis = new Vector4(val, val, val, 1.0f);
            Vector4 ang = new Vector4(angle_val, angle_val, angle_val, 1.0f);
            Vector4 result = pre * dis * ang;
            if (result.X < 0.0f) result.X = 0.0f;
            if (result.Y < 0.0f) result.Y = 0.0f;
            if (result.Z < 0.0f) result.Z = 0.0f;
            if (result.W < 0.0f) result.W = 0.0f;
            _Lighting += result;
            return true;
        }
        Vector4 _Lighting;

        protected virtual bool TraceRay(LightSource light)
        {
            return _Parent.Shadows.ForEach(ForEachRay, light, null, null);
        }
        private bool ForEachRay(ShadowRegion shadow, object p1, object p2, object p3)
        {
            if (_Shadows.Contains(shadow)) return true;
            LightSource light = (LightSource)p1;

            Vector2 dir = light.Geometry.Position - Geometry.Position;
            Vector2 diff = shadow.Geometry.Position - Geometry.Position;
            float t = (diff.X * dir.X + diff.Y * dir.Y) / (dir.X * dir.X + dir.Y * dir.Y);
            if (t < 0.0f)
                t = 0.0f;
            if (t > 1.0f)
                t = 1.0f;
            Vector2 closest = Geometry.Position + t * dir;
            Vector2 d = shadow.Geometry.Position - closest;
            float distsqr = d.X * d.X + d.Y * d.Y;
            return distsqr > shadow.Radius * shadow.Radius;
        }

        public virtual void Draw(GameTime time, SpriteBatch batch) 
        {
            if (_Draw_OnScreen == false) return;
            if (_Disposed) return;
            if (_Draw_Animation == null) return;
            if (_Hide) return;

            batch.Draw(_Draw_Animation.Texture, _Draw_Destination, _Draw_Source, _ActualColour, (float)_Geometry.Direction, _Draw_Origin, SpriteEffects.None, _Depth);
        }

        #endregion

        public bool isOnScreen()
        {
            if (_Geometry.Position.X + _Geometry.Radius < _Parent.ViewPort.ActualLocation.X ||
                _Geometry.Position.Y + _Geometry.Radius < _Parent.ViewPort.ActualLocation.Y ||
                _Geometry.Position.X - _Geometry.Radius > _Parent.ViewPort.ActualLocation.X + _Parent.ViewPort.Size.X ||
                _Geometry.Position.Y - _Geometry.Radius > _Parent.ViewPort.ActualLocation.Y + _Parent.ViewPort.Size.Y)
                return false;
            return true;
        }

        protected virtual void CheckCollisions()
        {
            _Parent.Entities.ForEach(ForEachCollisionCheck, null, null, null);
        }
        private bool ForEachCollisionCheck(Entity ent, object p1, object p2, object p3)
        {
            if (ent.CollisionType == CollisionType.None) return true;
            if (ent == this) return true;

            if (_Geometry.Collision(ent.Geometry))
            {
                HandleCollision(ent);
                ent.HandleCollision(this);
            }
            return true;
        }

        protected virtual void HandleCollision(Entity ent) { }
    }

    public enum CollisionType
    {
        Active,
        Passive,
        None
    }
}
