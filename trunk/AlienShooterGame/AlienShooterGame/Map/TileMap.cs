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

        //public TileMap(Screen screen, int width, int height, int Count)
        //{
        //    for (int i = 0; i < Count; i++)
        //    {
        //        layerList.Add(new Layer(screen, width, height));
        //    }
        //}

        public TileMap(Screen screen, int width, int height, int Count)
        {
            float alphaModifier = 0.1f;
            for (int i = 0; i < Count; i++)
            {
                layerList.Add(new Layer(screen, width, height, alphaModifier));
                alphaModifier += (float)i / 10;
                MathHelper.Clamp(alphaModifier, 0, 1f);
            }
        }
    }
}
