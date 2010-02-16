using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class LightSource
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private float range;

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private Texture2D lightTexture;

        public Texture2D LightTexture
        {
            get { return lightTexture; }
            set { lightTexture = value; }
        }
        public LightSource(Texture2D texture, Color color, float range, Vector2 position)
        {
            lightTexture = texture;
            this.color = color;
            this.range = range;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 center = new Vector2(lightTexture.Width / 2, lightTexture.Height / 2);
            float scale = range / ((float)lightTexture.Width / 2.0f);
            spriteBatch.Draw(lightTexture, position, null, color, 0, center, scale,SpriteEffects.None, 0.0f);
        }


    }
}
