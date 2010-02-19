using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Ammo_Gui : Entity
    {
        protected Screen parent;
        protected Ammo_Gui_Bullet[] bullets;
        protected Vector2 firstPos = new Vector2(794f, 422f);
        protected Vector2 increment = new Vector2(-7f, 0f);
        protected Vector2 reloadOffset = new Vector2(570, 495);
        protected int bulletIndex;

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

            _Geometry = new Geometry(this, new Vector2(), 180.0f, 70.0f, 0.0f, 50.0f);

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
                bulletIndex = world.Player.Ammo;
                if (world.Player.Reloading)
                    bulletIndex = -1;
                for (int i = 0; i < Marine.ClipSize; i++)
                {
                    if (i < bulletIndex)
                        bullets[i].Hide = false;
                    else
                        bullets[i].Hide = true;
                }
            }
            catch (Exception) { }
        }

        public override void Draw(GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (bulletIndex == -1)
            {
                try
                {
                    batch.DrawString(getFont(), "Reloading...",
                        new Vector2(this.Geometry.Position.X + reloadOffset.X,
                            this.Geometry.Position.Y + reloadOffset.Y),
                            Color.Red,
                            0.0f,
                            Vector2.Zero,
                            3.0f,
                            SpriteEffects.None,
                            0.0f);
                }
                catch (Exception) { }
            }
            base.Draw(time, batch);
        }

        private SpriteFont getFont()
        {
            Screen screen;
            WorldScreen world;

            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                return world.MessageFont;
            }
            catch (Exception) { }

            return null;
        }


    }
}
