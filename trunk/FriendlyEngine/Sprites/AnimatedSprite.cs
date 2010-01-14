using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class AnimatedSprite
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        bool visible = true;
        Texture2D texture;

        public Vector2 Position = Vector2.Zero;
        public Vector2 OriginOffset = Vector2.Zero;

        float rotation = 0f;
        SpriteEffects se = SpriteEffects.None;
        float radius = 12f;
        float speed = 3f;
        float scale = 1f;
        int maxAnims = -1;
        bool hideAtMaxAnim = true;
        bool reachedMaxAnim = false;
        bool trackParent = false;

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

        public bool HideWhenMaxAnim
        {
            get { return hideAtMaxAnim; }
            set { hideAtMaxAnim = value; }
        }

        public bool HasReachedMaxAnimations
        {
            get { return reachedMaxAnim; }
            set { reachedMaxAnim = value; }
        }

        public bool TrackParent
        {
            get { return trackParent; }
            set { trackParent = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float CollisionRadius
        {
            get { return radius; }
            set { radius = (float)Math.Max(value, 1f); }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = (float)Math.Max(value, 0.10f); }
        }

        public int MaxAnimations
        {
            get { return maxAnims; }
            set { maxAnims = value; }
        }

        public SpriteEffects SE
        {
            get { return se; }
            set { se = value; }
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

        public AnimatedSprite()
        {
            
        }

        public AnimatedSprite(Texture2D texture)
        {
            this.texture = texture;
            //this.OriginOffset = Vector2.Zero;
        }

        public static bool AreColliding(AnimatedSprite a, AnimatedSprite b)
        {
            if (a.visible == false || b.visible == false)
                return false;

            Vector2 d = b.Origin - a.Origin;
            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
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

        public static void ReFresh(AnimatedSprite b)
        {
            foreach (KeyValuePair<string, FrameAnimation> a in b.Animations)
            {
                a.Value.CountedFrames = 1;
                a.Value.CurrentFrame = 0;
            }
            b.Visible = true;
            b.IsAnimating = true;
        }

        public void TrackSprite(AnimatedSprite tracked, AnimatedSprite tracker, int xoffset, int yoffset)
        {
            if (tracker.Position.X != tracked.Position.X + xoffset)
                tracker.Position.X = tracked.Position.X + xoffset;
            if (tracker.Position.Y != tracked.Position.Y + yoffset)
                tracker.Position.Y = tracked.Position.Y + yoffset;
        }

        public void TrackSprite(Vector2 tracked, AnimatedSprite tracker, int xoffset, int yoffset)
        {
            if (tracker.Position.X != tracked.X + xoffset)
                tracker.Position.X = tracked.X + xoffset;
            if (tracker.Position.Y != tracked.Y + yoffset)
                tracker.Position.Y = tracked.Y + yoffset;
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

            if(maxAnims != -1)
                if (animation.CountedFrames >= animation.NumberOfFrames)
                    if (animation.CountedFrames / animation.NumberOfFrames >= maxAnims)
                    {
                        IsAnimating = false;
                        reachedMaxAnim = true;
                        if (hideAtMaxAnim == true)
                            visible = false;
                    }

            animation.Update(gameTime);
        }

        public virtual void Update(TimeSpan gameTime)
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

            if (maxAnims != -1)
                if (animation.CountedFrames >= animation.NumberOfFrames)
                    if (animation.CountedFrames / animation.NumberOfFrames >= maxAnims)
                    {
                        IsAnimating = false;
                        reachedMaxAnim = true;
                        if (hideAtMaxAnim == true)
                            visible = false;
                    }

            animation.Update(gameTime);
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
                    scale,
                    se,
                    0f);
        }
    }
}
