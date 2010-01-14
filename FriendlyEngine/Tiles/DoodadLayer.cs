using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class DoodadLayer
    {
        int[,] map;
        string name;
        List<Texture2D> doodadTextures = new List<Texture2D>();

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

        public int IsUsingTexture(Texture2D texture)
        {
            if (doodadTextures.Contains(texture))
                return doodadTextures.IndexOf(texture);

            return -1;
        }

        public DoodadLayer(int width, int height)
        {
            map = new int[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }

        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Textures]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Doodads]");

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

        public static DoodadLayer FromFile(string filename)
        {
            DoodadLayer DoodadLayer;
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
                        readingTypes = false;
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

            DoodadLayer = new DoodadLayer(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    DoodadLayer.SetCellIndex(x, y, tempLayout[y][x]);

            return DoodadLayer;
        }

        public void SetTextureList(List<Texture2D> tex)
        {
            if (tex != null)
                doodadTextures = tex;
        }

        public void AddTexture(Texture2D texture)
        {
            doodadTextures.Add(texture);
        }

        public void RemoveTexture(Texture2D texture)
        {
            RemoveIndex(doodadTextures.IndexOf(texture));
            doodadTextures.Remove(texture);
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

        public void Draw(SpriteBatch batch, Camera camera)
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

                    Texture2D texture = doodadTextures[textureIndex];

                    batch.Draw(
                        texture,
                        new Rectangle(
                            x * Engine.TileHeight,
                            y * Engine.TileHeight,
                            Engine.TileHeight,
                            Engine.TileHeight),
                        Color.White);
                }
            }

            batch.End();

        } 

        public void Draw(SpriteBatch batch, Camera camera, Point min, Point max)
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

                    Texture2D texture = doodadTextures[textureIndex];

                    batch.Draw(
                        texture,
                        new Rectangle(
                            x * Engine.TileHeight,
                            y * Engine.TileHeight,
                            Engine.TileHeight,
                            Engine.TileHeight),
                        Color.White);
                }
            }

            batch.End();

        }
        public void DrawExperiment(SpriteBatch batch, Camera camera, Point min, Point max, ExperimentEngine engine)
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

                    Texture2D texture = doodadTextures[textureIndex];

                    batch.Draw(
                        texture,
                        new Rectangle(
                            x * engine.TileHeight,
                            y * engine.TileHeight,
                            engine.TileHeight,
                            engine.TileHeight),
                        Color.White);
                }
            }

            batch.End();

        }
    }
}
