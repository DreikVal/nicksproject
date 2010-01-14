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

    [ContentProcessor(DisplayName = "Tile Layer Processor")]
    public class TileLayerProcessor : ContentProcessor<XmlDocument, TileLayerContent>
    {
        public override TileLayerContent Process(XmlDocument input, ContentProcessorContext context)
        {
            TileLayerContent layer = new TileLayerContent();

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

                        layer.Textures.Add(textureContent);
                    }
                }
                else if (rootNode.Name == "Properties")
                {
                    foreach (XmlNode propNode in rootNode.ChildNodes)
                    {
                        TileLayerPropertyContent propertyContent = new TileLayerPropertyContent();
                        propertyContent.Name = propNode.Name;
                        propertyContent.Value = propNode.InnerText;
                        layer.Properties.Add(propertyContent);
                    }
                }
                else if (rootNode.Name == "Layout")
                {
                    int width = int.Parse(rootNode.Attributes["Width"].Value);
                    int height = int.Parse(rootNode.Attributes["Height"].Value);

                    layer.Layout = new int[height, width];

                    string layout = rootNode.InnerText;

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

            return layer;
        }
    }
}