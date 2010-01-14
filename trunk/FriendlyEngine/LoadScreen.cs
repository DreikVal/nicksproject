using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public class LoadScreen
    {
        Texture2D image1;
        Vector2 Position = Vector2.Zero;
        SpriteFont font32;
        Text Text;
        int count = 0;
        int MaxCount = 180;
        public bool loaded = false;
        bool Visible = true;

        public LoadScreen(int Max, Texture2D tex, SpriteFont font)
        {
            MaxCount = Max;
            loaded = false;
            image1 = tex;
            font32 = font;
            Text = new Text(new Vector2(512, 384), "Loading...", font32, (int)(font32.MeasureString("T").Y), 0);
        }

        public void Update()
        {
            if (count >= MaxCount)
            {
                loaded = true;
            }
            else
            {
                count++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            spriteBatch.Draw(image1, Position, Color.White);
            Text.Draw(spriteBatch);
        }
    }
}
