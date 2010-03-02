using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Score_GUI : Entity
    {
        #region Constants

        public const String FontName = "Fonts/DefaultFont";
        public static Color ScoreColour = new Color(0.9f, 0.9f, 0.9f, 0.5f);
        public static Color FlashColour = new Color(0.5f, 0.7f, 1f, 0.5f);
        public const int FlashTime = 1200;

        #endregion

        #region Members

        /// <summary>
        /// The font for the score text.
        /// </summary>
        protected SpriteFont _Font;

        /// <summary>
        /// The text to be displayed.
        /// </summary>
        protected String _Text;

        /// <summary>
        /// The time in milliseconds remaining on the "Flash" which occurs when the marine' score changes.
        /// </summary>
        protected int _FlashRemaining;

        /// <summary>
        /// The last score of the marine, used to set off Flashes.
        /// </summary>
        protected int _LastScore;

        /// <summary>
        /// The difference in colour between FlashColour and ScoreColour
        /// </summary>
        private Vector4 _ColourDiff;

        /// <summary>
        /// The location of the score text.
        /// </summary>
        private Vector2 _TextLocation;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new Score gui element.
        /// </summary>
        /// <param name="Parent">The screen for the score entity.</param>
        /// <param name="position">The location of the score.</param>
        public Score_GUI(Screen Parent, Vector3 position)
            : base(Parent.Entities, position, new Vector3(50, 50, 0), Vector3.Zero) { }

        public override string Initialize()
        {
            base.Initialize();

            // Animations

            // Settings
            _DynamicLighting = false;
            _Depth = 0.19f;
            _Font = Application.AppReference.Content.Load<SpriteFont>(FontName);
            _Text = "Score: ";
            _FlashRemaining = 0;
            _ColourDiff = FlashColour.ToVector4() - ScoreColour.ToVector4();
            _LastScore = 0;

            return "Health_GUI";
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);

            // Find current score
            Marine player = ((WorldScreen)_Parent.Manager.GetScreen("World")).PlayerMarine;
            _Text = "Score: " + player.Score.ToString();
            //_TextLocation = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Position);

            // Check for change in score
            if (player.Score != _LastScore)
            {
                _FlashRemaining = FlashTime;
                _LastScore = player.Score;
            }

            // Update flash timers
            if (_FlashRemaining > 0) _FlashRemaining -= time.ElapsedGameTime.Milliseconds;
            if (_FlashRemaining < 0) _FlashRemaining = 0;

            // Calculate appropriate colour gradient
            float ratio = (float)_FlashRemaining / (float)FlashTime;
            _ColourOverlay = new Color(ScoreColour.ToVector4() + ratio * _ColourDiff);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.DrawString(_Font, _Text, _TextLocation, _ColourOverlay, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }

        #endregion
    }
}
