using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Bullet : Entity
    {
        public static float Speed = 1.8f;
        public static int LifeTime = 2000;

        protected int _Remaining = LifeTime;

        public Bullet(Screen parent, Vector2 position, double direction) : base(parent) 
        {
            _Geometry.Position = position;
            _Geometry.Direction = direction;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 2.0f, 56.0f, 0.0f, 3.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("bullet", "Normal", 1, 1, 8.0f));

            // Set crosshair to front of screen
            _Depth = 0.82f;

            // Flag as an active collision entity
            CollisionType = CollisionType.Active;

            // Return the name for this class
            return "Bullet";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            _Remaining--;

            if (_Remaining < 0) Dispose();

            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * Speed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * Speed * time.ElapsedGameTime.Milliseconds;          
        }

        protected override void HandleCollision(Entity otherEnt)
        {
            base.HandleCollision(otherEnt);

            if (otherEnt as Marine == null && otherEnt as Bullet == null)
                Dispose();
        }
    }
}
