using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public class Text
    {
        public Vector2 Position = new Vector2(0, 0);
        public Rectangle Box = new Rectangle(0, 0, 0, 0);
        private SpriteFont mSpriteFont;
        public int Selectable;
        public String text;

        public Color TextColor;
        public Color HighlightColor;
        public bool Selected;
        public bool waitForMePlx;

        public Text(Vector2 position, String text, SpriteFont spriteFont, int Size, int Selectable)
        {
            Box = new Rectangle((int)position.X, (int)position.Y, (int)(Size * text.Length / 1.5), (int)(Size * 1.5));
            Position = position;
            mSpriteFont = spriteFont;
            this.text = text;
            this.Selectable = Selectable;
        }

        public Color Select()
        {
            TextColor = new Color(255, 255, 255);
            HighlightColor = new Color(204, 204, 0);
            if (Selected) return HighlightColor;
            else return TextColor;
        }

        public bool inBound(Vector2 xy)
        {
            return (xy.X > Box.Left && xy.X < Box.Right && xy.Y > Box.Top && xy.Y < Box.Bottom);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(mSpriteFont, text, Position, Select());
        }
    }
}
