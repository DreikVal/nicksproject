using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class TileLayer
    {
        int[,] map;
        int DrawCalls = 0;
        float alpha = 1f;
        bool visible = true;


        public float Alpha
        {
            get { return alpha; }
            set
            {
                alpha = MathHelper.Clamp(value, 0f, 1f);
            }
        }


        public int WidthInPixels
        {
            get
            {
                return Width * Engine.TileHeight;
            }
        }
        public int HeightInPixels
        {
            get
            {
                return Height * Engine.TileHeight;
            }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public TileLayer(int width, int height)
        {
            map = new int[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }

        public TileLayer(int[,] existingMap)
        {
            map = (int[,])existingMap.Clone();
        }

        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Textures]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Properties]");
                writer.WriteLine("Alpha = " + Alpha.ToString());
                writer.WriteLine();

                writer.WriteLine("[Layout]");

                for (int y = 0; y < Height; y++)
                {

                    string line = string.Empty;
                    for (int x = 0; x < Width; x++)
                    {
                        line += map[y, x].ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
            }

        }

         public static TileLayer FromFile(string filename, out string[] textureNameArray)
        {
            TileLayer tileLayer;

            List<string> textureNames = new List<string>();
            tileLayer = ProcessFile(filename, textureNames);

            textureNameArray = textureNames.ToArray();

            return tileLayer;
        }

        public static TileLayer FromFile(ContentManager content, string filename)
        {
            TileLayer tileLayer;

            List<string> textureNames = new List<string>();
            tileLayer = ProcessFile(filename, textureNames);

            return tileLayer;
        }

        private static TileLayer ProcessFile(string filename, List<string> textureNames)
        {
            TileLayer tileLayer;
            List<List<int>> tempLayout = new List<List<int>>();
            Dictionary<string, string> properties = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingLayout = false;
                bool readingProperties = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Textures]"))
                    {
                        readingTextures = true;
                        readingLayout = false;
                        readingProperties = false;
                    }
                    else if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                        readingTextures = false;
                        readingProperties = false;
                    }
                    else if (line.Contains("[Properties]"))
                    {
                        readingProperties = true;
                        readingLayout = false;
                        readingTextures = false;
                    }
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayout.Add(row);
                    }
                    else if (readingProperties)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        properties.Add(key, value);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            tileLayer = new TileLayer(width, height);

            foreach (KeyValuePair<string, string> property in properties)
            {
                switch (property.Key)
                {
                    case "Alpha":
                        tileLayer.Alpha = float.Parse(property.Value);
                        break;
                }
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tileLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return tileLayer;
        }

        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public int GetCellIndex(Point point)
        {
            return map[point.Y, point.X];
        }

        public void SetCellIndex(int x, int y, int cellIndex)
        {
            map[y, x] = cellIndex;
        }

        public void SetCellIndex(Point point, int cellIndex)
        {
            map[point.Y, point.X] = cellIndex;
        }

        public void RemoveIndex(int existingIndex)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[y, x] == existingIndex)
                        map[y, x] = -1;
                    else if (map[y, x] > existingIndex)
                        map[y, x]--;
                }
            }
        }
        public void ReplaceIndex(int existingIndex, int newIndex)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (map[y, x] == existingIndex)
                        map[y, x] = newIndex;
        }

        public int HasIndex(int index)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (map[y, x] == index)
                        return map[y, x];

            return -1;
        }

        public void Draw(SpriteBatch batch, Camera camera, List<Texture2D> tileTextures)
        {
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Texture,
                SaveStateMode.None,
                camera.TransformMatrix);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    Texture2D texture = tileTextures[textureIndex];

                    batch.Draw(
                        texture,
                        new Rectangle(
                            x * Engine.TileHeight,
                            y * Engine.TileHeight,
                            Engine.TileHeight,
                            Engine.TileHeight),
                        new Color(new Vector4(1f, 1f, 1f, Alpha)));
                }
            }

            batch.End();

        } 

        public void Draw(SpriteBatch batch, Camera camera, Point min, Point max, List<Texture2D> tileTextures, ExperimentEngine engine)
        {
            if (visible == true)
            {
                batch.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Texture,
                    SaveStateMode.None,
                    camera.TransformMatrix);

                min.X = (int)Math.Max(min.X, 0);
                min.Y = (int)Math.Max(min.Y, 0);
                max.X = (int)Math.Min(max.X, Width);
                max.Y = (int)Math.Min(max.Y, Height);

                for (int x = min.X; x < max.X; x++)
                {
                    for (int y = min.Y; y < max.Y; y++)
                    {
                        int textureIndex = map[y, x];

                        if (textureIndex == -1)
                            continue;

                        Texture2D texture = tileTextures[textureIndex];

                        batch.Draw(
                            texture,
                            new Rectangle(
                                x * engine.TileHeight,
                                y * engine.TileHeight,
                                engine.TileHeight,
                                engine.TileHeight),
                            new Color(new Vector4(1f, 1f, 1f, Alpha)));
                    }
                }
                batch.End();
                DrawCalls++;
            }
        }
    }
}
