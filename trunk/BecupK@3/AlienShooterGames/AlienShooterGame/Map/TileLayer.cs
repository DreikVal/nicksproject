using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class TileLayer
    {
        TilePosition[,] grid;
        float alpha = 1f;

        public float Depth { get { return _Depth; } set { _Depth = value; } }
        protected float _Depth = 0.5f;

        public float Alpha { get { return alpha; } set { alpha = MathHelper.Clamp(value, 0f, 1f); } }
        public int Width { get { return grid.GetLength(1); } }
        public int Height { get { return grid.GetLength(0); } }

        public TileLayer(Screen screen, int width, int height, float alpha)
        {
            grid = new TilePosition[height, width];

            Alpha = alpha;
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    grid[y, x] = new TilePosition(rand.Next(2));
                }
        }

        public void Update(WorldScreen screen, GameTime time, Point min, Point max, List<TileType> tileType)
        {
               min.X = (int)Math.Max(min.X, 0);
                min.Y = (int)Math.Max(min.Y, 0);
                max.X = (int)Math.Min(max.X, Width);
                max.Y = (int)Math.Min(max.Y, Height);

                for (int x = min.X; x < max.X; x++)
                {
                    for (int y = min.Y; y < max.Y; y++)
                    {
                        int tileIndex = grid[y, x].Index;

                        if (tileIndex == -1)
                            continue;

                        grid[y, x].Update(time, screen);
                        grid[y, x].Position = new Vector2(x * MapGlobals.TileWidth,
                            y * MapGlobals.TileHeight);

                        TileType tile = tileType[tileIndex];

                        if (tile.Disposed != true)
                        {
                            tile.Update(time, screen);
                        }
                    }
                }
        }

        public void Draw(Screen screen, SpriteBatch batch, Point min, Point max, List<TileType> tileType)
        {
                min.X = (int)Math.Max(min.X, 0);
                min.Y = (int)Math.Max(min.Y, 0);
                max.X = (int)Math.Min(max.X, Width);
                max.Y = (int)Math.Min(max.Y, Height);

                for (int x = min.X; x < max.X; x++)
                {
                    for (int y = min.Y; y < max.Y; y++)
                    {
                        int tileIndex = grid[y, x].Index;

                        if (tileIndex == -1)
                            continue;

                        TilePosition tilePos = grid[y, x];
                        TileType tile = tileType[tileIndex];                        
                        Texture2D texture = tile.Animations.Current.Texture;

                        batch.Draw(
                            tile.Animations.Current.Texture,
                            new Rectangle(
                                x * MapGlobals.TileWidth,
                                y * MapGlobals.TileHeight,
                                MapGlobals.TileHeight,
                                MapGlobals.TileHeight),
                                tile.CurrentSource,
                                tilePos.ActualColour,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                this.Depth);
                    }
                }
        }

    }
}
