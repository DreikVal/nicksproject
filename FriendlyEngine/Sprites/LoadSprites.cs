using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace FriendlyEngine
{
    public class LoadSprites
    {
        public NpcPlane FromFile(ContentManager content, string filename)
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

        public PlayerPlane PlayerFromFile(ContentManager content, string filename)
        {
            PlayerPlane npc = new PlayerPlane();
            AnimatedSprite child;
            List<AnimatedSprite> childAnims = new List<AnimatedSprite>();
            List<string> textureNames = new List<string>();
            List<string> animationKeys = new List<string>();
            Dictionary<string, List<int>> animationDict = new Dictionary<string, List<int>>();
            Dictionary<string, string> boolsDict = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingAnimations = false;
                bool readingBools = false;
                bool readingEndSprite = false;
                bool readingMaxCount = false;
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
                    }
                    else if (line.Contains("[Animations]"))
                    {
                        readingAnimations = true;
                        readingTextures = false;
                        readingEndSprite = false;
                        readingBools = false;
                        readingMaxCount = false;
                    }
                    else if (line.Contains("[Bools]"))
                    {
                        readingBools = true;
                        readingAnimations = false;
                        readingTextures = false;
                        readingEndSprite = false;
                        readingMaxCount = false;
                    }
                    else if (line.Contains("[EndSprite]"))
                    {
                        readingEndSprite = true;
                        readingAnimations = false;
                        readingTextures = false;
                        readingBools = false;
                        readingMaxCount = false;
                    }
                    else if (line.Contains("[MaxCount]"))
                    {
                        readingEndSprite = false;
                        readingAnimations = false;
                        readingTextures = false;
                        readingBools = false;
                        readingMaxCount = true;
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
                            npc.childAnimations.Add(child);
                            npc.childOffsets.Add(new Vector2(0, 0));
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
    }
}
