using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace FriendlyEngine
{
    public class PlayerPlane : AnimatedSprite
    {
        Vector2 target = Vector2.Zero;
        Rectangle targetRect;
        Vector2 motion = Vector2.Zero;

        public List<AnimatedSprite> childAnimations = new List<AnimatedSprite>();
        public List<Vector2> childOffsets = new List<Vector2>();
        public Rectangle homingSquare = new Rectangle(0, 0, 300, 250);

        public Projectile[] FireArray = new Projectile[200];
        public List<Missile> MissileList = new List<Missile>();

        public Texture2D Indicator;
        public Texture2D Indicator1;
        public Color indicatorColor;
        public Color indicatorColor1;

        public Color halfWhite;

        bool isHit = false;
        bool isMoving = true;
        bool isAlive = true;
        int health = 10000;
        int maxHealth = 10000;
        int missileCount = 0;
        bool firingTop = true;

        public PlayerPlane()
            : base()
        {
        }

        public PlayerPlane(Texture2D texture)
            : base(texture)
        {
        }

        public static PlayerPlane FromFile(ContentManager content, string filename)
        {
            PlayerPlane npc = new PlayerPlane();
            AnimatedSprite child;
            List<AnimatedSprite> childAnims = new List<AnimatedSprite>();
            List<string> textureNames = new List<string>();
            List<string> animationKeys = new List<string>();
            string spriteEffects = null;
            Vector2 originOffset = Vector2.Zero;
            Vector2 Position = Vector2.Zero;
            Vector2 childOffset = Vector2.Zero;
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
                                childOffset = new Vector2(int.Parse(key1), int.Parse(value1));
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
                            npc = new PlayerPlane(content.Load<Texture2D>(textureNames[spriteCount]));
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
                            if (childOffset != Vector2.Zero)
                                npc.childOffsets.Add(childOffset);
                        }
                        animationDict.Clear();
                        animationKeys.Clear();
                        boolsDict.Clear();
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
            for (int i = 0; i < FireArray.Length; i++)
                if (FireArray[i].Visible == false)
                {
                    childAnimations[2].MaxAnimations = 1;
                    AnimatedSprite.ReFresh(childAnimations[2]);
                    FireArray[i].FireRedLaser(FireArray[i], Position + new Vector2(Bounds.Width, Bounds.Height / 2));
                    break;
                }
            for (int i = 0; i < FireArray.Length; i++)
                if (FireArray[i].Visible == false)
                {
                    childAnimations[2].MaxAnimations = 1;
                    AnimatedSprite.ReFresh(childAnimations[2]);
                    FireArray[i].FireRedLaser(FireArray[i], Position + new Vector2(Bounds.Width, (Bounds.Height / 2) + 6));
                    break;
                }
        }

        public void FireMissile()
        {
            if (isAlive == true)
            {
                if (missileCount >= MissileList.Count - 1)
                    missileCount = 0;
                if(MissileList[missileCount].Visible == false)
                {
                    childAnimations[2].Visible = true;
                    childAnimations[2].MaxAnimations = 1;
                    childAnimations[2].IsAnimating = true;
                    AnimatedSprite.ReFresh(childAnimations[2]);
                    if(firingTop == true)
                    {
                        MissileList[missileCount].FireMissile(Position + new Vector2(Bounds.Width - 9, Bounds.Height / 4), true);
                        firingTop = false;
                    }
                    else if (!firingTop)
                    {
                        MissileList[missileCount].FireMissile(Position + new Vector2(Bounds.Width - 9, Bounds.Height / 4 * 3), true);
                        firingTop = true;
                    }
                    missileCount++;
                }
                if (MissileList[missileCount].Visible == false)
                {
                    childAnimations[2].Visible = true;
                    childAnimations[2].MaxAnimations = 1;
                    childAnimations[2].IsAnimating = true;
                    AnimatedSprite.ReFresh(childAnimations[2]);
                    if (firingTop == true)
                    {
                        MissileList[missileCount].FireMissile(Position + new Vector2(Bounds.Width - 9, Bounds.Height / 4), true);
                        firingTop = false;
                    }
                    else if (!firingTop)
                    {
                        MissileList[missileCount].FireMissile(Position + new Vector2(Bounds.Width - 9, Bounds.Height / 4 * 3), true);
                        firingTop = true;
                    }
                    missileCount++;
                }
            }
        }

        public bool ProjectileCollision(Projectile a)
        {
            if (IsAlive == false)
            {
                return false;
            }

            Health = Health - a.Damage;

            foreach (KeyValuePair<string, FrameAnimation> s in childAnimations[1].Animations)
            {
                s.Value.CountedFrames = 1;
                s.Value.CurrentFrame = 0;
            }
            childAnimations[1].CurrentAnimationName = "Burst";
            childAnimations[1].MaxAnimations = 1;
            childAnimations[1].HideWhenMaxAnim = true;
            childAnimations[1].Visible = true;
            childAnimations[1].IsAnimating = true;
            childAnimations[1].Position = Position;

            if (Health <= 0)
            {
                IsAlive = false;
                foreach (KeyValuePair<string, FrameAnimation> anim in Animations)
                {
                    anim.Value.CountedFrames = 1;
                    anim.Value.CurrentFrame = 0;
                }
                CurrentAnimationName = "Death";
                HideWhenMaxAnim = false;
                MaxAnimations = 1;
                IsAnimating = true;
                a.CleanUp(a);
                return true;
            }

            a.CleanUp(a);
            return false;
        }
        public bool ProjectileCollision(Missile a)
        {
            if (IsAlive == false)
            {
                return false;
            }

            Health = Health - a.Damage;

            foreach (KeyValuePair<string, FrameAnimation> s in childAnimations[1].Animations)
            {
                s.Value.CountedFrames = 1;
                s.Value.CurrentFrame = 0;
            }
            childAnimations[1].CurrentAnimationName = "Burst";
            childAnimations[1].MaxAnimations = 1;
            childAnimations[1].HideWhenMaxAnim = true;
            childAnimations[1].Visible = true;
            childAnimations[1].IsAnimating = true;
            childAnimations[1].Position = Position;

            if (Health <= 0)
            {
                IsAlive = false;
                foreach (KeyValuePair<string, FrameAnimation> anim in Animations)
                {
                    anim.Value.CountedFrames = 1;
                    anim.Value.CurrentFrame = 0;
                }
                CurrentAnimationName = "Death";
                HideWhenMaxAnim = false;
                MaxAnimations = 1;
                IsAnimating = true;
                a.CleanUp();
                return true;
            }

            a.CleanUp();
            return false;
        }


        public void ReSpawnPlane(PlayerPlane player)
        {
            Random Rand = new Random();
            player.MaxAnimations = -1;
            player.Health = Rand.Next(75, 125);
            player.CurrentAnimationName = "Right";
            player.IsAnimating = true;
            player.Visible = true;
            player.childAnimations[0].Visible = true;
            player.HasReachedMaxAnimations = false;
            player.IsMoving = true;
            player.IsAlive = true;
        }

        public void UpdateAnimations(GameTime gameTime, PlayerPlane player)
        {
            if (player.CurrentAnimationName == "Death")
            {
                player.childAnimations[0].Visible = false;
            }

            if (player.CurrentAnimationName == "Explode" && player.HasReachedMaxAnimations == true)
            {
                player.ReSpawnPlane(player);
            }

            if (player.CurrentAnimationName == "Death" && player.HasReachedMaxAnimations == true)
            {
                player.CurrentAnimationName = "Explode";
                player.IsMoving = false;
                foreach (KeyValuePair<string, FrameAnimation> a in player.Animations)
                {
                    a.Value.CountedFrames = 1;
                    a.Value.CurrentFrame = 0;
                }
                player.MaxAnimations = 1;
                player.IsAnimating = true;
                player.HideWhenMaxAnim = true;
                player.HasReachedMaxAnimations = false;
            }
        }

        public override void Update(GameTime gameTime)
        {

            for( int i = 0; i < MissileList.Count; i++)
                MissileList[i].Update(gameTime);
            base.Update(gameTime);
            homingSquare.X = (int)Position.X;
            homingSquare.Y = (int)Position.Y - (homingSquare.Height / 2);

            for (int i = 0; i < childAnimations.Count; i++)
            {
                childAnimations[i].Update(gameTime);
                if (childAnimations[i].TrackParent == true)
                    childAnimations[i].TrackSprite(this, childAnimations[i], (int)childOffsets[i].X, (int)childOffsets[i].Y);
            }
        }
    }
}
