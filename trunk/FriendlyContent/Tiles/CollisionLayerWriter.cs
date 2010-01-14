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
    public class CollisionLayerWriter : ContentTypeWriter<CollisionLayerContent>
    {
        protected override void Write(ContentWriter output, CollisionLayerContent value)
        {
            output.Write(value.Layout.GetLength(0));
            output.Write(value.Layout.GetLength(1));
            for(int y = 0; y < value.Layout.GetLength(0); y++)
                for (int x = 0; x < value.Layout.GetLength(1); x++)
                    output.Write(value.Layout[y,x]);

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "TileEngine.CollisionLayerReader, TileEngine";
        }
    }
}
