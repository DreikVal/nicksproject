using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    public class Ammo_Gui_Bullet : Entity
    {
        public Ammo_Gui_Bullet(Screen Parent)
            : base(Parent)

        {
            DynamicLighting = false;
            Depth = 0.18f;
        }

        public override string Initialize()
        {
            Geometry = Geometry.CreateRectangularGeometry(this, 44, 7);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("bullet", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Ammo_Gui_Bullet";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }


    }
}
