using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class ShadowRegion : Entity
    {
        #region Members

        /// <summary>
        /// Gets or sets the owner of this shadow region.
        /// </summary>
        public Entity Owner { get { return _Owner; } }
        protected Entity _Owner;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new shadow region.
        /// </summary>
        /// <param name="owner">Owner of the region.</param>
        /// <param name="position">Position of the region.</param>
        /// <param name="size">The size of the region. (Width and Height)</param>
        public ShadowRegion(Entity owner, Vector2 position, float size) : base(owner.Parent.Shadows, position, size, size, 0.0f)
        {
            _Owner = owner;
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            if (_Owner != null)
                Position = _Owner.Position;

            if (_Owner.Disposed)
                Dispose();
        }

        public override void BackgroundUpdate() { }

        #endregion

        #region Draw

        public override void Draw(GameTime time, SpriteBatch batch) { }

        #endregion

    }
}
