using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    class MapGlobals
    {
        public static int TileWidth = 24;
        public static int TileHeight = 24;

        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }
    }
}
