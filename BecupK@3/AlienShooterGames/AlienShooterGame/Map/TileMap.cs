using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class TileMap
    {
        List<TileLayer> layerList = new List<TileLayer>();
        List<TileType> tileTypes = new List<TileType>();

        public TileMap(Screen screen, int width, int height, int LayerCount)
        {
            if (LayerCount <= 0)
                LayerCount = 1;

            float alphaModifier = 1f / LayerCount;

            for (int i = 0; i < LayerCount; i++)
            {
                layerList.Add(new TileLayer(screen, width, height, alphaModifier));
                alphaModifier += (float)i / 10;
                MathHelper.Clamp(alphaModifier, 0, 1f);
            }

            tileTypes.Add(new TileType("grass_tile"));
            tileTypes.Add(new TileType("dirt_tile"));
        }

        public void Update(WorldScreen screen, GameTime time)
        {
            Point min = MapGlobals.ConvertPositionToCell(screen.ViewPort.TargetLocation);
            Point max = MapGlobals.ConvertPositionToCell(
                screen.ViewPort.TargetLocation + new Vector2(
                screen.ViewPort.Size.X + MapGlobals.TileWidth,
                screen.ViewPort.Size.Y + MapGlobals.TileHeight));

            foreach (TileLayer layer in layerList)
            {
                layer.Update(screen, time, min, max, tileTypes);
            }
        }

        public void Draw(Screen screen, SpriteBatch batch)
        {
            Point min = MapGlobals.ConvertPositionToCell(screen.ViewPort.TargetLocation);
            Point max = MapGlobals.ConvertPositionToCell(
                screen.ViewPort.TargetLocation + new Vector2(
                screen.ViewPort.Size.X + MapGlobals.TileWidth,
                screen.ViewPort.Size.Y + MapGlobals.TileHeight));

            foreach (TileLayer layer in layerList)
                layer.Draw(screen, batch, min, max, tileTypes);
        }
    }
}
