using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Health_GUI : Entity
    {
        protected Screen parent;
        protected int currentHP;
        protected Vector2 healthOffset = new Vector2 (100, 485);

        public Health_GUI(Screen Parent, Vector2 position)
            : base(Parent.Entities, position, 45f, 50f, 0f)

        {}

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/Health01_1x1", "Normal", 1, 1, 1.0f));

            // Settings
            _Depth = 0.19f;
            _DynamicLighting = false;

            return "Health_GUI";
        }

        private int getHP()
        {
            Screen screen;
            WorldScreen world;

            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                return world.PlayerMarine.CurrentHP;
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
            try
            {
                batch.DrawString(getFont(), getHP().ToString(),
                    new Vector2(_Position.X + healthOffset.X,
                        _Position.Y + healthOffset.Y),
                        Color.Orange,
                        0.0f,
                        Vector2.Zero,
                        3.0f,
                        SpriteEffects.None,
                        0.0f);
            }
            catch (Exception) { }

            base.Draw(time, batch);
        }

    }
}
