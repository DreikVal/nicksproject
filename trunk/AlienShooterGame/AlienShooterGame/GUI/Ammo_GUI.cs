using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Ammo_Gui : Entity
    {
        protected Screen parent;
        protected Ammo_Gui_Bullet[] bullets;
        protected Vector2 firstPos = new Vector2(794f, 422f);
        protected Vector2 increment = new Vector2(-7f, 0f);

        public Ammo_Gui(Screen Parent)
            : base(Parent)

        {
            this.parent = Parent;
            DynamicLighting = false;
            Depth = 0.19f;
        }

        public override string Initialize()
        {
            bullets = new Ammo_Gui_Bullet[Marine.ClipSize];
            for (int i = 0; i < Marine.ClipSize; i++)
                bullets[i] = new Ammo_Gui_Bullet(_Parent, firstPos + (i * increment));

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

            Screen screen;
            WorldScreen world;
            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                int index = world.Player.Ammo;
                if (world.Player.Reloading) index = 0;
                for (int i = 0; i < Marine.ClipSize; i++)
                {
                    if (i < index)
                        bullets[i].Hide = false;
                    else
                        bullets[i].Hide = true;
                }
            }
            catch (Exception) { }
        }


    }
}
