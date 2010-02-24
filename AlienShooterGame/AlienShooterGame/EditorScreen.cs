using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class EditorScreen : Screen
    {
        protected Crosshair _Crosshair;

        public int TileCols = 125;
        public int TileRows = 125;

        public int _TileIndex = 0;
        public int _SecondaryIndex = 1;

        public const float ScreenMoveRate = 75.0f;

        protected bool _Dragging = false;
        protected bool _SecondaryDragging = false;
        protected int lastRow, lastCol;

        protected Tile[,] _Tiles;

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        protected int row = 0;
        protected int col = 0;

        protected BackgroundWorker worker = new BackgroundWorker();

        public EditorScreen(ScreenManager manager)
            : base(manager, "Editor")
        {
            // Create crosshair
            _Crosshair = new Crosshair(this);

            Application.AppReference.DynamicLighting = false;
            Tile.TileWidth = 8.0f;
            Tile.TileHeight = 8.0f;

            // Setup tiles
            _Tiles = new Tile[TileRows, TileCols];
            for (row = 0; row < TileRows; row++)
            {
                for (col = 0; col < TileCols; col++)
                {
                    _Tiles[row, col] = Tile.TileGen[0](this, row, col, 0);
                }
            }

            // Setup screen behavior
            Depth = 0.7f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = "Left Click/Right Click, Press (F) to Close Browser";
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Font");
            _MessageColour = Color.White;
            _BackgroundDrawingOn = true;

            LoadPort = new LoadPort(this, new Vector2(), new Vector2(1050, 750), 100f);

            _ViewPort.TargetLocation = new Vector2(TileCols * Tile.TileWidth, TileRows * Tile.TileHeight);
            
        }

        protected override void HandleInputActive(Bind bind)
        {
            //base.HandleInputActive(bind);

            if (bind.Name.CompareTo("PrimaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _SecondaryDragging = false;
                    _Dragging = true;
                }
                else _Dragging = false;
            }
            else if (bind.Name.CompareTo("SecondaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _Dragging = false;
                    _SecondaryDragging = true;
                }
                else _SecondaryDragging = false;
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
            else if (bind.Name.CompareTo("Save") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _Message = "Saved map to world.awo";
                    worker.DoWork += SaveMap;
                    worker.RunWorkerAsync();
                }
            }
            else if (bind.Name.CompareTo("back") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    Screen gui;
                    _Manager.LookupScreen("GUIEditor", out gui);
                    gui.Remove();
                    this.Remove();
                    _Manager.AddScreen(new WorldScreen(_Manager, "world.awo"));
                    _Manager.AddScreen(new GUIScreen(_Manager));
                }
            }
            else if (bind.Name.CompareTo("Reload") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _ViewPort.TargetLocation = new Vector2(TileCols * Tile.TileWidth / 2, TileRows * Tile.TileHeight / 2);
                    _ViewPort.Size = new Vector2(800, 500);
                }
            }
            else if (bind.Name.CompareTo("ZoomIn") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _ViewPort.Size -= new Vector2(_Manager.Resolution.X / 20, _Manager.Resolution.Y / 20);
                }
            }
            else if (bind.Name.CompareTo("ZoomOut") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _ViewPort.Size += new Vector2(_Manager.Resolution.X / 20, _Manager.Resolution.Y / 20);
                }
            }
        }

        protected void SaveMap(object source, DoWorkEventArgs e)
        {
            FileStream fs = File.OpenWrite("world.awo");
            BinaryWriter bin = new BinaryWriter(fs);

            bin.Write(TileRows);
            bin.Write(TileCols);
            bin.Write(Tile.TileWidth);
            bin.Write(Tile.TileHeight);
            
            for (int row = 0; row < TileRows; row++)
            {
                for (int col = 0; col < TileCols; col++)
                {
                    bin.Write(_Tiles[row, col].TileIndex);
                }
            }

            bin.Flush();
            bin.Close();
            fs.Close();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (_Dragging || _SecondaryDragging)
            {
                col = (int)((_Crosshair.Geometry.Position.X ) / Tile.TileWidth);
                row = (int)((_Crosshair.Geometry.Position.Y ) / Tile.TileHeight);
                if (col != lastCol || row != lastRow)
                {
                    try
                    {
                        _Tiles[row, col].Dispose();
                        if (_SecondaryDragging)
                            _Tiles[row, col] = Tile.TileGen[_SecondaryIndex](this, row, col, _SecondaryIndex);
                        else
                            _Tiles[row, col] = Tile.TileGen[_TileIndex](this, row, col, _TileIndex);
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
