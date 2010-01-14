using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace FriendlyEngine
{
    public class TileMapReader : ContentTypeReader<TileMap>
    {
        protected override TileMap Read(ContentReader input, TileMap existingInstance)
        {
            TileMap map = new TileMap();

            List<TempTexture> textures = new List<TempTexture>();

            int maxTextures = input.ReadInt32();
            for (int i = 0; i < maxTextures; i++)
            {
                TempTexture t = new TempTexture();
                t.Texture = input.ReadExternalReference<Texture2D>();
                t.Index = input.ReadInt32();
                textures.Add(t);
            }

            textures.Sort(delegate(TempTexture a, TempTexture b)
            {
                return a.Index.CompareTo(b.Index);
            });

            foreach (TempTexture t in textures)
                map.AddTexture(t.Texture);

            int maxLayers = input.ReadInt32();
            for (int o = 0; o < maxLayers; o++)
            {
                int height = input.ReadInt32();
                int width = input.ReadInt32();
                TileLayer layer = new TileLayer(width, height);

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        layer.SetCellIndex(x, y, input.ReadInt32());


                int maxProperties = input.ReadInt32();

                for (int i = 0; i < maxProperties; i++)
                {
                    string name = input.ReadString();
                    string value = input.ReadString();

                    PropertyInfo propInfo = typeof(TileLayer).GetProperty(name);
                    object realValue = null;

                    if (propInfo.PropertyType == typeof(float))
                        realValue = float.Parse(value);
                    else if (propInfo.PropertyType == typeof(int))
                        realValue = int.Parse(value);
                    else if (propInfo.PropertyType == typeof(string))
                        realValue = value;

                    propInfo.SetValue(layer, realValue, null);
                }

                map.Layers.Add(layer);
            }

            int dheight = input.ReadInt32();
            int dwidth = input.ReadInt32();

            DoodadLayer doodadLayer = new DoodadLayer(dwidth, dheight);

            for (int y = 0; y < dheight; y++)
                for (int x = 0; x < dwidth; x++)
                    doodadLayer.SetCellIndex(x, y, input.ReadInt32());

            map.DoodadLayer = doodadLayer;

            List<TempTexture> doodadtextures = new List<TempTexture>();

            int doodadmaxTextures = input.ReadInt32();
            for (int i = 0; i < doodadmaxTextures; i++)
            {
                TempTexture t = new TempTexture();
                t.Texture = input.ReadExternalReference<Texture2D>();
                t.Index = input.ReadInt32();
                doodadtextures.Add(t);
            }

            doodadtextures.Sort(delegate(TempTexture a, TempTexture b)
            {
                return a.Index.CompareTo(b.Index);
            });

            foreach (TempTexture t in doodadtextures)
                map.DoodadLayer.AddTexture(t.Texture);

            int cheight = input.ReadInt32();
            int cwidth = input.ReadInt32();

            CollisionLayer clayer = new CollisionLayer(cwidth, cheight);

            for (int y = 0; y < cheight; y++)
                for (int x = 0; x < cwidth; x++)
                    clayer.SetCellIndex(x, y, input.ReadInt32());

            map.CollisionLayer = clayer;

            int maxTypes = input.ReadInt32();
            for (int i = 0; i < maxTypes; i++)
            {
                int key = input.ReadInt32();
                string value = input.ReadString();
                if (map.CollisionLayer.Types.ContainsKey(value))
                {
                }
                else
                  map.CollisionLayer.Types.Add(value, key);
            }

            return map;
        }
    }

    class TempTexture
    {
        public Texture2D Texture;
        public int Index;
    }

}
