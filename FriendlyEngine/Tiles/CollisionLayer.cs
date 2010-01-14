using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class CollisionLayer
    {
        int[,] map;
        string name;
        Dictionary<string, int> types = new Dictionary<string, int>();
        List<Color> Colors = new List<Color>();

        public Dictionary<string, int> Types
        {
            get { return types; }
            set
            {
                types = value;
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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public CollisionLayer(int width, int height)
        {
            map = new int[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = 0;

            types.Add("Slow", 1);
            types.Add("Impassable", 2);
            Colors.Add(Color.White);
            Colors.Add(new Color(new Vector4(1f, 1f, 0f, 1f)));
            Colors.Add(new Color(new Vector4(1f, 0f, 1f, 1f)));
        }

        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Types]");

                types.Values.ToString();

                foreach(KeyValuePair<string, int> type in types)
                    writer.WriteLine(type.Value.ToString() + " = " + type.Key.ToString());

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

        public static CollisionLayer FromFile(string filename)
        {
            CollisionLayer CollisionLayer;
            List<List<int>> tempLayout = new List<List<int>>();
            Dictionary<string, int> ttypes = new Dictionary<string, int>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingLayout = false;
                bool readingTypes = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Types]"))
                    {
                        readingTypes = true;
                        readingLayout = false;
                    }

                    else if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                        readingTypes =  false;
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
                    else if (readingTypes)
                    {
                        string[] pair = line.Split('=');
                        string value = pair[0].Trim();
                        string key = pair[1].Trim();

                        ttypes.Add(key, int.Parse(value));
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            CollisionLayer = new CollisionLayer(width, height);

            CollisionLayer.Types = ttypes;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    CollisionLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return CollisionLayer;
        }

        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public int GetCellIndex(Point point)
        {
            if (point.Y <= Height - 1 && point.X <= Width - 1)
            {
                return map[point.Y, point.X];
            }
            else 
                return 2;

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

        public Color GetColors(int colorIndex)
        {
            return Colors[colorIndex];
        }

        public int GetColorCount
        {
            get
            {
                return Colors.Count;
            }
        }

        public void SetColors(Vector4 color, int index)
        {
            if(Colors[index] != null)
            {
                Colors.RemoveAt(index);
            }
            Colors.Insert(index, new Color(color));
        }

        public void Draw(SpriteBatch batch, Camera camera, Texture2D tex)
        {
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Immediate,
                SaveStateMode.None,
                camera.TransformMatrix);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == 0)
                        continue;

                    batch.Draw(
                        tex,
                        new Rectangle(
                            x * Engine.TileHeight,
                            y * Engine.TileHeight,
                            Engine.TileHeight,
                            Engine.TileHeight),
                        Colors[textureIndex]);
                }
            }

            batch.End();

        }
    }
}
