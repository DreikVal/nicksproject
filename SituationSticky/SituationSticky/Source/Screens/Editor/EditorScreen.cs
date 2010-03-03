using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    class EditorScreen : Screen
    {
        protected Crosshair _Crosshair;

        public int TileCols = 50;
        public int TileRows = 50;

        public int TileIndex { get { return _TileIndex; } set { _TileIndex = value; } }
        protected int _TileIndex = 0;

        public int SecondaryIndex { get { return _SecondaryIndex; } set { _SecondaryIndex = value; } }
        protected int _SecondaryIndex = 1;

        public const float ScreenMoveRate = 75.0f;

        protected bool _Dragging = false;
        protected bool _SecondaryDragging = false;
        protected int lastRow, lastCol;

        protected Tile[,] _TileGrid;

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

            Tile.TileWidth = 24.0f;
            Tile.TileHeight = 24.0f;

            // Setup tiles
            _TileGrid = new Tile[TileRows, TileCols];
            for (row = 0; row < TileRows; row++)
            {
                for (col = 0; col < TileCols; col++)
                    _TileGrid[row, col] = Tile.TileGen[0](this, new Vector3(col * Tile.TileWidth, row * Tile.TileHeight, 0));
            }

            // Setup screen behavior
            _Depth = 0.7f;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = "LeftClick: Place Primary Tile\nRightClick: Place Secondary Tile\nMouseScroll: Zoom, W S A D: Move\nF: Toggle Texture Browser";
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Fonts/DefaultFont");
            _MessageColour = Color.White;
            _DynamicLighting = false;
            _Crosshair.Hide = true;

            // Setup ports
            _ViewPort.TargetLocation = new Vector3(TileCols * Tile.TileWidth, TileRows * Tile.TileHeight, 0);
            _LoadPort = new LoadPort(this, Vector2.Zero, _ViewPort.Size*1.20f, _ViewPort.Size.Y*0.1f);
        }

        protected override void HandleInputActive(Bind bind)
        {
            //base.HandleInputActive(bind);

            if (bind.Name.CompareTo("PRI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _SecondaryDragging = false;
                    _Dragging = true;
                }
                else _Dragging = false;
            }
            else if (bind.Name.CompareTo("SEC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _Dragging = false;
                    _SecondaryDragging = true;
                }
                else _SecondaryDragging = false;
            }
            else if (bind.Name.CompareTo("LFT") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.X -= ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("RHT") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.X += ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("FWD") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.Y -= ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("BAC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _ViewPort.TargetLocation.Y += ScreenMoveRate;
            }
            else if (bind.Name.CompareTo("SAV") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _Message = "Saved map to world.awo";
                    worker.DoWork += SaveMap;
                    worker.RunWorkerAsync();
                }
            }
            else if (bind.Name.CompareTo("ESC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    Remove();
                    _Manager.GetScreen("EditorGUI").Remove();

                    _Manager.AddScreen(new WorldScreen(_Manager, "Content/Maps/world.awo"));
                    _Manager.AddScreen(new WorldGUI(_Manager));
                }
            }
            else if (bind.Name.CompareTo("RLD") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _ViewPort.TargetLocation = new Vector3(TileCols * Tile.TileWidth / 2, TileRows * Tile.TileHeight / 2, 0);
                    _ViewPort.Size = new Vector2(800, 500);
                }
            }
            else if (bind.Name.CompareTo("ZIN") == 0)
            {
                _ViewPort.Size -= new Vector2(_Manager.Resolution.X / 20, _Manager.Resolution.Y / 20);
                //_ViewPort.TargetLocation = _Crosshair.Position - _ViewPort.Size / 2;
                _LoadPort.Size = _ViewPort.Size * 1.20f;
                //_LoadPort.LoadContent(null, null);
            }
            else if (bind.Name.CompareTo("ZOU") == 0)
            {
                _ViewPort.Size += new Vector2(_Manager.Resolution.X / 20, _Manager.Resolution.Y / 20);
                //_ViewPort.TargetLocation = _Crosshair.Position - _ViewPort.Size / 2;
                _LoadPort.Size = _ViewPort.Size * 1.20f;
                //_LoadPort.LoadContent(null, null);
            }
        }

        protected void SaveMap(object source, DoWorkEventArgs e)
        {
            FileStream fs = File.OpenWrite("Content/Maps/world.awo");
            BinaryWriter bin = new BinaryWriter(fs);

            bin.Write(TileRows);
            bin.Write(TileCols);
            bin.Write(Tile.TileWidth);
            bin.Write(Tile.TileHeight);
            
            for (int row = 0; row < TileRows; row++)
            {
                for (int col = 0; col < TileCols; col++)
                {
                    bin.Write(_TileGrid[row, col].EntityClass);
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
                EditorGUI gui = _Manager.GetScreen("EditorGUI") as EditorGUI;
                if (gui.TextureListShown) return;

                col = (int)((_Crosshair.Position.X + Tile.TileWidth / 2) / Tile.TileWidth );
                row = (int)((_Crosshair.Position.Y + Tile.TileHeight / 2) / Tile.TileHeight );
                if (col != lastCol || row != lastRow)
                {
                    try
                    {
                        _TileGrid[row, col].Dispose();
                        _TileGrid[row, col] = Tile.TileGen[_Dragging ? _TileIndex : _SecondaryIndex](this, new Vector3(col*Tile.TileWidth, row*Tile.TileHeight, 0));
                        lastRow = row;
                        lastCol = col;
                    }
                    catch (Exception) { }
                }
            }

            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);
        }
    }
}
