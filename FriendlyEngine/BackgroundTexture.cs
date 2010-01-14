using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class BackgroundTexture
    {
        List<Texture2D> backgroundTextures = new List<Texture2D>();

        bool animating = true;
        bool visible = true;

        public Vector2 Position = Vector2.Zero;

        Color color = Color.White;
        Color invertColor = Color.White;
        float speed = 3f;
        float scale = 1f;
        int frameSkip = 5;
        int alphaCount = 255;

        int currentTex = 0;
        int nextTex = 1;

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = (float)Math.Max(value, .1f);
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = (float)Math.Max(value, 0.10f); }
        }

        public BackgroundTexture(params Texture2D[] texture)
        {
            for (int i = 0; i < texture.Length; i++)
                backgroundTextures.Add(texture[i]);
        }

        public virtual void Update(GameTime gameTime, Vector2 posi)
        {
            if (!IsAnimating)
                return;
            if (alphaCount <= 0)
            {
                currentTex++;

                if (nextTex == 1)
                    currentTex = 0;

                if (currentTex + 1 > backgroundTextures.Count - 1)
                    nextTex = 0;
                else
                    nextTex = currentTex + 1;
            }
            if (frameSkip >= 60)
            {
                color = new Color(255, 255, 255, (byte)alphaCount);
                invertColor = new Color(255, 255, 255, (byte)(255 - alphaCount));
                if (alphaCount - alphaCount < 0)
                    invertColor = new Color(255, 255, 255, (byte)((255 - alphaCount) * -1));
                alphaCount--;
                frameSkip = 0;
            }
            frameSkip++;
            Position = posi;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!visible)
                return;

            spriteBatch.Draw(
                backgroundTextures[nextTex],
                new Rectangle((int)Position.X, (int)Position.Y, 800, 608),
                invertColor);

            spriteBatch.Draw(
                backgroundTextures[currentTex],
                new Rectangle((int)Position.X, (int)Position.Y, 800, 608),
                color);

        }
    }
}
