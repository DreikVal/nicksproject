using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class FloatingText : Entity
    {
        protected float _Damping;
        protected String _Text;
        protected SpriteFont _Font;

        public FloatingText(Screen parent, Vector2 position, float speed, float damping, String text, String font, Color colour, int lifeTime) : 
            base(parent.Entities, position, 50f, 28f, 0f)
        { 
            _Text = text;
            _Font = Application.AppReference.Content.Load<SpriteFont>(font);
            _ColourOverlay = colour;
            _LifeTime = lifeTime;
            _Damping = damping;
            _Speed = speed;
        }

        public override string Initialize()
        {
            // Set crosshair to front of screen
            _Depth = 0.15f;
            _Temporary = true;

            // Return the name for this class
            return "FloatingText";
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            if (Disposed) return;

            // Draw the message string
            if (_Text != null && _Font != null)
            {
                Vector2 Length = _Font.MeasureString(_Text);
                Vector2 TopLeft_Pixels = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Position) - Length/2;
                Vector2 Size_Pixels = Length;

                batch.DrawString(_Font, _Text, TopLeft_Pixels, _ColourOverlay, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, Depth);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _Speed *= _Damping;
        }
    }
}
