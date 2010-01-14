using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class Missile
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        bool visible = false;
        Texture2D texture;
        public AnimatedSprite targetHair = new AnimatedSprite();

        public List<AnimatedSprite> childAnimations = new List<AnimatedSprite>();
        public List<Point> childOffsets = new List<Point>();

        public List<AnimatedSprite> smokeList = new List<AnimatedSprite>();
        public int smokeListCount = 60;

        public Vector2 Position = Vector2.Zero;
        public Vector2 LaunchPosi = Vector2.Zero;
        public Vector2 OriginOffset = Vector2.Zero;
        public Vector2 direction = Vector2.Zero;
        public float t = 0;
        public Vector2 pos2 = Vector2.Zero;
        public Vector2 pos3 = Vector2.Zero;
        float rotation = 0f;
        SpriteEffects se = SpriteEffects.None;

        public bool PathVisible = false;
        public bool PlayerMissile = false;
        Color Tint;
        Vector2 missilePathPosition = Vector2.Zero;
        public BasicPrimitives triangle;
        public BasicPrimitives triangle1;
        public BasicPrimitives line;
        public BasicPrimitives line1;
        public Texture2D missileTriangle;

        int damage = 1;
        int maxAnims = 0;
        int smokeCount = 0;
        public int positionInTabDar = 0;
        float radius = 8f;
        float speed = 1250;
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

        public Vector2 mpPosi
        {
            get { return missilePathPosition; }
            set { missilePathPosition = value; }
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

        public Missile(Texture2D texture)
        {
            this.texture = texture;
        }
        public Missile()
        {
        }

        public static bool IsVectorColliding(Missile a, AnimatedSprite b)
        {
            Vector2 d = b.Origin - a.Origin;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }

        public static bool IsRectColliding(Missile a, AnimatedSprite b)
        {
            bool d = b.Bounds.Intersects(a.Bounds);

            return (d);
        }

        public static bool IsRectColliding(Missile a, Rectangle b)
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
                this.CleanUp();
            }

            if (Position.Y > height - CurrentAnimation.CurrentRect.Height)
                Position.Y = height - CurrentAnimation.CurrentRect.Height;

        }

        public void FireMissile(Vector2 position, bool path)
        {
            Position = position + new Vector2(0 - 3);
            LaunchPosi = position;
            direction = new Vector2(1, 0);
            MaxAnimations = -1;
            IsAnimating = true;
            Visible = true;
            Damage = 5;
            speed = 1000;
            if(path)
                PathVisible = true;
        }

        public void FireMissileBezier(Missile p, Vector2 position)
        {
            p.Position = position + new Vector2(0 - 3);
            p.pos2 = position + new Vector2(-30, -40);
            p.pos3 = position + new Vector2(500, 40);
            p.MaxAnimations = -1;
            p.IsAnimating = true;
            p.Visible = true;
            p.Damage = 2;
        }

        public void CleanUp()
        {
            Visible = false;
            PathVisible = false;
            Rotation = 0f;
            direction = Vector2.Zero;
            pos2 = Vector2.Zero;
            pos3 = Vector2.Zero;
            t = 0;
            bezier = false;
            Position = Vector2.Zero;
            IsAnimating = false;
            foreach (KeyValuePair<string, FrameAnimation> a in Animations)
            {
                a.Value.CountedFrames = 1;
                a.Value.CurrentFrame = 0;
            }
        }

        public void InitPath(GraphicsDevice graphics)
        {

            //triangle = new BasicPrimitives(graphics);
            //triangle.Colour = Color.Red;
            //triangle1 = new BasicPrimitives(graphics);
            //triangle1.Colour = Color.Red;
            line = new BasicPrimitives(graphics);
            line.Colour = Color.Red;
            Tint = Color.White;
            //line1 = new BasicPrimitives(graphics);
            //line1.Colour = Color.Red;

            //triangle.CreateLine(new Vector2(0, 0), new Vector2(300, 40));
            //triangle1.CreateLine(new Vector2(0, 40), new Vector2(300, 0));
            //line1.CreateLine(Vector2.Zero, new Vector2(0, 81));

            line.CreateLine(Vector2.Zero, Vector2.Zero);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.visible == true)
            {
                Random rand = new Random();
                childAnimations[0].Visible = true;

                if (smokeCount >= smokeListCount - 1)
                    smokeCount = 0;

                AnimatedSprite.ReFresh(smokeList[smokeCount]);
                smokeList[smokeCount].MaxAnimations = 1;
                smokeList[smokeCount].Scale = (float)rand.Next(7, 10) / 10;
                smokeList[smokeCount].Rotation = MathHelper.ToRadians(rand.Next(0, 360));
                smokeList[smokeCount].Position = Position - new Vector2(0, -2f);

                smokeCount++;

                AnimatedSprite.ReFresh(smokeList[smokeCount]);
                smokeList[smokeCount].MaxAnimations = 1;
                smokeList[smokeCount].Scale = (float)rand.Next(7, 10) / 10;
                smokeList[smokeCount].Rotation = MathHelper.ToRadians(rand.Next(180, 360));
                smokeList[smokeCount].Position = Position - new Vector2(8, -2f);

                smokeCount++;

            }

            for (int i = 0; i < smokeList.Count; i++)
                smokeList[i].Update(gameTime);

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

            if (PathVisible == true && PlayerMissile == false)
            {
                //triangle.Position = new Vector2(mpPosi.X, mpPosi.Y - 38);
                //triangle1.Position = new Vector2(mpPosi.X, mpPosi.Y + 2);
                line.Position = new Vector2(mpPosi.X + 280, mpPosi.Y + 2);
                //line1.Position = new Vector2(mpPosi.X, mpPosi.Y - 38);
            }

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


            for (int i = 0; i < childAnimations.Count; i++)
            {
                childAnimations[i].Update(gameTime);
                if (childAnimations[i].TrackParent == true)
                    childAnimations[i].TrackSprite(this.Position, childAnimations[i], childOffsets[i].X, childOffsets[i].Y);
            }

        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            if (PathVisible == true && PlayerMissile == false)
            {
                line.RenderLinePrimitive(spriteBatch);
                spriteBatch.Draw(missileTriangle, new Rectangle((int)mpPosi.X, (int)mpPosi.Y - (missileTriangle.Height / 2 - 2), 301, 81), Tint);
                targetHair.Draw(spriteBatch);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite a in smokeList)
                a.Draw(spriteBatch);

            if (!visible)
                return;
            FrameAnimation animation = CurrentAnimation;

            for (int i = 0; i < childAnimations.Count; i++)
                childAnimations[i].Draw(spriteBatch);

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
