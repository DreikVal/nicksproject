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

            Animation a = _Animations.Current;
            Vector2 pos = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Geometry.Position);
            Vector2 size = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(_Geometry.UncompensatedSize);
            Vector2 origin = new Vector2(-_Geometry.MinX()/(_Geometry.MaxX() - _Geometry.MinX()) * _Animations.Current.WidthPerCell, -_Geometry.MinY()/(_Geometry.MaxY() - _Geometry.MinY()) * _Animations.Current.HeightPerCell);
            Rectangle dest = new Rectangle((int)(pos.X), (int)(pos.Y), (int)size.X, (int)size.Y);
            batch.Draw(a.Texture, dest, a.UpdateSource(time), Color.White, (float)_Geometry.Direction, origin, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
