using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class HealthBar
    {
        Texture2D texture;
        Rectangle lengthRect = new Rectangle(0,0,32,10);
        int length = 100;
        int targetLength = 100;
        int immediateTarget = 100;
        bool animating = true;

        Vector2 Position = Vector2.Zero;


        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public Vector2 posi
        {
            get { return Position; }
            set { Position = value + new Vector2(0, -16); }
        }

        public int Length
        {
            get { return length; }
            set { length = value;}
        }
        public int Target
        {
            get { return targetLength; }
            set { targetLength = value; }
        }

        public int ImmediateTarget
        {
            get { return immediateTarget; }
            set { immediateTarget = value; }
        }

        public HealthBar(Texture2D tex)
        {
            texture = tex;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsAnimating)
                return;

            if (targetLength > length)
            {
                length++;
                lengthRect.Width = (length * 32) / 100;
            }

            else if (immediateTarget > length)
            {
                length = targetLength = immediateTarget;
                lengthRect.Width = (length * 32) / 100;
            }

            else if (targetLength < length)
            {
                length--;
                lengthRect.Width = (length * 32) / 100;
            }

            else if (immediateTarget < length)
            {
                length = targetLength = immediateTarget;
                lengthRect.Width = (length * 32) / 100;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(
                    texture,
                    Position,
                    lengthRect,
                    Color.White);
        }
    }
}
