using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FriendlyEngine
{
    public class NpcPlane : AnimatedSprite
    {
        Vector2 target = Vector2.Zero;
        Rectangle targetRect;
        Vector2 motion = Vector2.Zero;

        public Projectile[] FireArray = new Projectile[60];
        public Missile Missile = new Missile();

        public List<AnimatedSprite> childAnimations = new List<AnimatedSprite>();
        public List<Point> childOffsets = new List<Point>();
        public Rectangle homingSquare = new Rectangle(0, 0, 300, 250);

        bool isHit = false;
        bool isMoving = true;
        bool isAlive = true;
        public bool reloaded = true;
        public bool projReloaded = true;
        public bool incrementScore = false;
        int health = 3;
        int maxHealth = 500;
        int reloadCount = 0;
        int reloadMax = 30;
        public int positionInTabDar = 0;


        public NpcPlane()
            : base()
        {
        }

        public NpcPlane(Texture2D texture)
            : base(texture)
        {
        }

        public static NpcPlane FromFile(ContentManager content, string filename)
        {
            NpcPlane npc = new NpcPlane();
            AnimatedSprite child;
            List<AnimatedSprite> childAnims = new List<AnimatedSprite>();
            List<string> textureNames = new List<string>();
            List<string> animationKeys = new List<string>();
            string spriteEffects = null;
            Vector2 originOffset = Vector2.Zero;
            Vector2 Position = Vector2.Zero;
            Point childOffset = Point.Zero;
            Dictionary<string, List<int>> animationDict = new Dictionary<string, List<int>>();
            Dictionary<string, string> boolsDict = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingAnimations = false;
                bool readingBools = false;
                bool readingEndSprite = false;
                bool readingMaxCount = false;
                bool readingExtras = false;
                int spriteCount = 0;
                int maxCount = 1;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;
                    #region Bools
                    if (line.Contains("[Texture]"))
                    {
                        readingTextures = true;
                        readingAnimations = false;
                        readingEndSprite = false;
                        readingBools = false;
                        readingMaxCount = false;
                        readingExtras = false;
                    }
                    else if (line.Contains("[Animations]"))
                    {
                        readingAnimations = true;
                        readingTextures = false;
                        readingEndSprite = false;
                        readingBools = false;
                        readingMaxCount = false;
                        readingExtras = false;
                    }
                    else if (line.Contains("[Bools]"))
                    {
                        readingBools = true;
                        readingAnimations = false;
                        readingTextures = false;
                        readingEndSprite = false;
                        readingMaxCount = false;
                        readingExtras = false;
                    }
                    else if (line.Contains("[EndSprite]"))
                    {
                        readingEndSprite = true;
                        readingAnimations = false;
                        readingTextures = false;
                        readingBools = false;
                        readingMaxCount = false;
                        readingExtras = false;
                    }
                    else if (line.Contains("[MaxCount]"))
                    {
                        readingEndSprite = false;
                        readingAnimations = false;
                        readingTextures = false;
                        readingBools = false;
                        readingMaxCount = true;
                        readingExtras = false;
                    }
                    else if (line.Contains("[Extras]"))
                    {
                        readingEndSprite = false;
                        readingAnimations = false;
                        readingTextures = false;
                        readingBools = false;
                        readingMaxCount = false;
                        readingExtras = true;
                    }
                    #endregion

                    else if (readingMaxCount)
                    {
                        maxCount = int.Parse(line);
                    }
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                    else if (readingBools)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        boolsDict.Add(key, value);
                    }
                    else if (readingExtras)
                    {
                        if (line.Contains("FlipHorizontally"))
                            spriteEffects = line;
                        else
                        {
                            string[] pair = line.Split('=');
                            string key = pair[0].Trim();
                            string value = pair[1].Trim();

                            if (key.Contains("OriginOffset"))
                            {
                                string[] pair1 = value.Split(',');
                                string key1 = pair1[0].Trim();
                                string value1 = pair1[1].Trim();
                                originOffset = new Vector2(float.Parse(key1), float.Parse(value1));
                            }
                            if (key.Contains("Position"))
                            {
                                string[] pair1 = value.Split(',');
                                string key1 = pair1[0].Trim();
                                string value1 = pair1[1].Trim();
                                Position = new Vector2(float.Parse(key1), float.Parse(value1));
                            }
                            if (key.Contains("childOffset"))
                            {
                                string[] pair1 = value.Split(',');
                                string key1 = pair1[0].Trim();
                                string value1 = pair1[1].Trim();
                                childOffset = new Point(int.Parse(key1), int.Parse(value1));
                            }
                        }
                    }
                    else if (readingAnimations)
                    {
                        List<int> row = new List<int>();

                        string[] pair = line.Split(':');
                        string name = pair[0].Trim();

                        string[] cells = pair[1].Split(',');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        animationKeys.Add(name);
                        animationDict.Add(name, row);
                    }
                    if (readingEndSprite)
                    {
                        if (spriteCount <= 0)
                        {
                            FrameAnimation frame;
                            npc = new NpcPlane(content.Load<Texture2D>(textureNames[spriteCount]));
                            for (int i = 0; i < animationDict.Count; i++)
                            {
                                frame = new FrameAnimation(animationDict[animationKeys[i]][0],
                                    animationDict[animationKeys[i]][1], animationDict[animationKeys[i]][2],
                                    animationDict[animationKeys[i]][3], animationDict[animationKeys[i]][4]);
                                frame.FramesPerSecond = animationDict[animationKeys[i]][5];
                                npc.Animations.Add(animationKeys[i], frame);
                            }
                            npc.CurrentAnimationName = "Right";
                            if (Position != Vector2.Zero)
                                npc.Position = Position;
                            if (spriteEffects == "FlipHorizontally")
                                npc.SE = SpriteEffects.FlipHorizontally;
                            if (originOffset != Vector2.Zero)
                                npc.OriginOffset = originOffset;
                        }
                        if (spriteCount >= 1)
                        {
                            FrameAnimation frame;
                            child = new AnimatedSprite(content.Load<Texture2D>(textureNames[spriteCount]));
                            for (int i = 0; i < animationDict.Count; i++)
                            {
                                frame = new FrameAnimation(animationDict[animationKeys[i]][0],
                                    animationDict[animationKeys[i]][1], animationDict[animationKeys[i]][2],
                                    animationDict[animationKeys[i]][3], animationDict[animationKeys[i]][4]);
                                frame.FramesPerSecond = animationDict[animationKeys[i]][5];
                                child.Animations.Add(animationKeys[i], frame);
                            }
                            child.CurrentAnimationName = animationKeys[0];

                            for (int i = 0; i < boolsDict.Count; i++)
                            {
                                if (boolsDict["TrackParent"].Contains("true"))
                                    child.TrackParent = true;
                                else
                                    child.TrackParent = false;
                            }
                            if (spriteEffects == "FlipHorizontally")
                                child.SE = SpriteEffects.FlipHorizontally;

                            npc.childAnimations.Add(child);
                            if (childOffset != Point.Zero)
                                npc.childOffsets.Add(childOffset);

                        }
                        animationDict.Clear();
                        animationKeys.Clear();
                        boolsDict.Clear();
                        spriteEffects = null;
                        spriteCount++;
                    }
                    if (spriteCount == maxCount)
                    {
                        return npc;
                    }
                }
            }
            return null;
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth;}
            set { maxHealth = Math.Max(value, 1);}
        }

        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Rectangle TargetRect
        {
            get { return targetRect; }
            set { targetRect = value; }
        }

        public bool isHitWait
        {
            get { return isHit; }
            set { isHit = value; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public AnimatedSprite ReturnChild(int index)
        {
            if (childAnimations[index] != null)
                return childAnimations[index];
            else
                return null;
        }

        public static bool IsVectorColliding(Vector2 t, Vector2 p)
        {
            Vector2 d = t - p;

            return (d.Length() < 2 + 2);
        }

        public void FireWeapon()
        {
            if (projReloaded == true && isAlive == true)
            {
                for (int i = 0; i < FireArray.Length; i++)
                    if (FireArray[i].Visible == false)
                    {
                        childAnimations[2].Visible = true;
                        childAnimations[2].MaxAnimations = 1;
                        childAnimations[2].IsAnimating = true;
                        foreach (KeyValuePair<string, FrameAnimation> a in childAnimations[2].Animations)
                        {
                            a.Value.CountedFrames = 1;
                            a.Value.CurrentFrame = 0;
                        }
                        FireArray[i].FireRedLaser(FireArray[i], Position + new Vector2(0, Bounds.Height / 2));
                        FireArray[i].SE = SpriteEffects.FlipHorizontally;
                        break;
                    }
            }
        }

        public void FireMissile()
        {
            if (reloaded == true && isAlive == true)
            {
                if (Missile.Visible == false)
                {
                    childAnimations[2].Visible = true;
                    childAnimations[2].MaxAnimations = 1;
                    childAnimations[2].IsAnimating = true;
                    foreach (KeyValuePair<string, FrameAnimation> a in childAnimations[2].Animations)
                    {
                        a.Value.CountedFrames = 1;
                        a.Value.CurrentFrame = 0;
                    }
                    Missile.FireMissile(Position + new Vector2(0, Bounds.Height / 2), true);
                    Missile.SE = SpriteEffects.FlipHorizontally;
                    Missile.mpPosi = new Vector2(Missile.mpPosi.X, Missile.Position.Y);
                    Missile.PathVisible = true;
                }
            }
            reloaded = false;
        }

        public bool ProjectileCollision(Projectile a, NpcPlane npc)
        {
            if (npc.IsAlive == false)
            {
                return false;
            }

            npc.Health = npc.Health - a.Damage;

            AnimatedSprite.ReFresh(npc.childAnimations[1]);
            npc.childAnimations[1].CurrentAnimationName = "Burst";
            npc.childAnimations[1].MaxAnimations = 1;
            npc.childAnimations[1].HideWhenMaxAnim = true;
            npc.childAnimations[1].Position = npc.Position;

            if (npc.Health <= 0)
            {
                npc.IsAlive = false;
                npc.incrementScore = true;
                AnimatedSprite.ReFresh(npc);
                npc.CurrentAnimationName = "Death";
                npc.HideWhenMaxAnim = false;
                npc.MaxAnimations = 1;
                a.CleanUp(a);
                return true;
            }

            a.CleanUp(a);
            return false;
        }
        public bool ProjectileCollision(Missile a, NpcPlane npc)
        {
            if (npc.IsAlive == false)
            {
                return false;
            }

            npc.Health = npc.Health - a.Damage;

            AnimatedSprite.ReFresh(npc.childAnimations[1]);
            npc.childAnimations[1].CurrentAnimationName = "Burst";
            npc.childAnimations[1].MaxAnimations = 1;
            npc.childAnimations[1].HideWhenMaxAnim = true;
            npc.childAnimations[1].Position = npc.Position;

            if (npc.Health <= 0)
            {
                npc.IsAlive = false;
                npc.incrementScore = true;
                AnimatedSprite.ReFresh(npc);
                npc.CurrentAnimationName = "Death";
                npc.HideWhenMaxAnim = false;
                npc.MaxAnimations = 1;
                a.CleanUp();
                return true;
            }

            a.CleanUp();
            return false;
        }

        public void ReSpawnPlane(NpcPlane npc)
        {
            Random Rand = new Random();
            npc.MaxAnimations = -1;
            npc.Health = Rand.Next(1,2);
            npc.TargetRect = new Rectangle((int)npc.Position.X + Rand.Next(600, 750), Rand.Next(589), 2, 2);
            npc.Position = new Vector2(npc.Position.X + Rand.Next(2400,2600), Rand.Next(589));
            npc.CurrentAnimationName = "Right";
            npc.IsAnimating = true;
            npc.Visible = true;
            npc.childAnimations[0].Visible = true;
            npc.HasReachedMaxAnimations = false;
            npc.IsMoving = true;
            npc.IsAlive = true;
            npc.reloaded = true;
        }

        public void UpdateAnimations(GameTime gameTime, NpcPlane npc)
        {
            if (npc.CurrentAnimationName == "Death")
            {
                AnimatedSprite g = npc.ReturnChild(0);
                g.Visible = false;
            }

            if (npc.CurrentAnimationName == "Explode" && npc.HasReachedMaxAnimations == true)
            {
                npc.ReSpawnPlane(npc);
            }

            if (npc.CurrentAnimationName == "Death" && npc.HasReachedMaxAnimations == true)
            {
                npc.CurrentAnimationName = "Explode";
                npc.IsMoving = false;
                foreach (KeyValuePair<string, FrameAnimation> a in npc.Animations)
                {
                    a.Value.CountedFrames = 1;
                    a.Value.CurrentFrame = 0;
                }
                npc.MaxAnimations = 1;
                npc.IsAnimating = true;
                npc.HideWhenMaxAnim = true;
                npc.HasReachedMaxAnimations = false;
            }
            if (npc.CurrentAnimationName == "Explode")
            {
                npc.TargetRect = new Rectangle((int)npc.Position.X, (int)npc.Position.Y, 2, 2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Random Rand = new Random();

            if (isHit == true)
            {
                isHit = false;
            }

            if (this.isMoving == true)
            {
                if (!targetRect.Intersects(Bounds))
                {
                    Vector2 d = Position - new Vector2(targetRect.X, targetRect.Y);

                    if (d.X > 1)
                        motion.X--;
                    else if (d.X < -1)
                        motion.X++;
                    else
                        motion.X = 0;

                    if (d.Y > 1)
                        motion.Y--;
                    else if (d.Y < -1)
                        motion.Y++;
                    else
                        motion.Y = 0;
                }
                else
                {
                    TargetRect = new Rectangle((int)Position.X - Rand.Next(100, 300), Rand.Next(589), 2, 2);
                }
            }
            else
                motion = Vector2.Zero;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                Position += motion * Speed;

                IsAnimating = true;
            }
            if (isAlive == true && motion != Vector2.Zero)
            {
                float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

                if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
                    CurrentAnimationName = "Left";
                else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
                    CurrentAnimationName = "Down";
                else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
                    CurrentAnimationName = "Up";
                else
                    CurrentAnimationName = "Right";
            }
            Missile.Update(gameTime);
            base.Update(gameTime);

            homingSquare.X = (int)Position.X - (homingSquare.Width);
            homingSquare.Y = (int)Position.Y - (homingSquare.Height / 2);

            for (int i = 0; i < childAnimations.Count; i++)
            {
                childAnimations[i].Update(gameTime);
                if (childAnimations[i].TrackParent == true)
                    childAnimations[i].TrackSprite(this, childAnimations[i], childOffsets[i].X, childOffsets[i].Y);
            }

            if (reloadCount == 10)
            {
                projReloaded = false;
            }
            if (reloadCount >= reloadMax)
            {
                projReloaded = true;
                reloadCount = 0;
            }
            else
                reloadCount++;
        }

    }
}
