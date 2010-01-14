using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace FriendlyContent
{
    [ContentTypeWriter]
    public class TileMapWriter : ContentTypeWriter<TileMapContent>
    {
        protected override void Write(ContentWriter output, TileMapContent value)
        {

            output.Write(value.Textures.Count);
            foreach (TileLayerTextureContent textureContent in value.Textures)
            {
                output.WriteExternalReference<TextureContent>(textureContent.Texture);
                output.Write(textureContent.Index);
            }

            output.Write(value.Layers.Count);
            foreach (TileLayerContent layer in value.Layers)
            {
                output.Write(layer.Layout.GetLength(0));
                output.Write(layer.Layout.GetLength(1));
                for (int y = 0; y < layer.Layout.GetLength(0); y++)
                    for (int x = 0; x < layer.Layout.GetLength(1); x++)
                        output.Write(layer.Layout[y, x]);

                output.Write(layer.Properties.Count);

                foreach (TileLayerPropertyContent propContent in layer.Properties)
                {
                    output.Write(propContent.Name);
                    output.Write(propContent.Value);
                }
            }

            output.Write(value.DoodadLayer.Layout.GetLength(0));
            output.Write(value.DoodadLayer.Layout.GetLength(1));
            for (int y = 0; y < value.DoodadLayer.Layout.GetLength(0); y++)
                for (int x = 0; x < value.DoodadLayer.Layout.GetLength(1); x++)
                    output.Write(value.DoodadLayer.Layout[y, x]);

            output.Write(value.DoodadTextures.Count);
            foreach (TileLayerTextureContent textureContent in value.DoodadTextures)
            {
                output.WriteExternalReference<TextureContent>(textureContent.Texture);
                output.Write(textureContent.Index);
            }

            output.Write(value.Collision.Layout.GetLength(0));
            output.Write(value.Collision.Layout.GetLength(1));
            for (int y = 0; y < value.Collision.Layout.GetLength(0); y++)
                for (int x = 0; x < value.Collision.Layout.GetLength(1); x++)
                    output.Write(value.Collision.Layout[y, x]);

            output.Write(value.CollisionTypes.Count);
            foreach (KeyValuePair<int, string> kvp in value.CollisionTypes)
            {
                output.Write(kvp.Key);
                output.Write(kvp.Value);
            }



        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {

            return "FriendlyEngine.TileMapReader, FriendlyEngine";
        }
    }
}
