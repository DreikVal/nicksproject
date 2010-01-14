using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;
using System.IO;

namespace FriendlyContent
{

    [ContentProcessor(DisplayName = "Tile Level Processor")]
    public class TileMapProcessor : ContentProcessor<XmlDocument, TileMapContent>
    {
        public override TileMapContent Process(XmlDocument input, ContentProcessorContext context)
        {
            TileMapContent tileMap = new TileMapContent();

            foreach (XmlNode rootNode in input.DocumentElement.ChildNodes)
            {
                if (rootNode.Name == "Textures")
                {
                    foreach (XmlNode textureNode in rootNode.ChildNodes)
                    {
                        string file = textureNode.Attributes["File"].Value;
                        int index = int.Parse(textureNode.Attributes["ID"].Value);

                        TileLayerTextureContent textureContent = new TileLayerTextureContent();

                        OpaqueDataDictionary data = new OpaqueDataDictionary();
                        data.Add("GenerateMipmaps", true);

                        textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(file),
                            "TextureProcessor",
                            data,
                            "TextureImporter",
                            Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file));
                        textureContent.Index = index;

                        tileMap.Textures.Add(textureContent);
                    }
                }
                else if (rootNode.Name == "DoodadTextures")
                {
                    foreach (XmlNode textureNode in rootNode.ChildNodes)
                    {
                        string file = textureNode.Attributes["File"].Value;
                        int index = int.Parse(textureNode.Attributes["ID"].Value);

                        TileLayerTextureContent textureContent = new TileLayerTextureContent();

                        OpaqueDataDictionary data = new OpaqueDataDictionary();
                        data.Add("GenerateMipmaps", true);

                        textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(file),
                            "TextureProcessor",
                            data,
                            "TextureImporter",
                            Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file));
                        textureContent.Index = index;

                        tileMap.DoodadTextures.Add(textureContent);
                    }
                }
                else if (rootNode.Name == "TileLayer")
                {
                    TileLayerContent layer = new TileLayerContent();
                    foreach (XmlNode layerNode in rootNode)
                    {
                        int index = int.Parse(rootNode.Attributes["ID"].Value);
                        layer.Index = index;
                        if (layerNode.Name == "Properties")
                        {
                            foreach (XmlNode propNode in layerNode.ChildNodes)
                            {
                                TileLayerPropertyContent propertyContent = new TileLayerPropertyContent();
                                propertyContent.Name = propNode.Name;
                                propertyContent.Value = propNode.InnerText;
                                layer.Properties.Add(propertyContent);
                            }
                        }
                        else if (layerNode.Name == "Layout")
                        {
                            int width = int.Parse(layerNode.Attributes["Width"].Value);
                            int height = int.Parse(layerNode.Attributes["Height"].Value);

                            layer.Layout = new int[height, width];

                            string layout = layerNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < width; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    layer.Layout[row, x] = cellIndex;
                                }

                                row++;
                            }
                        }
                    }
                    tileMap.Layers.Add(layer);
                }

                else if (rootNode.Name == "DoodadLayer")
                {
                    CollisionLayerContent colLayer = new CollisionLayerContent();
                    foreach (XmlNode colNode in rootNode)
                    {
                        if (colNode.Name == "Layout")
                        {
                            int width = int.Parse(colNode.Attributes["Width"].Value);
                            int height = int.Parse(colNode.Attributes["Height"].Value);

                            colLayer.Layout = new int[height, width];

                            string layout = colNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < width; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    colLayer.Layout[row, x] = cellIndex;
                                }

                                row++;
                            }
                        }
                    }
                    tileMap.DoodadLayer = colLayer;
                }

                else if (rootNode.Name == "CollisionLayer")
                {
                    CollisionLayerContent colLayer = new CollisionLayerContent();
                    foreach (XmlNode colNode in rootNode)
                    {
                        if (colNode.Name == "Types")
                        {
                            foreach (XmlNode type in colNode)
                            {
                                tileMap.CollisionTypes.Add(int.Parse(type.Attributes["ID"].Value),
                                    type.Attributes["Name"].Value as string);
                            }
                        }
                        if (colNode.Name == "Layout")
                        {
                            int width = int.Parse(colNode.Attributes["Width"].Value);
                            int height = int.Parse(colNode.Attributes["Height"].Value);

                            colLayer.Layout = new int[height, width];

                            string layout = colNode.InnerText;

                            string[] lines = layout.Split('\r', '\n');

                            int row = 0;

                            foreach (string line in lines)
                            {
                                string realLine = line.Trim();

                                if (string.IsNullOrEmpty(realLine))
                                    continue;

                                string[] cells = realLine.Split(' ');

                                for (int x = 0; x < width; x++)
                                {
                                    int cellIndex = int.Parse(cells[x]);

                                    colLayer.Layout[row, x] = cellIndex;
                                }

                                row++;
                            }
                        }
                    }
                    tileMap.Collision = colLayer;
                }
            }

            return tileMap;
        }
    }
}