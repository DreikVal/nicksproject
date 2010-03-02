using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Ammo_GUI : Entity_Quad
    {
        #region Constants

        public static Vector3 AmmoBarOffset = new Vector3(0, 15, 0);
        public static Vector3 AmmoBarSize = new Vector3(135, 22, 0);
        public const String TextFont = "Fonts/DefaultFont";
        public static Color TextColour = new Color(0f, 0f, 0f, 0.95f);
        public const String DullTexture = "Textures/GUI/ProgressDull01_1x1";
        public static Color DullColour = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        public const String BarTexture = "Textures/GUI/ProgressBar01_1x1";
        public static Color BarColour = new Color(1.0f, 0.2f, 0.2f, 0.25f);
        public static Vector2 TextOffset = new Vector2(80, 12);
        public static String[] ReloadingText = { "Reloading", "Reloading.", "Reloading..", "Reloading...", "Reloading....", "Reloading....." };

        #endregion

        #region Members

        // Ammo bar textures
        protected Texture2D _DullTexture, _BarTexture;

        // Text font
        protected SpriteFont _TextFont;

        protected String _Text = "Reloading";

        // Internal members for handling drawing of ammo box
        private Rectangle _Draw_DullDest, _Draw_BarDest;
        private Vector2 _TextLocation;
        private float _TextScale;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new Ammo_GUI box which displays the player's weapon situation.
        /// </summary>
        /// <param name="Parent">Screen on which to display.</param>
        /// <param name="position">Location of the ammo box.</param>
        public Ammo_GUI(Screen Parent, Vector3 position)
            : base(Parent.Entities, position, new Vector3(180, 70, 0), Vector3.Zero)
        { }

        public override string Initialize()
        {
            base.Initialize();

            // Animations
            _Animations = new AnimationSet();
            //_Animations.AddAnimation(new Animation("Textures/GUI/AmmoBox01_1x1", "Normal", 1, 1, 1.0f));
            _DullTexture = Application.AppReference.Content.Load<Texture2D>(DullTexture);
            _BarTexture = Application.AppReference.Content.Load<Texture2D>(BarTexture);

            // Settings
            _Depth = 0.19f;
            _DynamicLighting = false;
            _TextFont = Application.AppReference.Content.Load<SpriteFont>(TextFont);

            return "Ammo_GUI";
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);

            // Drawing calculations for the dull ammo box
            Vector3 dullPos = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(Position + AmmoBarOffset);
            Vector3 dullSize = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(AmmoBarSize);
            _Draw_DullDest = new Rectangle((int)(dullPos.X), (int)(dullPos.Y), (int)dullSize.X, (int)dullSize.Y);

            // Grab the Marine's weapon info to compute ammo status
            Weapon weap = ((WorldScreen)_Parent.Manager.GetScreen("World")).PlayerMarine.CurrentWeapon;
            float percAmmo = (float)weap.Ammo / (float)weap.ClipSize;
            if (percAmmo < 0) percAmmo = 0f;

            // Set reloading text
            if (weap.IsReloading)
            {
                float remaining = (float)weap.RemainingReload / (float)weap.ReloadTime;
                int index = ReloadingText.Length - (int)(remaining * (ReloadingText.Length-1)) - 1;
                _Text = ReloadingText[index];
            }
            else
                _Text = weap.Name;

            Vector2 textSize = _TextFont.MeasureString(_Text);
            _TextScale = 1f;
            if (textSize.X > dullSize.X) _TextScale = dullSize.X / textSize.X;
            else if (textSize.Y > dullSize.Y) _TextScale = dullSize.Y / textSize.Y;
            _TextLocation.X = dullPos.X - (textSize.X * _TextScale / 2);
            _TextLocation.Y = dullPos.Y - (textSize.Y * _TextScale / 2);

            // Drawing calculations for ammo box
            Vector3 barPos = dullPos;
            barPos.X += dullSize.X / 2 * (1f - percAmmo) + 1f;
            Vector3 barSize = dullSize;
            barSize.X = barSize.X * percAmmo;
            _Draw_BarDest = new Rectangle((int)(barPos.X), (int)(barPos.Y), (int)barSize.X, (int)barSize.Y);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            // Draw background box
            base.Draw(time, batch);

            // Draw ammo bars
            batch.Draw(_DullTexture, _Draw_DullDest, null, DullColour, 0f, new Vector2(_DullTexture.Width / 2, _DullTexture.Height / 2), SpriteEffects.None, 0.05f);
            batch.Draw(_BarTexture, _Draw_BarDest, null, BarColour, 0f, new Vector2(_BarTexture.Width / 2, _BarTexture.Height / 2), SpriteEffects.None, 0.04f);
            
            // Draw text
            batch.DrawString(_TextFont, _Text, _TextLocation, TextColour, 0f, new Vector2(), _TextScale, SpriteEffects.None, 0.03f);
        }

        #endregion
    }
}
