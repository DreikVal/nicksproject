using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace FriendlyEngine
{
    public class CollisionLayerReader : ContentTypeReader<CollisionLayer>
    {
        protected override CollisionLayer Read(ContentReader input, CollisionLayer existingInstance)
        {
            int height = input.ReadInt32();
            int width = input.ReadInt32();

            CollisionLayer layer = new CollisionLayer(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    layer.SetCellIndex(x, y, input.ReadInt32());

            return layer;
        }
    }
}
