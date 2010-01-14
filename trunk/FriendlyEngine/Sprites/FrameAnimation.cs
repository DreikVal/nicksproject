using System;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public class FrameAnimation :ICloneable
    {
        Rectangle[] frames;
        int currentFrame = 0;
        int comparisonFrame = 0;
        int countedFrames = 1;

        float frameLength = 0.5f;
        float timer = 0f;

        public int FramesPerSecond
        {
            get
            {
                return (int)(1f / frameLength);
            }
            set
            {
                frameLength = (float)Math.Max(1f / (float)value, 0.01f);
            }
        }

        public Rectangle CurrentRect
        {
            get
            {
                return frames[currentFrame];
            }
            
        }

        public int NumberOfFrames
        {
            get { return frames.Length; }
        }

        public int CountedFrames
        {
            get { return countedFrames; }
            set { countedFrames = value; }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
        }

        public FrameAnimation(
            int numberOfFrames,
            int frameWidth,
            int frameHeight,
            int xOffset,
            int yOffset)
        {
            frames = new Rectangle[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = frameWidth;
                rect.Height = frameHeight;
                rect.X = xOffset + (i * frameWidth);
                rect.Y = yOffset;

                frames[i] = rect;
            }
        }

        private FrameAnimation()
        {

        }

        public void Update(GameTime gameTime)
        {
            comparisonFrame = currentFrame;
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameLength)
            {
                timer = 0f;

                currentFrame = (currentFrame + 1) % frames.Length;
            }
            if (currentFrame != comparisonFrame)
                countedFrames++;
        }

        public void Update(TimeSpan gameTime)
        {
            comparisonFrame = currentFrame;
            timer += (float)gameTime.TotalSeconds;

            if (timer >= frameLength)
            {
                timer = 0f;

                currentFrame = (currentFrame + 1) % frames.Length;
            }
            if (currentFrame != comparisonFrame)
                countedFrames++;
        }

        #region ICloneable Members

        public object Clone()
        {
            FrameAnimation anim = new FrameAnimation();

            anim.frameLength = frameLength;
            anim.frames = frames;

            return anim;
        }

        #endregion
    }
}
