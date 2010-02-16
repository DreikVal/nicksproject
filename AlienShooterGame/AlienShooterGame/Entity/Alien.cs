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
            _Depth = 0.19f;

            _DynamicLighting = true;

            // Return the name for this class
            return "Alien";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);
        }
    }
}
