using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FriendlyEngine
{
    public static class Engine
    {
        public const int TileWidth = 24;
        public const int TileHeight = 24;

        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight, 
                TileWidth, 
                TileHeight);
        }

        public static Vector2 QuadBezierCurve(Vector2 pos1,
                                Vector2 pos2,
                                Vector2 pos3,
                                float t)
        {
            return (1 - t) * (1 - t) * pos1 + 2 * t * (1 - t) * pos2 + t * t * pos3;
        }
    }
}
