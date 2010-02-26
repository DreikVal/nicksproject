using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Ammo_GUI : Entity
    {
        protected Screen parent;
        //protected Bullet_GUI[] bullets;
        protected Vector2 firstPos = new Vector2(794f, 422f);
        protected Vector2 increment = new Vector2(-7f, 0f);
        protected Vector2 reloadOffset = new Vector2(500, 495);
        protected int bulletIndex;
        protected Texture2D _BarDull, _Bar;

        public Ammo_GUI(Screen Parent, Vector2 position)
            : base(Parent.Entities, position, 180f, 70f, 0f)

        {}

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/AmmoBox01_1x1", "Normal", 1, 1, 1.0f));
            _BarDull = Application.AppReference.Content.Load<Texture2D>("Textures/GUI/ProgressDull01_1x1");
            _Bar = Application.AppReference.Content.Load<Texture2D>("Textures/GUI/ProgressBar01_1x1");

            // Settings
            _Depth = 0.19f;
            _DynamicLighting = false;

            return "Ammo_GUI";
        }

        public override void Draw(GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            /*
            Screen screen;
            WorldScreen world;
            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;

                //get weapon name
                
                batch.DrawString(getFont(), world.Player.currentWeapon.getName(),
                    new Vector2(this.Geometry.Position.X + reloadOffset.X,
                        this.Geometry.Position.Y + reloadOffset.Y - 140),
                        Color.Red,
                        0.0f,
                        Vector2.Zero,
                        2.5f,
                        SpriteEffects.None,
                        0.0f);
            }
            
            catch{}


            if (bulletIndex == -1)
                    batch.DrawString(getFont(), "Reloading...",
                        new Vector2(_Position.X + reloadOffset.X,
                            _Position.Y + reloadOffset.Y),
                            Color.Red,
                            0.0f,
                            Vector2.Zero,
                            3.0f,
                            SpriteEffects.None,
                            0.0f);
            base.Draw(time, batch);
            */

            //batch.Draw(_BarDull, new Rectangle(1400, 900, 128, 32), Color.White);
            //batch.Draw(_Bar, new Rectangle(1425, 900, 128-50, 28), new Color(0.2f, 0.4f, 1f, 0.8f));
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
