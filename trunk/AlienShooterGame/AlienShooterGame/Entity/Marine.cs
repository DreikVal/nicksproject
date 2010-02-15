using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    class Marine : Entity
    {
        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = Geometry.CreateCircularGeometry(this, 20.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("soldier", "Normal", 1, 1, 10.0f));

            // Return the name for this class
            return "Marine";
        }
    }
}
