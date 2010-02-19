using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Health_Gui : Entity
    {
        protected Screen parent;
        protected int currentHP;
        protected Vector2 Position;
        protected Vector2 healthOffset = new Vector2 (100, 485);

        public Health_Gui(Screen Parent)
            : base(Parent)

        {
            this.parent = Parent;
            DynamicLighting = false;
            Depth = 0.19f;
        }

        public override string Initialize()
        {
            Geometry = Geometry.CreateRectangularGeometry(this, 45, 50);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("health", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Health_Gui";
        }

        private int getHP()
        {
            Screen screen;
            WorldScreen world;

            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                return world.Player.CurrentHP;
            }
            catch (Exception) { }
            return 0;
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

        public override void Draw(GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.DrawString(getFont(), getHP().ToString(),
                new Vector2(this.Geometry.Position.X + healthOffset.X,
                    this.Geometry.Position.Y + healthOffset.Y),
                    Color.Orange,
                    0.0f,
                    Vector2.Zero,
                    3.0f,
                    SpriteEffects.None,
                    0.0f);

            base.Draw(time, batch);
        }

    }
}
