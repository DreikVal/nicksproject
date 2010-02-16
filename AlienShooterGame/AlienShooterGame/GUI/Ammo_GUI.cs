using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    public class Ammo_Gui : Entity
    {
        Screen parent;
        Ammo_Gui_Bullet bullets;
        public Ammo_Gui(Screen Parent)
            : base(Parent)

        {
            this.parent = Parent;
            DynamicLighting = false;
            Depth = 0.19f;
        }

        public override string Initialize()
        {
            bullets = new Ammo_Gui_Bullet(Parent);
            bullets.Geometry.Position.X = Parent.ViewPort.Size.X - 90;
            bullets.Geometry.Position.Y = Parent.ViewPort.Size.Y - 35;

            Geometry = Geometry.CreateRectangularGeometry(this, 70, 180);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("ammobox", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Ammo_Gui";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }


    }
}
