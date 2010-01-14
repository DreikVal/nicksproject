using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class Projectile
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        bool visible = false;
        Texture2D texture;

        public Vector2 Position = Vector2.Zero;
        public Vector2 OriginOffset = Vector2.Zero;
        public Vector2 direction = Vector2.Zero;
        public float t = 0;
        public Vector2 pos2 = Vector2.Zero;
        public Vector2 pos3 = Vector2.Zero;
        float rotation = 0f;
        SpriteEffects se = SpriteEffects.None;

        int damage = 1;
        int maxAnims = 0;
        float radius = 8f;
        float speed = 1000;
        bool hideAtMaxAnim = false;
        bool reachedMaxAnim = false;
        public bool bezier = false;

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

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

        public int MaxAnimations
        {
            get { return maxAnims; }
            set { maxAnims = value; }
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

        public Projectile(Texture2D texture)
        {
            this.texture = texture;
        }
        public Projectile()
        {
        }

        public static bool IsVectorColliding(Projectile a, AnimatedSprite b)
        {
            Vector2 d = b.Origin - a.Origin;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }

        public static bool IsRectColliding(Projectile a, AnimatedSprite b)
        {
            bool d = b.Bounds.Intersects(a.Bounds);

            return (d);
        }

        public static bool IsRectColliding(Projectile a, Rectangle b)
        {
            bool d = b.Intersects(a.Bounds);

            return (d);
        }

        public void ClampToArea(int width, int height)
        {

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X > width - CurrentAnimation.CurrentRect.Width)
            {
                this.CleanUp(this);
            }

            if (Position.Y > height - CurrentAnimation.CurrentRect.Height)
                Position.Y = height - CurrentAnimation.CurrentRect.Height;

        }

        public void FireRedLaser(Projectile p, Vector2 position)
        {
            p.Position = position + new Vector2(0,-3);
            p.direction = new Vector2(1, 0);
            p.MaxAnimations = -1;
            p.IsAnimating = true;
            p.Visible = true;
            p.Damage = 1;
        }

        public void CleanUp(Projectile p)
        {
            p.Visible = false;
            p.Rotation = 0f;
            p.direction = Vector2.Zero;
            p.pos2 = Vector2.Zero;
            p.pos3 = Vector2.Zero;
            p.t = 0;
            p.bezier = false;
            p.Position = Vector2.Zero;
            p.IsAnimating = false;
           /* foreach (KeyValuePair<string, FrameAnimation> a in p.Animations)
            {
                a.Value.CountedFrames = 1;
                a.Value.CurrentFrame = 0;
            }*/
        }

        public virtual void Update(GameTime gameTime)
        {
            if (bezier == true)
            {
                Position = Engine.QuadBezierCurve(Position, pos2, pos3, t);
                t += 0.0166f;
            }
            else
            {
                if (direction != Vector2.Zero)
                    if (SE == SpriteEffects.FlipHorizontally)
                        Position -= Vector2.Multiply(direction, (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
                    else
                        Position += Vector2.Multiply(direction, (float)gameTime.ElapsedGameTime.TotalSeconds * speed);
            }
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
                    1f, 
                    se, 
                    0);
        }
    }
}
