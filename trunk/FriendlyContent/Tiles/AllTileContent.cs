using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace FriendlyContent
{
    public class TileMapContent
    {
        public Collection<TileLayerTextureContent> Textures = new Collection<TileLayerTextureContent>();
        public Collection<TileLayerContent> Layers = new Collection<TileLayerContent>();
        public CollisionLayerContent DoodadLayer = new CollisionLayerContent();
        public Collection<TileLayerTextureContent> DoodadTextures = new Collection<TileLayerTextureContent>();
        public CollisionLayerContent Collision = new CollisionLayerContent();
        public Dictionary<int, string> CollisionTypes = new Dictionary<int, string>(); 
    }

    public class CollisionLayerContent
    {
        public int[,] Layout;
    }

    public class TileLayerContent
    {
        public Collection<TileLayerPropertyContent> Properties = new Collection<TileLayerPropertyContent>();

        public int[,] Layout;
        public int Index;
    }

    public class TileLayerTextureContent
    {
        public ExternalReference<TextureContent> Texture;
        public int Index;
    }

    public class TileLayerPropertyContent
    {
        public string Name;
        public string Value;
    }
}
