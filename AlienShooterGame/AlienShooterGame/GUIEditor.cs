using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class GUIEditor : Screen
    {
        Editor_Gui Editor_GUI;
        protected Tile PreviewTile;
        protected Tile PreviewTileB;
        protected int _TileIndex = 0;

        public GUIEditor(ScreenManager manager)
            : base(manager, "GUIEditor")
        {
            //_ViewPort.Size = new Vector2(800, 440);
            Editor_GUI = new Editor_Gui(this);

            PreviewTile = Tile.TileGen[_TileIndex](this, 1, 1, _TileIndex);
            PreviewTileB = Tile.TileGen[(_TileIndex + 1) % Tile.TileGen.Length](this, 1, 1, _TileIndex);
            PreviewTile.Geometry.Position = new Vector2(554, 397);
            PreviewTile.Depth = 0.17f;
            PreviewTileB.Geometry.Position = new Vector2(574, 417);
            PreviewTileB.Depth = 0.18f;

            Depth = 0.2f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            this.BlocksInput = false;
            this.BlocksUpdates = false;
            this.BlocksVisibility = false;
            this.Lights.Clear();
        }
        public override void HandleInput(Bind bind)
        {
            //base.HandleInput(bind);

            if (bind.Name.CompareTo("SecondaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                Screen screen;
                EditorScreen world;
                    _Manager.LookupScreen("Editor", out screen);
                    world = (EditorScreen)screen;
                    if (_TileIndex != world._TileIndex)
                    {
                        PreviewTile.Dispose();
                        PreviewTile = Tile.TileGen[world._TileIndex](this, world.Row, world.Col, world._TileIndex);
                        PreviewTile.Geometry.Position = new Vector2(554, 397);
                        PreviewTile.Depth = 0.17f;
                        PreviewTileB.Dispose();
                        PreviewTileB = Tile.TileGen[(world._TileIndex + 1) % Tile.TileGen.Length](this, world.Row, world.Col, world._TileIndex);
                        PreviewTileB.Geometry.Position = new Vector2(574, 417);
                        PreviewTileB.Depth = 0.18f;
                    }
                    _TileIndex = world._TileIndex;
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }
    }
}