using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Drone : Entity
    {
        #region Constants

        public static int[]         BloodOnHit              = { 3, 6, 10, 15, 25 };
        public static int[]         BloodOnDeath            = { 7, 11, 18, 26, 40 };
        public const float          DefaultCollisionRadius  = 20f;
        public static int[]         DefaultCollisionPeriod  = { 150, 115, 90, 65, 50 };
        public static Color         BloodColour             = Color.Green;
        public const float          BloodSizeBase           = 5f;
        public const float          BloodSizeVar            = 18f;
        public const int            BloodLifeTime           = 650;
        public const float          BloodSpeedBase          = 0.1f;
        public const float          BloodSpeedVar           = 0.3f;
        public const float          BloodSpeedDamp          = 0.92f;
        public const int            DamagePerHit            = 2;
        public const int            MaxHP                   = 100;
        public const float          DroneSpeed              = 0.14f;
        public const int            Bounty                  = 50;
        public const float          BulletKnockback         = 0.7f;
        public const float          MarineKnockback         = 1.2f;

        #endregion

        #region Members

        /// <summary>
        /// The entity that this drone will lock onto and attack.
        /// </summary>
        public Entity Target { get { return _Target; } set { _Target = value; } } 
        protected Entity _Target;

        /// <summary>
        /// The current hitpoints of this drone.
        /// </summary>
        public int CurrentHP { get { return _CurrentHP; } set { _CurrentHP = value; } }
        protected int _CurrentHP;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new drone entity.
        /// </summary>
        /// <param name="parent">The screen to create the drone on.</param>
        /// <param name="position">The position of the drone.</param>
        /// <param name="target">The target that this drone will attack.</param>
        public Drone(Screen parent, Vector2 position, Entity target) : base(parent.Entities, position, 44f, 44f, 0f) 
        {
            _Target = target;
        }

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/Enemies/Drone03_1x5", "Normal", 1, 5, 8.0f));
            _Animations.PlayAnimation("Normal");

            // Settings
            _Depth = 0.79f;
            _CurrentHP = MaxHP;
            _DynamicLighting = true;
            _CollisionType = CollisionType.Active;
            _Speed = DroneSpeed;
            _CollisionRadius = DefaultCollisionRadius;
            _CollisionPeriod = DefaultCollisionPeriod[Application.AppReference.GfxLevel];

            return "Alien";
        }

        public override void Dispose()
        {
            base.Dispose();

            // Create blood explosion on death.
            for (int i = 0; i < BloodOnDeath[Application.AppReference.GfxLevel]; i++)
                new Blood(_Parent, _Position, BloodColour, BloodSizeBase, BloodSizeVar, BloodSpeedBase, BloodSpeedVar, BloodSpeedDamp, BloodLifeTime);

            // Create a new drone to replace this one.
            Drone.CreateNearbyDrone(_Parent, _Parent.PlayerEntity, 450, _Target);
            
            // Create floating text
            new FloatingText(_Parent, _Position+new Vector2(0f,-20f), 0.12f, 0.95f, ((WorldScreen)_Parent).PlayerMarine.GiveScore(Bounty).ToString(),
                "Fonts/FloatingFont", new Color(0.6f, 0.7f, 1.0f, 0.5f), 1000);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the drone.
        /// </summary>
        /// <param name="time">The XNA gametime object.</param>
        public override void Update(GameTime time)
        {
 	        base.Update(time);

            if (Disposed) return;

            // Check HP
            if (_CurrentHP <= 0)
                Dispose();

            // Reacquire target if dead or not applied.
            if (_Target == null || _Target.Disposed) _Target = _Parent.PlayerEntity;

            // Make drone face his target.
            float x_diff = _Position.X - _Target.Position.X;
            float y_diff = _Position.Y - _Target.Position.Y;
            _Direction = (float) (Math.Atan2(y_diff, x_diff) - Math.PI / 2);

            // Allow drone to regain his speed up to maximum if he has been stunned earlier.
            if (_Speed < DroneSpeed)
                _Speed *= 1.01f;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Handle the collisions between drones and other entities
        /// </summary>
        /// <param name="otherEnt">The entity with which the drone has collided</param>
        protected override void HandleCollision(Entity otherEnt)
        {
            base.HandleCollision(otherEnt);

            if (otherEnt as Bullet != null)
            {
                // Apply bullet damage to drone
                Bullet bullet = otherEnt as Bullet;
                _CurrentHP -= bullet.Damage;

                // Create blood explosion
                for (int i = 0; i < BloodOnHit[Application.AppReference.GfxLevel]; i++)
                    new Blood(_Parent, _Position, BloodColour, BloodSizeBase, BloodSizeVar, BloodSpeedBase, BloodSpeedVar, BloodSpeedDamp, BloodLifeTime);
                
                // Calculate angle between bullet and drone
                Vector2 diff = otherEnt.Position - _Position;
                double angle = Math.Atan2(diff.Y, diff.X);
                
                // Apply knockback and stun to drone
                _Position.X += (float)((_CollisionRadius + otherEnt.CollisionRadius) * Math.Cos(_Direction - Math.PI) * BulletKnockback);
                _Position.Y += (float)((_CollisionRadius + otherEnt.CollisionRadius) * -Math.Sin(_Direction - Math.PI) * BulletKnockback);
                _Speed *= 0.60f;
            }
            else if (otherEnt as Drone != null)
            {
                
                // Apply small knockback between multiple drones
                Vector2 diff = _Position - otherEnt.Position;
                double angle = Math.Atan2(diff.Y, diff.X);
                otherEnt.SetXPosition((float)(_Position.X + (otherEnt.CollisionRadius + _CollisionRadius) * -Math.Cos(angle)));
                otherEnt.SetYPosition((float)(_Position.Y + (otherEnt.CollisionRadius + _CollisionRadius) * -Math.Sin(angle)));
            }
            else if (otherEnt as Marine != null)
                AttackPlayer(otherEnt as Marine);
        }

        /// <summary>
        /// Applies damage to the marine.
        /// </summary>
        /// <param name="player">Reference to the player controlled marine.</param>
        protected virtual void AttackPlayer(Marine player)
        {
            // Apply damage to marine
            player.CurrentHP -= DamagePerHit;

            // Shake screen
            Parent.ViewPort.Shake(3.0f, 0.8f, 0.95f);

            // Create blood explosion
            for (int i = 0; i < Marine.BloodParticles[Application.AppReference.GfxLevel]; i++)
                new Blood(_Parent, player.Position, Marine.BloodColour, Marine.BloodSizeBase, Marine.BloodSizeVar, Marine.BloodSpeedBase, 
                    Marine.BloodSpeedVar, Marine.BloodSpeedDamp, Marine.BloodLifeTime);

            // Calculate angle difference
            Vector2 diff = _Position - player.Position;
            double angle = Math.Atan2(diff.Y, diff.X);

            // Knockback marine
            player.SetXPosition( (float)(_Position.X + ((player.CollisionRadius + _CollisionRadius) * -Math.Cos(angle) * MarineKnockback)) );
            player.SetYPosition( (float)(_Position.Y + ((player.CollisionRadius + _CollisionRadius) * -Math.Sin(angle) * MarineKnockback)) );
        }

        /// <summary>
        /// Creates a drone near to the given entity.
        /// </summary>
        /// <param name="scr">The screen on which to create the drone.</param>
        /// <param name="nearTo">The entity that the drone should be placed near.</param>
        /// <param name="distance">The distance from the entity the drone should be placed.</param>
        /// <param name="target">The target for the drone to attack.</param>
        /// <returns></returns>
        public static Drone CreateNearbyDrone(Screen scr, Entity nearTo, float distance, Entity target)
        {
            double angle = Application.AppReference.Random.NextDouble() * 2 * Math.PI;
            Vector2 disp = new Vector2(distance * (float)Math.Cos(angle), distance * (float)Math.Sin(angle));
            return new Drone(scr, nearTo.Position + disp, target);
        }

        #endregion
    }
}
