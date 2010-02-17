using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Alien : Entity
    {
        public Alien(Screen parent, Vector2 position) : base(parent) 
        {
            _Geometry.Position = position;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateCircularGeometry(this, 32.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("alien1", "Normal", 1, 1, 8.0f));

            // Set crosshair to front of screen
            _Depth = 0.79f;

            _DynamicLighting = true;

            // Flag as an active collision entity
            CollisionType = CollisionType.Active;

            // Return the name for this class
            return "Alien";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            float x_diff = _Geometry.Position.X - ((WorldScreen)_Parent).Player.Geometry.Position.X;
            float y_diff = _Geometry.Position.Y - ((WorldScreen)_Parent).Player.Geometry.Position.Y;
            _Geometry.Direction = Math.Atan2(y_diff, x_diff) - Math.PI/2;
        }

        protected override void HandleCollision(Entity otherEnt, CollisionResult result)
        {
            base.HandleCollision(otherEnt, result);

            if (otherEnt as Bullet != null)
                Dispose();
        }
    }
}
