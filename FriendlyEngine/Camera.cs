using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FriendlyEngine
{
    public class Camera
    {
        public Vector2 ScrollRate = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;

        public Matrix TransformMatrix
        {
            get 
            { 
                return Matrix.CreateTranslation(new Vector3(-Position, 0)); 
            }
        }

        public void LockToTarget(AnimatedSprite sprite, int screenWidth, int screenHeight)
        {

            Position.X =
              sprite.Position.X +
               (sprite.CurrentAnimation.CurrentRect.Width / 2) -
               (screenWidth / 2);

            Position.Y =
                sprite.Position.Y +
                (sprite.CurrentAnimation.CurrentRect.Height / 2) -
                (screenHeight / 2);

        }

        public void LockTargetToCamera(AnimatedSprite sprite)
        {
            if (sprite.Position.X < Position.X + sprite.CurrentAnimation.CurrentRect.Width)
            {
                sprite.Position.X =
                   Position.X +
                   (sprite.CurrentAnimation.CurrentRect.Width);
            }
        }

        public void ClampToArea(int width, int height)
        {
            if (Position.X > width)
                Position.X = width;

            if (Position.Y > height)
                Position.Y = height;

            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y < 0)
                Position.Y = 0;

        }

        public void Update()
        {
            Position += ScrollRate;
        }
    }
}
