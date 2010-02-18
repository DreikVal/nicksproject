using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class EditorScreen : Screen
    {
        protected Crosshair _Crosshair;
        protected Editor_Gui _EditorGUI;

        public const int TileCols = 120;
        public const int TileRows = 120;

        protected int _TileIndex = 0;

        public const float ScreenMoveRate = 50.0f;

        protected bool _Dragging = false;
        protected int lastRow, lastCol;

        protected Tile PreviewTile;
        protected Tile PreviewTileB;

        protected Tile[,] _Tiles;

        protected int row = 0;
        protected int col = 0;

        public EditorScreen(ScreenManager manager)
            : base(manager, "Editor")
        {
            // Create crosshair
            _Crosshair = new Crosshair(this);

            _EditorGUI = new Editor_Gui(this);
            _EditorGUI.Geometry.Position = new Vector2(764, 407);
            PreviewTile = Tile.TileGen[_TileIndex](this, row, col);
            PreviewTile.Geometry.Position = new Vector2(734, 256);
            PreviewTile.Depth = 0.16f;
            PreviewTileB = Tile.TileGen[(_TileIndex + 1) % Tile.TileGen.Length](this, row, col);
            PreviewTileB.Geometry.Position = new Vector2(734, 256);
            PreviewTileB.Depth = 0.16f;


            Application.AppReference.DynamicLighting = false;

            // Setup tiles
            _Tiles = new Tile[TileRows, TileCols];
            for (row = 0; row < TileRows; row++)
            {
                for (col = 0; col < TileCols; col++)
                {
                    _Tiles[row, col] = new Tile(this, "bar_tile", false, row, col);
                }
            }

            // Setup screen behavior
            Depth = 0.7f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = "World Editor Mode";
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Font");
            _MessageColour = Color.Red;
        }

        protected override void HandleInputActive(Bind bind)
        {
            base.HandleInputActive(bind);

            if (bind.Name.CompareTo("PrimaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down) _Dragging = true;
                else _Dragging = false;
            }
            else if (bind.Name.CompareTo("SecondaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _TileIndex = (_TileIndex + 1) % Tile.TileGen.Length;
                    PreviewTile.Dispose();
                    PreviewTile = Tile.TileGen[_TileIndex](this, row, col);
                    PreviewTile.Depth = 0.16f;
                    PreviewTileB.Dispose();
                    PreviewTileB = Tile.TileGen[(_TileIndex + 1) % Tile.TileGen.Length](this, row, col);
                    PreviewTileB.Depth = 0.18f;
                }
            }
            else if (bind.Name.CompareTo("StrafeLeft") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.X -= ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("StrafeRight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.X += ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("MoveForward") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.Y -= ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("MoveBack") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.Y += ScreenMoveRate;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _EditorGUI.Geometry.Position = new Vector2(_ViewPort.ActualLocation.X + 764 ,_ViewPort.ActualLocation.Y + 407);
            PreviewTile.Geometry.Position = new Vector2(_ViewPort.ActualLocation.X + 754, _ViewPort.ActualLocation.Y + 397);
            PreviewTileB.Geometry.Position = new Vector2(_ViewPort.ActualLocation.X + 774, _ViewPort.ActualLocation.Y + 417);

            if (_Dragging)
            {
                col = (int)((_Crosshair.Geometry.Position.X + _Crosshair.Geometry.Radius / 2) / Tile.TileWidth);
                row = (int)((_Crosshair.Geometry.Position.Y + _Crosshair.Geometry.Radius / 2) / Tile.TileHeight);
                if (col != lastCol || row != lastRow)
                {
                    try
                    {
                        _Tiles[row, col].Dispose();
                        _Tiles[row, col] = Tile.TileGen[_TileIndex](this, row, col);
                        lastRow = row;
                        lastCol = col;
                    }
                    catch (Exception) { }
                }
            }

            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);
        }
    }
}
