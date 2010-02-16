using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Layer
    {
        Tile[,] grid;
        float alpha = 1f;

        public float Alpha { get { return alpha; } set { alpha = MathHelper.Clamp(value, 0f, 1f); } }
        public int Width { get { return grid.GetLength(1); } }
        public int Height { get { return grid.GetLength(0); } }

        public Layer(Screen screen, int width, int height)
        {
            grid = new Tile[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid[y, x] = new Tile(screen, x, y);
        }

        public Layer(Screen screen, int width, int height, float alpha)
        {
            grid = new Tile[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    grid[y, x] = new Tile(screen, x, y);
                    grid[y, x].ColourOverlay = new Color(1f, 1f, 1f, alpha);
                }
        }

    }
}
