using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SituationSticky
{
    public class Bullet : Entity
    {
        #region Constants

        public const int        DamageBase              = 15;
        public const int        DamageVar               = 25;
        public const float      BulletSpeed             = 1.5f;
        public const int        BulletLifeTime          = 750;
        public static int[]     DefaultCollisionPeriod  = { 75, 50, 40, 25, 18 };

        #endregion

        #region Members

        /// <summary>
        /// Gets or sets the entity who owns this bullet (he who fired it.)
        /// </summary>
        public Entity Owner { get { return _Owner; } set { _Owner = value; } }
        protected Entity _Owner;

        /// <summary>
        /// Gets or sets the damage that this bullet will inflict.
        /// </summary>
        public int Damage { get { return _Damage; } set { _Damage = value; } }
        protected int _Damage;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new bullet entity.
        /// </summary>
        /// <param name="parent">The screen for the bullet.</param>
        /// <param name="owner">The owner of the bullet.</param>
        /// <param name="position">The position of the bullet.</param>
        /// <param name="direction">The direction of the bullet.</param>
        public Bullet(Screen parent, Entity owner, Vector3 position, Vector2 direction) : base(parent.Entities, position, new Vector3(2,56,2), direction) 
        {
            _Owner = owner;
        }

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/Misc/Bullet01_1x1", "Normal", 1, 1, 8.0f));

            // Settings
            _Depth = 0.82f;
            _CollisionRadius = 3f;
            _CollisionType = CollisionType.Active;
            _DynamicLighting = false;
            _LifeTime = BulletLifeTime;
            _Temporary = true;
            _Speed = BulletSpeed;
            _Damage = DamageBase + Application.AppReference.Random.Next(DamageVar);
            _CollisionPeriod = DefaultCollisionPeriod[Application.AppReference.GfxLevel];

            return "Bullet";
        }

        #endregion

        #region Utility

        protected override void HandleCollision(Entity otherEnt)
        {
            base.HandleCollision(otherEnt);

            // If collides with anything other than a marine or another bullet, destroy the bullet.
            if (otherEnt as Marine == null && otherEnt as Bullet == null)
                Dispose();
        }

        #endregion
    }
}
