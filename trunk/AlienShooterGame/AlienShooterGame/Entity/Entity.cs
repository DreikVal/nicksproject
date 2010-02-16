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

        protected Color _ActualColour = Color.White;
        

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
        }

        public virtual void Draw(GameTime time, SpriteBatch batch) 
        {
            if (_Disposed) return;

            if (_Animations.Current == null) return;

            if (_DynamicLighting)
            {
                Vector4 _Lighting = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                foreach (LightSource light in _Parent.Lights)
                {
                    if (!light.Active) continue;

                    float x_diff = _Geometry.Position.X - light.Position.X;
                    float y_diff = _Geometry.Position.Y - light.Position.Y;
                    float dist = (float)Math.Sqrt((x_diff * x_diff) + (y_diff * y_diff));
                    float val = 1.0f - (dist / light.Range);
                    if (val < 0.0f) val = 0.0f;

                    float angle = (float)Math.Atan2(y_diff, x_diff);
                    float angle_diff = (float)Math.Abs(light.Direction - Math.PI / 2 - angle);
                    if (angle_diff > Math.PI)
                        angle_diff = 2 * (float)Math.PI - angle_diff;
                    float angle_val = 1.0f - (angle_diff / light.Radius);

                    Vector4 pre = light.Colour.ToVector4();
                    Vector4 dis = new Vector4(val, val, val, 1.0f);
                    Vector4 ang = new Vector4(angle_val, angle_val, angle_val, 1.0f);
                    Vector4 result = pre * dis * ang;
                    if (result.X < 0.0f) result.X = 0.0f;
                    if (result.Y < 0.0f) result.Y = 0.0f;
                    if (result.Z < 0.0f) result.Z = 0.0f;
                    if (result.W < 0.0f) result.W = 0.0f;
                    _Lighting += result;
                }
                Vector4 _VectorOverlay = _ColourOverlay.ToVector4();

                _ActualColour = new Color(_VectorOverlay * _Lighting);
            }
            else
                _ActualColour = _ColourOverlay;

            Animation a = _Animations.Current;
            Vector2 pos = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Geometry.Position);
            Vector2 size = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(_Geometry.UncompensatedSize);
            Vector2 origin = new Vector2(-_Geometry.MinX()/(_Geometry.MaxX() - _Geometry.MinX()) * _Animations.Current.WidthPerCell, -_Geometry.MinY()/(_Geometry.MaxY() - _Geometry.MinY()) * _Animations.Current.HeightPerCell);
            Rectangle dest = new Rectangle((int)(pos.X), (int)(pos.Y), (int)size.X, (int)size.Y);
            batch.Draw(a.Texture, dest, a.UpdateSource(time), _ActualColour, (float)_Geometry.Direction, origin, SpriteEffects.None, _Depth);
        }

        #endregion
    }
}
