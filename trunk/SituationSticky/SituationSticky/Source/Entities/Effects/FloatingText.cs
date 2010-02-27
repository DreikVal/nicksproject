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
        #region Members

        /// <summary>
        /// Percentage of text speed that is maintained each update.
        /// </summary>
        protected float _Damping;

        /// <summary>
        /// What the floating text should say.
        /// </summary>
        protected String _Text;

        /// <summary>
        /// The font of the floating text.
        /// </summary>
        protected SpriteFont _Font;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new floating text entity.
        /// </summary>
        /// <param name="parent">Screen on which to create the text.</param>
        /// <param name="position">The initial location of the text.</param>
        /// <param name="speed">The velocity of the text.</param>
        /// <param name="damping">The percentage of velocity maintained each update.</param>
        /// <param name="text">What the text should say.</param>
        /// <param name="font">The content name of the font.</param>
        /// <param name="colour">The colour of the text.</param>
        /// <param name="lifeTime">How long, in milliseconds, the text should last for.</param>
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

        #endregion

        #region Draw

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            if (Disposed) return;

            // Draw the floating text
            if (_Text != null && _Font != null)
            {
                Vector2 Length = _Font.MeasureString(_Text);
                Vector2 TopLeft_Pixels = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Position) - Length/2;
                Vector2 Size_Pixels = Length;

                batch.DrawString(_Font, _Text, TopLeft_Pixels, _ColourOverlay, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, Depth);
            }
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);

            // Apply speed damping effect
            _Speed *= _Damping;
        }

        #endregion
    }
}
