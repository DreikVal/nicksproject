using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    class TileMap
    {
        List<Layer> layerList = new List<Layer>();

        public TileMap(Screen screen, int width, int height, int LayerCount)
        {
            if (LayerCount <= 0)
                LayerCount = 1;

            float alphaModifier = 1f / LayerCount;
            for (int i = 0; i < LayerCount; i++)
            {
                layerList.Add(new Layer(screen, width, height, alphaModifier));
                alphaModifier += (float)i / 10;
                MathHelper.Clamp(alphaModifier, 0, 1f);
            }
        }
    }
}
