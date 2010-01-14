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
    public class OptionState : GameState
    {

        SpriteBatch spriteBatch;
        SpriteFont font10;
        SpriteFont font16;
        SpriteFont font32;
        SpriteFont font48;
        SpriteFont font79;
        Text[] MenuText;

        Texture2D Crosshair;
        Texture2D logo;

        KeyboardState kState;
        bool pressed;
        bool click;
        bool kgo = false;

        public OptionState(Game game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font10 = Content.Load<SpriteFont>("Fonts/Calibri10");
            font16 = Content.Load<SpriteFont>("Fonts/Calibri16");
            font32 = Content.Load<SpriteFont>("Fonts/Calibri32");
            font48 = Content.Load<SpriteFont>("Fonts/Ara48");
            font79 = Content.Load<SpriteFont>("Fonts/Ara79");

            Crosshair = Content.Load<Texture2D>("Sprites/targethair");
            logo = Content.Load<Texture2D>("Sprites/TitleScreen/title");


            #region Input Constructors
            pressed = false;
            click = false;
            MenuText = new Text[2] {  
                new Text(new Vector2(100, 304), "options", font48, 48, 1),
                new Text(new Vector2(100, 354), "back", font48, 48, 1)};
            #endregion
        }

        public override void  UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();

            UpdateMouse(gameTime);
            moveOptions(kgo);
            moveBack(kgo);
            UpdateSelected();
            foreach (Text txt in MenuText)
            {
                if (txt.waitForMePlx == true)
                    break;
                if (kgo)
                { 
                    kgo = false; 
                    Manager.CurrentState = PGGameState.Title;
                    InputHelper.SetMouseXY(InputHelper.MouseX(), InputHelper.MouseY());
                }
            }

        }

        private void UpdateMouse(GameTime gameTime)
        {
            bool activeSelect = false;

            for (int i = 0; i < MenuText.GetLength(0); i++)
            {
                if (MenuText[i].inBound(InputHelper.MouseXY()) && MenuText[i].Selectable == 1) { Selected = i; activeSelect = true; }
            }

            if (!activeSelect) Selected = -1;

            if (InputHelper.IsLeftButton()) pressed = true;

            if (InputHelper.IsLeftButtonReleased() && pressed)
            {
                click = true;
                pressed = false;
            }

            if (click)
            {
                if (Selected == 0) Manager.CurrentState = PGGameState.Options;
                if (Selected == 1 && gameTime.TotalRealTime.TotalSeconds > 2) kgo = true;
                click = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(logo, new Rectangle(0, 0, 800, 600), Color.White);
            foreach (Text txt in MenuText) { txt.Draw(spriteBatch); };

            spriteBatch.Draw(Crosshair, new Rectangle(InputHelper.MouseX() - 10, InputHelper.MouseY() - 5, 20, 20), Color.White);
            spriteBatch.End();
        }
        private void UpdateSelected()
        {
            for (int i = 0; i < MenuText.GetLength(0); i++)
            {
                if (Selected == i) MenuText[i].Selected = true;
                else MenuText[i].Selected = false;
            }

        }

        private void moveOptions(bool kgo)
        {
            if (!kgo)
            {
                for (int i = 0; i < MenuText.GetLength(0); i++)
                {
                    if (MenuText[i].text == "options")
                    {
                        if (MenuText[i].Position.X >= 61)
                        {
                            MenuText[i].Position.X -= 2;
                            MenuText[i].Box.X = (int)MenuText[i].Position.X;
                        }
                        if (MenuText[i].Position.Y >= 122)
                        {
                            MenuText[i].Position.Y -= 3;
                            MenuText[i].Box.Y = (int)MenuText[i].Position.Y;
                        }
                    }
                }
            }
            if (kgo)
            {
                for (int i = 0; i < MenuText.GetLength(0); i++)
                {
                    if (MenuText[i].text == "options")
                    {
                        if (MenuText[i].Position.X <= 99)
                        {
                            MenuText[i].waitForMePlx = true;
                            MenuText[i].Position.X += 2;
                            MenuText[i].Box.X = (int)MenuText[i].Position.X;
                        }
                        else
                            MenuText[i].waitForMePlx = false;

                        if (MenuText[i].Position.Y <= 303)
                        {
                            MenuText[i].waitForMePlx = true;
                            MenuText[i].Position.Y += 3;
                            MenuText[i].Box.Y = (int)MenuText[i].Position.Y;
                        }
                        else
                            MenuText[i].waitForMePlx = false;
                    }
                }
            }
        }
        private void moveBack(bool kgo)
        {
            if (!kgo)
            {
                for (int i = 0; i < MenuText.GetLength(0); i++)
                {
                    if (MenuText[i].text == "back")
                    {
                        if (MenuText[i].Position.X >= 61)
                        {
                            MenuText[i].Position.X -= 2;
                            MenuText[i].Box.X = (int)MenuText[i].Position.X;
                        }
                        if (MenuText[i].Position.Y <= 422)
                        {
                            MenuText[i].Position.Y += 2;
                            MenuText[i].Box.Y = (int)MenuText[i].Position.Y;
                        }
                    }
                }
            }
            if (kgo)
            {
                for (int i = 0; i < MenuText.GetLength(0); i++)
                {
                    if (MenuText[i].text == "back")
                    {
                        if (MenuText[i].Position.X <= 100)
                        {
                            MenuText[i].waitForMePlx = true;
                            MenuText[i].Position.X += 2;
                            MenuText[i].Box.X = (int)MenuText[i].Position.X;
                        }
                        else
                            MenuText[i].waitForMePlx = false;

                        if (MenuText[i].Position.Y >= 354)
                        {
                            MenuText[i].waitForMePlx = true;
                            MenuText[i].Position.Y -= 2;
                            MenuText[i].Box.Y = (int)MenuText[i].Position.Y;
                        }
                        else
                            MenuText[i].waitForMePlx = false;
                    }
                }
            }
        }


        //    #region Text Fader Methods


        //    private Color FadeColor(Color baseColor, float textFadeValue)
        //     {
        //          Color tempColor;
        //          tempColor = new Color(baseColor.R, baseColor.G, baseColor.B, (byte)textFadeValue);
        //          return tempColor;
        //       }
        //
        //     private void updateFadeAsset(GameTime gameTime, double start, bool image, bool permanent, int speed)
        //      {
        //          updateAssetFade(gameTime, start, start + 3.0, true, image, speed);
        //          if (!permanent) updateAssetFade(gameTime, start + 4.0, start + 6.0, false, image, speed);
        //     }
        //
        //    private void drawImage(SpriteBatch spriteBatch, GameTime gameTime)
        //     {
        //         if (gameTime.TotalRealTime.TotalSeconds > 3.0) spriteBatch.Draw(logo, new Rectangle(0, 0, 1024, 768), FadeColor(Color.White, imageFadeValue));
        //     }
        //

        //private bool timeBetween(GameTime gameTime, double st, double end)
        // {
        //       return (gameTime.TotalRealTime.TotalSeconds > startTime && gameTime.TotalRealTime.TotalSeconds < end);
        //     }


        //    private void updateAssetFade(GameTime gameTime, double start, double end, bool increment, bool image, float speed)
        //  {
        //
        //  if (timeBetween(gameTime start, end) && increment))
        // {
        //    if (!image) textFadeValue += 3 * speed;
        //   else imageFadeValue += 3 * speed;
        // }

        //if (timeBetween(gameTime, start, end) && !increment)
        //{
        //   if (!image) textFadeValue -= 3 * speed;
        //   else imageFadeValue -= 3 * speed;
        // }

        // if (textFadeValue < 0) textFadeValue = 0;
        //if (textFadeValue > 255) textFadeValue = 255;
        //if (imageFadeValue < 0) imageFadeValue = 0;
        //if (imageFadeValue > 255) imageFadeValue = 255;
        //}



        // #endregion

    }

}
