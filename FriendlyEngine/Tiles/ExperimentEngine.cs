using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public class ExperimentEngine
    {
        public int TileWidth = 24;
        public int TileHeight = 24;

        public ExperimentEngine()
        {
            TileWidth = 128;
            TileHeight = 128;
        }

        public Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        public Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight, 
                TileWidth, 
                TileHeight);
        }
    }
}
