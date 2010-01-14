using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class TileMap
    {
        public List<Texture2D> tileTextures = new List<Texture2D>();
        public List<AnimatedSprite> animatedTextures = new List<AnimatedSprite>();
        public List<TileLayer> Layers = new List<TileLayer>();
        public CollisionLayer CollisionLayer;
        public DoodadLayer DoodadLayer;
        public int LayerCount = 0;

        public int GetWidthInPixels()
        {
            return GetWidth() * Engine.TileWidth;
        }

        public int GetHeightInPixels()
        {
            return GetHeight() * Engine.TileHeight;
        }

        public int GetWidth()
        {
            int width = -10000;

            foreach (TileLayer layer in Layers)
                width = (int)Math.Max(width, layer.Width);

            return width;
        }

        public int GetHeight()
        {
            int height = -10000;

            foreach (TileLayer layer in Layers)
                height = (int)Math.Max(height, layer.Height);

            return height;
        }

        public void ClearTextureList()
        {
            if(tileTextures != null)
            tileTextures.Clear();
        }

        public Texture2D RetrieveTexture(int index)
        {
            return tileTextures[index];
        }

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void AddAnimatedSprite(AnimatedSprite woof)
        {
            animatedTextures.Add(woof);
        }

        public void AddAnimatedFromTex(Texture2D texture)
        {
            AnimatedSprite woof = new AnimatedSprite(texture);
            animatedTextures.Add(woof);
        }

        public void RemoveTexture(Texture2D texture, TileMap tileMap)
        {
            for(int i = 0; i < tileMap.LayerCount; i++)
            tileMap.Layers[i].RemoveIndex(tileTextures.IndexOf(texture));
            tileTextures.Remove(texture);
        }

        public void LoadTileTextures(ContentManager content, params string[] textureNames)
        {
            Texture2D texture;
            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
                tileTextures.Add(texture);
            }

        }

        public int ReturnTextureIndex(Texture2D texture)
        {
            if (tileTextures.Contains(texture))
                return tileTextures.IndexOf(texture);

            return -1;
        }

        public static TileMap FromFile(string filename, out string[] textureNameArray, out string[] doodadTexNameArray)
        {
            TileMap tileMap;

            List<string> textureNames = new List<string>();
            List<string> doodadTextureNames = new List<string>();
            tileMap = ProcessFile(filename, textureNames, doodadTextureNames);

            textureNameArray = textureNames.ToArray();
            doodadTexNameArray = doodadTextureNames.ToArray();

            return tileMap;
        }

        public static TileMap FromFile(ContentManager content, string filename)
        {
            TileMap tileMap;

            List<string> textureNames = new List<string>();
            List<string> doodadTextureNames = new List<string>();
            tileMap = ProcessFile(filename, textureNames, doodadTextureNames);
            Texture2D texture;
            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
                tileMap.AddTexture(texture);
            }
            foreach (string doodadName in doodadTextureNames)
            {
                texture = content.Load<Texture2D>(doodadName);
                tileMap.DoodadLayer.AddTexture(texture);
            };
           

            return tileMap;
        }

        private static TileMap ProcessFile(string filename, List<string> textureNames, List<string> doodadTextureNames)
        {
            TileMap tileMap = new TileMap();
            TileLayer tileLayer;
            CollisionLayer collisionLayer;
            DoodadLayer doodadLayer;
            List<List<int>> tempLayer = new List<List<int>>();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            Dictionary<string, int> ttypes = new Dictionary<string, int>();
            Dictionary<string, int> dtypes = new Dictionary<string, int>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingLayer = false;
                bool readingEndLayer = false;
                bool readingEndDoodads = false;
                bool readingProperties = false;
                bool readingCollTypes = false;
                bool readingDoodTex = false;
                bool readingDoodads = false;
                bool readingCollision = false;
                bool readingEndFile = false;
                int layerCount = 0;
                int doodadCount = 0;
                int collisionCount = 0;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;
                    #region Bools
                    if (line.Contains("[Textures]"))
                    {
                        readingTextures = true;
                        readingLayer = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingDoodTex = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;

                    }
                    else if (line.Contains("[Collision Types]"))
                    {
                        readingCollTypes = true;
                        readingProperties = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingDoodTex = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[Doodad Textures]"))
                    {
                        readingDoodTex = true;
                        readingDoodads = false;
                        readingCollTypes = false;
                        readingProperties = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingCollision = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[Properties]"))
                    {
                        readingProperties = true;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingCollTypes = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[Layer]"))
                    {
                        readingLayer = true;
                        readingDoodTex = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[Collision]"))
                    {
                        readingCollision = true;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingDoodads = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[Doodads]"))
                    {
                        readingDoodads = true;
                        readingCollision = false;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingEndFile = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[EndFile]"))
                    {
                        readingEndFile = true;
                        readingDoodads = false;
                        readingCollision = false;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingEndLayer = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[EndLayer]"))
                    {
                        readingEndLayer = true;
                        readingEndFile = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                        readingEndDoodads = false;
                    }
                    else if (line.Contains("[EndDoodads]"))
                    {
                        readingEndDoodads = true;
                        readingEndLayer = false;
                        readingEndFile = false;
                        readingDoodads = false;
                        readingCollision = false;
                        readingDoodTex = false;
                        readingLayer = false;
                        readingTextures = false;
                        readingProperties = false;
                        readingCollTypes = false;
                    }
                    #endregion 
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                    else if (readingDoodTex)
                    {
                        doodadTextureNames.Add(line);
                    }
                    else if (readingCollTypes)
                    {
                        string[] pair = line.Split('=');
                        string value = pair[0].Trim();
                        string key = pair[1].Trim();

                        ttypes.Add(key, int.Parse(value));
                    }
                    else if (readingProperties)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        properties.Add(key, value);
                    }
                    else if (readingLayer)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayer.Add(row);
                        layerCount++;
                    }
                    else if (readingDoodads)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayer.Add(row);
                        doodadCount++;
                    }
                    else if (readingCollision)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayer.Add(row);
                        collisionCount++;
                    }
                    if (readingEndLayer)
                    {
                        if (layerCount >= 1)
                        {
                            int width = tempLayer[0].Count;
                            int height = tempLayer.Count;

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
                                    tileLayer.SetCellIndex(x, y, tempLayer[y][x]);

                            tileMap.Layers.Add(tileLayer);
                            properties.Clear();
                            tempLayer.Clear();
                        }
                    }
                    if (readingEndDoodads)
                    {
                        if (doodadCount >= 1)
                        {
                            int width = tempLayer[0].Count;
                            int height = tempLayer.Count;

                            doodadLayer = new DoodadLayer(width, height);

                            for (int y = 0; y < height; y++)
                                for (int x = 0; x < width; x++)
                                    doodadLayer.SetCellIndex(x, y, tempLayer[y][x]);

                            tileMap.DoodadLayer = doodadLayer;
                            tempLayer.Clear();
                        }

                    }
                    if (readingEndFile)
                    {
                        if (collisionCount >= 1)
                        {
                            int width = tempLayer[0].Count;
                            int height = tempLayer.Count;

                            collisionLayer = new CollisionLayer(width, height);

                            collisionLayer.Types = ttypes;

                            for (int y = 0; y < height; y++)
                                for (int x = 0; x < width; x++)
                                    collisionLayer.SetCellIndex(x, y, tempLayer[y][x]);
                            tileMap.CollisionLayer = collisionLayer;
                            tempLayer.Clear();
                        }
                    }
                }
            }
            tileMap.LayerCount = 0;
            foreach (TileLayer l in tileMap.Layers)
                tileMap.LayerCount++;
            return tileMap;
        }

        public static TileMap ProcessFile(XmlDocument input, Dictionary<int, string> textureNames, Dictionary<int, string> doodadTextureNames, Dictionary<int, string> layerNames)
        {
            TileMap tileMap = new TileMap();
            TileLayer tileLayer = new TileLayer(1, 1);
            CollisionLayer collisionLayer = new CollisionLayer(1, 1);
            DoodadLayer doodadLayer = new DoodadLayer(1, 1);

            foreach (XmlNode rootNode in input.DocumentElement.ChildNodes)
            {
                if (rootNode.Name == "Textures")
                {
                    foreach (XmlNode textureNode in rootNode.ChildNodes)
                    {
                        string file = textureNode.Attributes["File"].Value;
                        int index = int.Parse(textureNode.Attributes["ID"].Value);

                        textureNames.Add(index, file);
                    }
                }
                else if (rootNode.Name == "DoodadTextures")
                {
                    foreach (XmlNode textureNode in rootNode.ChildNodes)
                    {
                        string file = textureNode.Attributes["File"].Value;
                        int index = int.Parse(textureNode.Attributes["ID"].Value);

                        doodadTextureNames.Add(index, file);
                    }
                }
                else if (rootNode.Name == "TileLayer")
                {
                    int index = int.Parse(rootNode.Attributes["ID"].Value);
                    layerNames.Add(index, "Layer" + index as string);

                    foreach (XmlNode layerNode in rootNode)
                    {
                        if (layerNode.Name == "Properties")
                        {
                            foreach (XmlNode propNode in layerNode.ChildNodes)
                            {
                                if (propNode.Name == "Alpha")
                                    tileLayer.Alpha = float.Parse(propNode.InnerText);
                            }
                        }
                        else if (layerNode.Name == "Layout")
                        {
                            int processwidth = int.Parse(layerNode.Attributes["Width"].Value);
                            int processheight = int.Parse(layerNode.Attributes["Height"].Value);

                            tileLayer = new TileLayer(processwidth, processheight);

                            string layout = layerNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < processwidth; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    tileLayer.SetCellIndex(x, row, cellIndex);
                                }

                                row++;
                            }

                            tileMap.Layers.Add(tileLayer);
                        }
                    }
                }

                else if (rootNode.Name == "DoodadLayer")
                {
                    foreach (XmlNode layerNode in rootNode)
                    {
                        if (layerNode.Name == "Layout")
                        {
                            int processwidth = int.Parse(layerNode.Attributes["Width"].Value);
                            int processheight = int.Parse(layerNode.Attributes["Height"].Value);

                            doodadLayer = new DoodadLayer(processwidth, processheight);

                            string layout = layerNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < processwidth; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    doodadLayer.SetCellIndex(x, row, cellIndex);
                                }

                                row++;
                            }
                        }
                        tileMap.DoodadLayer = doodadLayer;
                    }
                }

                else if (rootNode.Name == "CollisionLayer")
                {
                    foreach (XmlNode layerNode in rootNode)
                    {
                        if (layerNode.Name == "Types")
                        {
                            foreach (XmlNode type in layerNode)
                            {
                                if(collisionLayer.Types.ContainsKey(type.Attributes["Name"].Value as string))
                                {
                                }
                                else
                                {
                                collisionLayer.Types.Add(type.Attributes["Name"].Value as string,
                                    int.Parse(type.Attributes["ID"].Value));
                                }
                            }
                        }
                        if (layerNode.Name == "Layout")
                        {
                            int processwidth = int.Parse(layerNode.Attributes["Width"].Value);
                            int processheight = int.Parse(layerNode.Attributes["Height"].Value);

                            collisionLayer = new CollisionLayer(processwidth, processheight);

                            string layout = layerNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < processwidth; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    collisionLayer.SetCellIndex(x, row, cellIndex);
                                }

                                row++;
                            }
                        }
                        tileMap.CollisionLayer = collisionLayer;
                    }
                }
            }

            return tileMap;
        }

        public void Save(string filename, string[] textureNames, string[] doodadTextureNames, Dictionary<string, int> collisionTypes)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Textures]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Doodad Textures]");
                foreach (string t in doodadTextureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Collision Types]");
                foreach (KeyValuePair<string, int> t in CollisionLayer.Types)
                {
                    writer.WriteLine(t.Value + " = " + t.Key);
                }
                writer.WriteLine();

                foreach (TileLayer l in Layers)
                {
                    writer.WriteLine("[Properties]");
                    writer.WriteLine("Alpha = " + l.Alpha.ToString());
                    writer.WriteLine();
                    writer.WriteLine("[Layer]");
                    for (int y = 0; y < l.Height; y++)
                    {

                        string line = string.Empty;
                        for (int x = 0; x < l.Width; x++)
                        {
                            line += l.GetCellIndex(x, y).ToString() + " ";
                        }
                        writer.WriteLine(line);
                    }
                    writer.WriteLine("[EndLayer]");
                    writer.WriteLine();
                }
                writer.WriteLine("[Doodads]");
                for (int y = 0; y < DoodadLayer.Height; y++)
                {

                    string line = string.Empty;
                    for (int x = 0; x < DoodadLayer.Width; x++)
                    {
                        line += DoodadLayer.GetCellIndex(x, y).ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
                writer.WriteLine("[EndDoodads]");
                writer.WriteLine();

                writer.WriteLine("[Collision]");
                for (int y = 0; y < CollisionLayer.Height; y++)
                {

                    string line = string.Empty;
                    for (int x = 0; x < CollisionLayer.Width; x++)
                    {
                        line += CollisionLayer.GetCellIndex(x, y).ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
                writer.WriteLine();
                writer.WriteLine("[EndFile]");
            }

        }

        public void Save(string filename, Dictionary<int, string> textureNames, Dictionary<int, string> doodadNames, TileMap tileMap)
        {
            XmlDocument document;
            XmlNode node;
            XmlElement element;
            XmlElement element2;
            XmlElement element3;
            XmlElement innerment;
            XmlNode holding;
            XmlText text;

            document = new XmlDocument();

            node = document.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            document.AppendChild(node);

            //textures and root

            element2 = document.CreateElement("Textures");
            element = document.CreateElement("TileMap");
            element.AppendChild(element2);
            document.AppendChild(element);

            foreach (KeyValuePair<int, string> a in textureNames)
            {
                element = document.CreateElement("Texture");
                element.SetAttribute("File", a.Value);
                element.SetAttribute("ID", a.Key.ToString());
                holding = document.GetElementsByTagName("Textures")[0];
                holding.AppendChild(element);
            }

            //doodad textures

            element = document.CreateElement("DoodadTextures");
            holding = document.GetElementsByTagName("TileMap")[0];
            holding.AppendChild(element);
            foreach (KeyValuePair<int, string> a in doodadNames)
            {
                element = document.CreateElement("Texture");
                element.SetAttribute("File", a.Value);
                element.SetAttribute("ID", a.Key.ToString());
                holding = document.GetElementsByTagName("DoodadTextures")[0];
                holding.AppendChild(element);
            }

            //layers

            for(int i = 0; i < tileMap.Layers.Count; i++)
            {
                element = document.CreateElement("TileLayer");
                element.SetAttribute("ID", i.ToString());

                element2 = document.CreateElement("Properties");
                innerment = document.CreateElement("Alpha");
                text = document.CreateTextNode(tileMap.Layers[i].Alpha.ToString());
                innerment.AppendChild(text);
                element2.AppendChild(innerment);

                element3 = document.CreateElement("Layout");
                element3.SetAttribute("Width", tileMap.Layers[i].Width.ToString());
                element3.SetAttribute("Height", tileMap.Layers[i].Height.ToString());

                text = document.CreateTextNode("\r\n\t\t\t");
                element3.AppendChild(text);

                for (int y = 0; y < tileMap.Layers[i].Height; y++)
                {
                    for (int x = 0; x < tileMap.Layers[i].Width + 1; x++)
                    {
                        if (x == tileMap.Layers[i].Width)
                        {
                            text = document.CreateTextNode("\r\n\t\t\t");
                            element3.AppendChild(text);
                            break;
                        }
                        text = document.CreateTextNode(tileMap.Layers[i].GetCellIndex(x, y).ToString());
                        text.AppendData(" ");
                        element3.AppendChild(text);
                    }
                }

                element.AppendChild(element2);
                element.AppendChild(element3);
                holding = document.GetElementsByTagName("TileMap")[0];
                holding.AppendChild(element);
            }

            //doodad layer

            element = document.CreateElement("DoodadLayer");

            element3 = document.CreateElement("Layout");
            element3.SetAttribute("Width", tileMap.DoodadLayer.Width.ToString());
            element3.SetAttribute("Height", tileMap.DoodadLayer.Height.ToString());

            text = document.CreateTextNode("\r\n\t\t\t");
            element3.AppendChild(text);

            for (int y = 0; y < tileMap.DoodadLayer.Height; y++)
            {
                for (int x = 0; x < tileMap.DoodadLayer.Width + 1; x++)
                {
                    if (x == tileMap.DoodadLayer.Width)
                    {
                        text = document.CreateTextNode("\r\n\t\t\t");
                        element3.AppendChild(text);
                        break;
                    }
                    text = document.CreateTextNode(tileMap.DoodadLayer.GetCellIndex(x, y).ToString());
                    text.AppendData(" ");
                    element3.AppendChild(text);
                }
            }

            element.AppendChild(element3);
            holding = document.GetElementsByTagName("TileMap")[0];
            holding.AppendChild(element);

            //collision

            element = document.CreateElement("CollisionLayer");
            element2 = document.CreateElement("Types");
            foreach (KeyValuePair<string, int> a in tileMap.CollisionLayer.Types)
            {
                innerment = document.CreateElement("Type");
                innerment.SetAttribute("Name", a.Key);
                innerment.SetAttribute("ID", a.Value.ToString());
                element2.AppendChild(innerment);
            }

            element3 = document.CreateElement("Layout");
            element3.SetAttribute("Width", tileMap.CollisionLayer.Width.ToString());
            element3.SetAttribute("Height", tileMap.CollisionLayer.Height.ToString());

            text = document.CreateTextNode("\r\n\t\t\t");
            element3.AppendChild(text);

            for (int y = 0; y < tileMap.CollisionLayer.Height; y++)
            {
                for (int x = 0; x < tileMap.CollisionLayer.Width + 1; x++)
                {
                    if (x == tileMap.CollisionLayer.Width)
                    {
                        text = document.CreateTextNode("\r\n\t\t\t");
                        element3.AppendChild(text);
                        break;
                    }
                    text = document.CreateTextNode(tileMap.CollisionLayer.GetCellIndex(x, y).ToString());
                    text.AppendData(" ");
                    element3.AppendChild(text);
                }
            }

            element.AppendChild(element2);
            element.AppendChild(element3);
            holding = document.GetElementsByTagName("TileMap")[0];
            holding.AppendChild(element);

            document.Save(filename);

        }

        public void Update(Point min, Point max, GameTime gt)
        {
            if (animatedTextures.Count != 0)
            {
                foreach (AnimatedSprite a in animatedTextures)
                {
                    if (a.Position.X <= max.X && a.Position.X >= min.X
                        && a.Position.Y <= max.Y && a.Position.Y >= min.Y)
                        a.Update(gt);
                }
            }
        }

        public void Update(Camera camera, int screenWidth, int screenHeight, GameTime gt, ExperimentEngine engine)
        {
            Point min = engine.ConvertPositionToCell(camera.Position);
            Point max = engine.ConvertPositionToCell(
                camera.Position + new Vector2(
                screenWidth + engine.TileWidth,
                screenHeight + engine.TileHeight));

            if (animatedTextures.Count != 0)
            {
                foreach (AnimatedSprite a in animatedTextures)
                {
                    if (a.Position.X <= max.X && a.Position.X >= min.X
                        && a.Position.Y <= max.Y && a.Position.Y >= min.Y)
                        a.Update(gt);
                }
            }
        }

        public void Update(Camera camera, int screenWidth, int screenHeight, TimeSpan gt)
        {
            Point min = Engine.ConvertPositionToCell(camera.Position);
            Point max = Engine.ConvertPositionToCell(
                camera.Position + new Vector2(
                screenWidth + Engine.TileWidth,
                screenHeight + Engine.TileHeight));

            if (animatedTextures.Count != 0)
            {
                foreach (AnimatedSprite a in animatedTextures)
                {
                    if (a.Position.X <= max.X && a.Position.X >= min.X
                        && a.Position.Y <= max.Y && a.Position.Y >= min.Y)
                        a.Update(gt);
                }
            }
        }

        public void Draw(SpriteBatch batch, Camera camera)
        {
            Point min = Engine.ConvertPositionToCell(camera.Position);
            Point max = Engine.ConvertPositionToCell(
                camera.Position + new Vector2(
                batch.GraphicsDevice.Viewport.Width + Engine.TileWidth,
                batch.GraphicsDevice.Viewport.Height + Engine.TileHeight));


            //foreach (TileLayer layer in Layers)
            //    layer.Draw(batch, camera, min, max, tileTextures);

            if(DoodadLayer != null)
                DoodadLayer.Draw(batch, camera, min, max);
           
        }

        public void DrawExperiment(SpriteBatch batch, Camera camera, ExperimentEngine engine)
        {
            Point min = engine.ConvertPositionToCell(camera.Position);
            Point max = engine.ConvertPositionToCell(
                camera.Position + new Vector2(
                batch.GraphicsDevice.Viewport.Width + engine.TileWidth,
                batch.GraphicsDevice.Viewport.Height + engine.TileHeight));


            foreach (TileLayer layer in Layers)
                layer.Draw(batch, camera, min, max, tileTextures, engine);

            if (DoodadLayer != null)
                DoodadLayer.Draw(batch, camera, min, max);

        }
    }
}
