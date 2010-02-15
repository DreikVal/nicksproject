using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    class WorldScreen : Screen
    {
        protected Marine _Player;
        protected Crosshair _Crosshair;

        public const int TileCols = 20;
        public const int TileRows = 15;
        protected Tile[,] _Tiles;

        public WorldScreen(ScreenManager manager)
            : base(manager, "World")
        {
            // Create player
            _Player = new Marine(this);
            _Player.Geometry.Position.X = 200;
            _Player.Geometry.Position.Y = 200;

            // Create crosshair
            _Crosshair = new Crosshair(this);

            // Setup tiles
            _Tiles = new Tile[TileRows, TileCols];
            for (int row = 0; row < TileRows; row++)
            {
                for (int col = 0; col < TileCols; col++)
                {
                    _Tiles[row, col] = new Tile(this, row, col);
                }
            }

            // Setup screen behavior
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
        }

        public override void HandleInput(Bind bind)
        {
            base.HandleInput(bind);

            if (bind.Name.CompareTo("MoveForward") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveForward = true;
                else
                    _Player.MoveForward = false;
            }
            if (bind.Name.CompareTo("MoveBack") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveBack = true;
                else
                    _Player.MoveBack = false;
            }
            if (bind.Name.CompareTo("StrafeLeft") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveLeft = true;
                else
                    _Player.MoveLeft = false;
            }
            if (bind.Name.CompareTo("StrafeRight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveRight = true;
                else
                    _Player.MoveRight = false;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _ViewPort.TargetLocation.X = _Player.Geometry.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = _Player.Geometry.Position.Y - (_ViewPort.Size.Y / 2);
        }
    }
}
