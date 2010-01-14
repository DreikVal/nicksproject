using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class AttackSprite
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        bool visible = true;
        bool swingWait = false;
        Texture2D texture;

        public Vector2 Position = Vector2.Zero;
        public Vector2 OriginOffset = Vector2.Zero;

        float radius = 12f;
        float rotation = 0f;
        SpriteEffects se = SpriteEffects.None;

        public Vector2 Origin
        {
            get { return Position + OriginOffset; }
        }

        public Vector2 Center
        {
            get
            {
                return Position +
                    new Vector2(
                    CurrentAnimation.CurrentRect.Width / 2,
                    CurrentAnimation.CurrentRect.Height / 2);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                Rectangle rect = CurrentAnimation.CurrentRect;
                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;
                return rect;
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool SwingWait
        {
            get { return swingWait; }
            set { swingWait = value; }
        }

        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public SpriteEffects SE
        {
            get { return se; }
            set { se = value; }
        }


        public float CollisionRadius
        {
            get { return radius; }
            set { radius = (float)Math.Max(value, 1f); }
        }

        public FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return Animations[currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
            }
        }

        public AttackSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public static bool IsVectorColliding(AttackSprite a, AnimatedSprite b)
        {
            Vector2 d = b.Origin - a.Origin;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }

        public static bool IsRectColliding(AttackSprite a, AnimatedSprite b)
        {
            bool d = b.Bounds.Intersects(a.Bounds);

            return (d);
        }

        public void ClampToArea(int width, int height)
        {

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X > width - CurrentAnimation.CurrentRect.Width)
                Position.X = width - CurrentAnimation.CurrentRect.Width;

            if (Position.Y > height - CurrentAnimation.CurrentRect.Height)
                Position.Y = height - CurrentAnimation.CurrentRect.Height;

        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsAnimating)
                return;

            FrameAnimation animation = CurrentAnimation;

            if (animation == null)
            {
                if (Animations.Count > 0)
                {
                    string[] keys = new string[Animations.Count];
                    Animations.Keys.CopyTo(keys, 0);

                    currentAnimation = keys[0];

                    animation = CurrentAnimation;
                }
                else
                    return;
            }             
            animation.Update(gameTime);
            if (CurrentAnimation.CurrentFrame == 5)
            {
                swingWait = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!visible)
                return;
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
                spriteBatch.Draw(
                    texture,
                    Position,
                    animation.CurrentRect,
                    Color.White, 
                    rotation, 
                    OriginOffset, 
                    1f, 
                    se, 
                    0);
        }
    }
}
