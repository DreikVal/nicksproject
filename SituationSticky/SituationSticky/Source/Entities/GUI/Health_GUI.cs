﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Health_GUI : Entity_Quad
    {
        #region Constants

        public const String FontName = "Fonts/DefaultFont";
        public static Vector2 TextOffset = new Vector2(25, -13);

        #endregion

        #region Members

        /// <summary>
        /// Text that displays the players current HP.
        /// </summary>
        protected String _HPText;

        /// <summary>
        /// The font for the HP Text.
        /// </summary>
        protected SpriteFont _Font;

        /// <summary>
        /// The percentage of HP that player has left.
        /// </summary>
        protected float _PercentHP;

        /// <summary>
        /// The location of the HP text.
        /// </summary>
        protected Vector2 _TextLocation;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new Health_GUI object.
        /// </summary>
        /// <param name="Parent">The screen for the health gui.</param>
        /// <param name="position">The location of the health gui.</param>
        public Health_GUI(Screen Parent, Vector3 position)
            : base(Parent.Entities, position, new Vector3(50, 45, 0), Vector3.Zero) { }

        public override string Initialize()
        {
            base.Initialize();

            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/Health03_1x1", "Normal", 1, 1, 1.0f));

            // Settings
            _Depth = 0.19f;
            _DynamicLighting = false;
            _Font = Application.AppReference.Content.Load<SpriteFont>(FontName);
            _HPText = "Loading..";

            return "Health_GUI";
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);
            Marine player = ((WorldScreen)_Parent.Manager.GetScreen("World")).PlayerMarine;
            int hp = player.CurrentHP <= 0 ? 0 : player.CurrentHP;
            _HPText = hp.ToString();
            _PercentHP = (float)hp / (float)Marine.MaxHP;
            _ColourOverlay = new Color(1f - _PercentHP, _PercentHP, 0f, 0.45f);
            _ActualColour = _ColourOverlay;
            //_TextLocation = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Position + TextOffset);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(time, batch);
            batch.DrawString(_Font, _HPText, _TextLocation, _ColourOverlay, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }

        #endregion

    }
}
