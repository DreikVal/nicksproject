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

namespace SituationSticky
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


        public EntityList List { get { return _List; } }
        protected EntityList _List;

        /// <summary>
        /// Gets the class name of this entity, as used in network communication.
        /// </summary>
        public String EntityClass { get { return _EntityClass; } }
        protected String _EntityClass = "Entity";

        /// <summary>
        /// Gets or sets the position of the centre of this entity in world coordinates.
        /// </summary>
        public Vector3 Position { get { return _Position; } set { _Position = value; } }
        protected Vector3 _Position = new Vector3();

        /// <summary>
        /// Gets or sets the direction in radians of this entity. (0.0f = North)
        /// </summary>
        public Vector3 Direction { get { return _Direction; } set { _Direction = value; } }
        protected Vector3 _Direction = new Vector3();

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

        /// <summary>
        /// Gets or sets a value to determine whether the entity should be rendered.
        /// </summary>
        public bool Hide { get { return _Hide; } set { _Hide = value; } }
        protected bool _Hide = false;
        
        /// <summary>
        /// Gets or sets a value to determine what type of collision detection this entity uses.
        /// </summary>
        public CollisionType CollisionType { get { return _CollisionType; } set { _CollisionType = value; } }
        protected CollisionType _CollisionType = CollisionType.None;

        /// <summary>
        /// Gets or sets the time between active collision checks for this entity.
        /// </summary>
        public int CollisionPeriod { get { return _CollisionPeriod; } set { _CollisionPeriod = value; } }
        protected int _CollisionPeriod = 125;
        protected int _CollisionCooldown;

        /// <summary>
        /// Gets the actual colour of the object (after dynamic lighting effects have been applied.)
        /// </summary>
        public Color ActualColour { get { return _ActualColour; } }
        protected Color _ActualColour = Color.White;

        /// <summary>
        /// Gets or sets whether the entity casts shadows on other entities.
        /// </summary>
        public bool Opaque { get { return _Opaque; } set { _Opaque = value; } }
        protected bool _Opaque = false;

        public float Radius { get { return _Radius; } }
        protected float _Radius = 0.0f;

        /// <summary>
        /// Gets or sets the size of the entity. (Interchangeable with Width and Height properties.)
        /// </summary>
        public Vector3 Size { get { return _Size; } set { _Size = value; CalculateRadius(); } }
        protected Vector3 _Size;

        /// <summary>
        /// Gets or sets the radius of collisions for this entity.
        /// </summary>
        public float CollisionRadius { get { return _CollisionRadius; } set { _CollisionRadius = value; } }
        protected float _CollisionRadius;

        /// <summary>
        /// Gets or sets the shadow object that belongs to this entity.
        /// </summary>
        public ShadowRegion Shadow { get { return _Shadow; } set { _Shadow = value; } }
        protected ShadowRegion _Shadow;

        /// <summary>
        /// Gets or sets whether an entity has a finite lifetime.
        /// </summary>
        public bool Temporary { get { return _Temporary; } set { _Temporary = value; } }
        protected bool _Temporary = false;

        /// <summary>
        /// Gets or sets the lifetime in milliseconds of this entity. (Only used if Entity.Temporary = true)
        /// </summary>
        public int LifeTime { get { return _LifeTime; } set { _LifeTime = value; } }
        protected int _LifeTime = 2000;

        /// <summary>
        /// Gets or sets the speed of the object.
        /// </summary>
        public float Speed { get { return _Speed; } set { _Speed = value; } }
        protected float _Speed = 0.0f;

        /// <summary>
        /// Gets or sets the speed of the object.
        /// </summary>
        public Vector3 Spin { get { return _Spin; } set { _Spin = value; } }
        protected Vector3 _Spin = Vector3.Zero;

        // Variables used to render object
        protected Animation _Draw_Animation;
        protected Vector2 _Draw_Origin, _Draw_Position, _Draw_Size;
        protected Rectangle _Draw_Destination, _Draw_Source;
        protected bool _Draw_OnScreen = false;

        /// <summary>
        /// Gets or sets the definition ID used to create an instance of this entity.
        /// </summary>
        public int DefinitionID { get { return _DefinitionID; } set { _DefinitionID = value; } }
        protected int _DefinitionID = -1;
        
        #endregion


        #region Initialisation and Disposal

        /// <summary>
        /// Constructs a new entity.
        /// </summary>
        /// <param name="parent">The screen to which the entity belongs.</param>
        /// <param name="position">The world location of the entity.</param>
        /// <param name="width">The width of the entity.</param>
        /// <param name="height">The height of the entity.</param>
        /// <param name="direction">The direction of the entity.</param>
        public Entity(EntityList list, Vector3 position, Vector3 size, Vector3 direction)
        {
            // Retrieve entity manager
            if (list == null)
                throw new Exception("Cannot create entity with a null Entity list.");
            _List = list;
            _Parent = list.Parent;

            // Set entity ID
            _ID = _EntityCount++;

            // Set geometrical settings
            _Position = position;
            _Size = size;
            _Direction = direction;
            CalculateRadius();

            // Allow inherited classes to initialize entity settings before finalizing construction
            _EntityClass = Initialize();

            // Register this entity with the entity manager
            _List.Loaded.Add(_ID, this);
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
            if (_Shadow != null) _Shadow.Dispose();
            _List.Loaded.Remove(_ID);
            _List.Unloaded.Remove(_ID);
        }

        #endregion


        #region Update

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="time">XNA game time.</param>
        public virtual void Update(GameTime time)
        {
            // Do nothing for disposed entities
            if (_Disposed) return;

            // Handle temporary entities
            if (_Temporary)
            {
                _LifeTime -= time.ElapsedGameTime.Milliseconds;
                if (_LifeTime <= 0) { Dispose(); return; }
            }

            // Update position and direction
            //_Position.X += (float)(Math.Sin(_Direction.Z) * Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds);
            //_Position.Y += -(float)(Math.Cos(_Direction.Z) * Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds);
            //_Position.Z += (float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Position.X += (float)(Math.Sin(_Direction.Z) * Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds);
            _Position.Y += -(float)(Math.Cos(_Direction.Z) * Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds);
            _Position.Z += (float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Direction += _Spin * time.ElapsedGameTime.Milliseconds;

            // Check collisions
            if (_CollisionType == CollisionType.Active)
            {
                _CollisionCooldown -= time.ElapsedGameTime.Milliseconds;
                if (_CollisionCooldown <= 0)
                {
                    _CollisionCooldown += _CollisionPeriod;
                    CheckCollisions();
                }  
            }
        }

        /// <summary>
        /// This method is called in a separate thread to the regular Update and is given the expensive but sub-critical tasks like Lighting 
        /// calculations to speed up performance.
        /// </summary>
        public virtual void BackgroundUpdate()
        {
            if (_Parent.DynamicLighting && _DynamicLighting)
            {
                _Lighting = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                _Parent.Lights.Loaded.ForEach(ForEachLight, null, null, null);
                Vector4 _VectorOverlay = _ColourOverlay.ToVector4();

                _ActualColour = new Color(_VectorOverlay * _Lighting);
            }
            else
                _ActualColour = _ColourOverlay;
        }
        private bool ForEachLight(Entity oLight, object p1, object p2, object p3)
        {
            LightSource light = (LightSource)oLight;

            if (!light.Active) return true;

            if (!TraceRay(light)) return true;

            float x_diff = Position.X - light.Position.X;
            float y_diff = Position.Y - light.Position.Y;
            float dist = (float)Math.Sqrt((x_diff * x_diff) + (y_diff * y_diff));
            float val = 1.0f - (dist / light.Range);
            if (val < 0.0f) val = 0.0f;

            float angle = (float)Math.Atan2(y_diff, x_diff);
            float angle_diff = (float)Math.Abs(light.Direction.Z - Math.PI / 2 - angle);
            if (angle_diff > Math.PI)
                angle_diff = 2 * (float)Math.PI - angle_diff;
            float angle_val = 1.0f - (angle_diff / light.Bandwidth);

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


        /// <summary>
        /// This function checks whether or not there is a clear path from the given lightsource to this entity, checking all shadow regions.
        /// </summary>
        /// <param name="light">The light source we are checking the path to.</param>
        /// <returns>True if there is a clear path to the light source.</returns>
        protected virtual bool TraceRay(LightSource light)
        {
            return _Parent.Shadows.Loaded.ForEach(ForEachRay, light, null, null);
        }
        private bool ForEachRay(Entity oShadow, object p1, object p2, object p3)
        {
            ShadowRegion shadow = (ShadowRegion)oShadow;

            if (_Shadow == shadow) return true;
            LightSource light = (LightSource)p1;

            Vector3 dir = light.Position - Position;
            Vector3 diff = shadow.Position - Position;
            float t = (diff.X * dir.X + diff.Y * dir.Y) / (dir.X * dir.X + dir.Y * dir.Y);
            if (t < 0.0f)
                t = 0.0f;
            if (t > 1.0f)
                t = 1.0f;
            Vector3 closest = Position + t * dir;
            Vector3 d = shadow.Position - closest;
            float distsqr = d.X * d.X + d.Y * d.Y;
            return distsqr > shadow.Radius * shadow.Radius;
        }

        #endregion


        #region Draw

        /// <summary>
        /// Renders the entity on screen. The calculations for this task have been completed in Update.
        /// </summary>
        /// <param name="time">XNA game time.</param>
        /// <param name="batch">The spritebatch to render to.</param>
        public virtual void Draw(GameTime time, SpriteBatch batch)
        { }

        #endregion


        #region Utility

        /// <summary>
        /// Checks whether this entity is inside the screens viewport.
        /// </summary>
        /// <returns>True if on screen.</returns>
        public bool isOnScreen()
        {
            if (Position.X + Radius < _Parent.ViewPort.ActualLocation.X ||
                Position.Y + Radius < _Parent.ViewPort.ActualLocation.Y ||
                Position.X - Radius > _Parent.ViewPort.ActualLocation.X + _Parent.ViewPort.Size.X ||
                Position.Y - Radius > _Parent.ViewPort.ActualLocation.Y + _Parent.ViewPort.Size.Y)
                return false;
            return true;
        }

        /// <summary>
        /// Checks whether this entity is inside the screens loadport.
        /// </summary>
        /// <returns>True if inside LoadPort region.</returns>
        public bool isInLoadPort()
        {
            if (Position.X + Radius < _Parent.LoadPort.Location.X ||
                Position.Y + Radius < _Parent.LoadPort.Location.Y ||
                Position.X - Radius > _Parent.LoadPort.Location.X + _Parent.LoadPort.Size.X ||
                Position.Y - Radius > _Parent.LoadPort.Location.Y + _Parent.LoadPort.Size.Y)
                return false;
            return true;
        }

        /// <summary>
        /// Checks whether this entity is colliding with any other entity. When a collision is found, HandleCollision is called.
        /// </summary>
        protected virtual void CheckCollisions()
        {
            _Parent.Entities.Loaded.ForEach(ForEachCollisionCheck, null, null, null);
            _Parent.Walls.Loaded.ForEach(ForEachCollisionCheck, null, null, null);
        }
        private bool ForEachCollisionCheck(Entity ent, object p1, object p2, object p3)
        {
            if (ent.CollisionType == CollisionType.None) return true;
            if (ent == this) return true;

            if (Collision(ent))
            {
                HandleCollision(ent);
                ent.HandleCollision(this);
            }
            return true;
        }

        /// <summary>
        /// This function can be overriden to handle collisions for different types of entities.
        /// </summary>
        /// <param name="ent">The other entity with which this one has collided.</param>
        protected virtual void HandleCollision(Entity ent) { }

        /// <summary>
        /// Checks whether this entity collides with the given entity.
        /// </summary>
        /// <param name="ent">Entity to test collision against.</param>
        /// <returns>True if the entities have collided.</returns>
        protected bool Collision(Entity ent)
        {
            Vector3 diff = Position - ent.Position;
            if (diff.Length() < CollisionRadius + ent.CollisionRadius)
                return true;

            return false;
        }

        /// <summary>
        /// Calculations the radius of the smallest circle that encompasses this entity.
        /// </summary>
        protected virtual void CalculateRadius()
        {
            _Radius = _Size.Length();
        }

        public virtual void SetPosition(Vector3 pos) { _Position = pos; }
        public virtual void SetXPosition(float xpos) { _Position.X = xpos; }
        public virtual void SetYPosition(float ypos) { _Position.Y = ypos; }
        public virtual void SetZPosition(float zpos) { _Position.Z = zpos; }


        #endregion
    }

    /// <summary>
    /// An enumeration for collision checking.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// Actively searches against all other entities for collisions.
        /// </summary>
        Active,

        /// <summary>
        /// Can only collide with active objects, good for static entities. This speeds up collision detection.
        /// </summary>
        Passive,

        /// <summary>
        /// Flags an entity as uncollidable with any other entity.
        /// </summary>
        None
    }
}
