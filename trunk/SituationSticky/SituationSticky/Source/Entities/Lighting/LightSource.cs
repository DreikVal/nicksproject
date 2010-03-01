using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class LightSource : Entity
    {
        #region Members

        /// <summary>
        /// Gets or sets the maximum distance the light can travel from this light source.
        /// </summary>
        public float Range { get { return _Range; } set { _Range = value; } }
        protected float _Range = 700.0f;

        /// <summary>
        /// Gets or sets the field of view of the light in radians. (6.28 = all directional lighting, 1.57 = 90 degree cone lighting.)
        /// </summary>
        public float Bandwidth { get { return _Bandwidth; } set { _Bandwidth = value; } }
        protected float _Bandwidth = 1.9f;

        /// <summary>
        /// Gets or sets whether this light is active.
        /// </summary>
        public bool Active { get { return _Active; } set { _Active = value; } }
        protected bool _Active = true;

        /// <summary>
        /// Gets or sets the owner of this light source.
        /// </summary>
        public Entity Owner { get { return _Owner; } set { _Owner = value; } }
        protected Entity _Owner;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new lightsource entity to provide dynamic lighting to nearby entities.
        /// </summary>
        /// <param name="parent">The screen for the light.</param>
        /// <param name="owner">The owner of the light.</param>
        /// <param name="position">The location of the light.</param>
        /// <param name="colour">The colour of the lighting.</param>
        /// <param name="range">The maximum distance of the lighting.</param>
        /// <param name="bandwidth">The field of view of the light.</param>
        /// <param name="direction">The direction in which the light is pointing.</param>
        public LightSource(Screen parent, Entity owner, Vector3 position, Color colour, float range, float bandwidth, Vector2 direction) : base(parent.Lights, position, Vector3.Zero, direction)
        {
            _Range = range;
            _Bandwidth = bandwidth;
            _ColourOverlay = colour;
            _Owner = owner;
        }

        #endregion

        #region Updates

        public override void Update(GameTime time) 
        {
            if (Disposed) return;
            if (_Owner != null)
            {
                if (_Owner.Disposed) Dispose();
                _Position = _Owner.Position;
                _Direction = _Owner.Direction;
            }
        }

        public override void BackgroundUpdate() { }

        #endregion

        #region Drawing

        public override void Draw(GameTime time, SpriteBatch batch) { }

        #endregion

    }
}
