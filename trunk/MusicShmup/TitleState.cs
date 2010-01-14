using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using FriendlyEngine;

namespace MusicShmup
{
    public class TitleState : GameState
    {

        SpriteBatch spriteBatch;
        SpriteFont font10;
        SpriteFont font16;
        SpriteFont font32;
        SpriteFont font48;
        SpriteFont font79;
        Text[] MenuText;

        Texture2D logo;
        Texture2D Crosshair;


        KeyboardState kState;
        MouseState mState;
        bool pressed;
        bool click;

        private float imageFadeValue = 0;
        private float textFadeValue = 0;

        public TitleState(Game game)
            : base(game)
        {
            pressed = false;
            click = false;
            font10 = Content.Load<SpriteFont>("Fonts/Calibri10");
            font16 = Content.Load<SpriteFont>("Fonts/Calibri16");
            font32 = Content.Load<SpriteFont>("Fonts/Calibri32");
            font48 = Content.Load<SpriteFont>("Fonts/Ara48");
            font79 = Content.Load<SpriteFont>("Fonts/Ara79");
            MenuText = new Text[5] {  
                new Text(new Vector2(60, 121), "Sub", font79, 79, 0),
                new Text(new Vector2(100, 204), "new game", font48, 48, 1),
                new Text(new Vector2(100, 254), "load game", font48, 48, 1),
                new Text(new Vector2(100, 304), "options", font48, 48, 1),
                new Text(new Vector2(100, 354), "quit", font48, 48, 1)};
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Crosshair = Content.Load<Texture2D>("Sprites/targethair");
            logo = Content.Load<Texture2D>("Sprites/TitleScreen/title");
        }


        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();
            mState = Mouse.GetState();

            updateFadeAsset(gameTime, 1.5, true, true, 1);
            UpdateMouse(gameTime);
            UpdateSelected();
        }

        private void UpdateMouse(GameTime gameTime)
        {
            mState = Mouse.GetState();
            bool activeSelect = false;

            for (int i = 0; i < 5; i++)
            {
                if (MenuText[i].inBound(InputHelper.MouseXY()) && MenuText[i].Selectable == 1) { Selected = i; activeSelect = true; }
            }

            if (!activeSelect) Selected = -1;

            if (mState.LeftButton == (ButtonState.Pressed)) pressed = true;

            if ((mState.LeftButton == ButtonState.Released) && pressed)
            {
                click = true;
                pressed = false;
            }

            if (click)
            {
                // if (Selected == 0 && gameTime.TotalRealTime.TotalSeconds > 3) { Manager.CurrentState = PGGameState.Battle; }
                if (Selected == 1 && gameTime.TotalRealTime.TotalSeconds > 1.5) Manager.CurrentState = PGGameState.InGame;
                // if (Selected == 2 && gameTime.TotalRealTime.TotalSeconds > 3) Manager.CurrentState = PGGameState.Upgrade;
                // if (Selected == 3 && gameTime.TotalRealTime.TotalSeconds > 3) Manager.CurrentState = PGGameState.Equipment;
                if (Selected == 3 && gameTime.TotalRealTime.TotalSeconds > 1.5) Manager.CurrentState = PGGameState.Options;
                if (Selected == 4 && gameTime.TotalRealTime.TotalSeconds > 0.25) Game.Exit();
                click = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            drawImage(spriteBatch, gameTime);
            foreach (Text txt in MenuText) { txt.Draw(spriteBatch); };

            //spriteBatch.DrawString(font10, gameTime.TotalRealTime.TotalSeconds.ToString(), new Vector2(0, 0), Color.Black);
            //spriteBatch.DrawString(font10, textFadeValue.ToString(), new Vector2(0, 10), Color.Black);
            //spriteBatch.DrawString(font10, imageFadeValue.ToString(), new Vector2(0, 20), Color.Black);

            spriteBatch.Draw(Crosshair, new Rectangle(mState.X - 10, mState.Y - 5, 20, 20), Color.White);

            spriteBatch.End();
        }
        private void UpdateSelected()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Selected == i) MenuText[i].Selected = true;
                else MenuText[i].Selected = false;
            }

        }

        #region Text Fader Methods


        private Color FadeColor(Color baseColor, float textFadeValue)
        {
            Color tempColor;
            tempColor = new Color(baseColor.R, baseColor.G, baseColor.B, (byte)textFadeValue);
            return tempColor;
        }

        private void updateFadeAsset(GameTime gameTime, double start, bool image, bool permanent, int speed)
        {
            updateAssetFade(gameTime, start, start + 3.0, true, image, speed);
            if (!permanent) updateAssetFade(gameTime, start + 4.0, start + 6.0, false, image, speed);
        }

        private void drawImage(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (gameTime.TotalRealTime.TotalSeconds > 1.50) spriteBatch.Draw(logo, new Rectangle(0, 0, 800, 600), FadeColor(Color.White, imageFadeValue));
        }


        private bool timeBetween(GameTime gameTime, double start, double end)
        {
            return (gameTime.TotalRealTime.TotalSeconds > start && gameTime.TotalRealTime.TotalSeconds < end);
        }


        private void updateAssetFade(GameTime gameTime, double start, double end, bool increment, bool image, float speed)
        {

            if (timeBetween(gameTime, start, end) && increment)
            {
                if (!image) textFadeValue += 3 * speed;
                else imageFadeValue += 3 * speed;
            }

            if (timeBetween(gameTime, start, end) && !increment)
            {
                if (!image) textFadeValue -= 3 * speed;
                else imageFadeValue -= 3 * speed;
            }

            if (textFadeValue < 0) textFadeValue = 0;
            if (textFadeValue > 255) textFadeValue = 255;
            if (imageFadeValue < 0) imageFadeValue = 0;
            if (imageFadeValue > 255) imageFadeValue = 255;
        }



        #endregion

    }

}
