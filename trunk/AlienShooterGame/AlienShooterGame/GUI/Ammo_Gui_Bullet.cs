using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Ammo_Gui_Bullet : Entity
    {
        public Ammo_Gui_Bullet(Screen Parent, Vector2 position)
            : base(Parent)
            
        {
            DynamicLighting = false;
            Depth = 0.18f;
            _Geometry.Position = position;
        }

        public override string Initialize()
        {
            _Geometry = new Geometry(this, new Vector2(), 6.0f, 28.0f, 0.0f, 12.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("bullet_gui", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Ammo_Gui_Bullet";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }

         //Screen screen;
         //   WorldScreen world;
         //   _Manager.LookupScreen("World", out screen);
         //   world = (WorldScreen)screen;
         //   world.Player.Ammo.FUCKYEAH



    }
}
