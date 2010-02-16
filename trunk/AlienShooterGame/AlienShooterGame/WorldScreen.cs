using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class WorldScreen : Screen
    {
        public Marine Player { get { return _Player; } }
        protected Marine _Player;

        protected Crosshair _Crosshair;

        public const int TileCols = 50;
        public const int TileRows = 50;
        protected TileMap tileMap;

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
            tileMap = new TileMap(this, TileCols, TileRows, 1);

            // Setup screen behavior
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = "Press M to toggle absolute/relative movement";
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Font");
        }

        protected override void HandleInputActive(Bind bind)
        {
            base.HandleInputActive(bind);

            if (bind.Name.CompareTo("MoveForward") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveForward = true;
                else
                    _Player.MoveForward = false;
            }
            else if (bind.Name.CompareTo("MoveBack") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveBack = true;
                else
                    _Player.MoveBack = false;
            }
            else if (bind.Name.CompareTo("StrafeLeft") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveLeft = true;
                else
                    _Player.MoveLeft = false;
            }
            else if (bind.Name.CompareTo("StrafeRight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveRight = true;
                else
                    _Player.MoveRight = false;
            }
            else if (bind.Name.CompareTo("MoveMode") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Manager.Input.AbsoluteMovement = !_Manager.Input.AbsoluteMovement;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _ViewPort.TargetLocation.X = _Player.Geometry.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = _Player.Geometry.Position.Y - (_ViewPort.Size.Y / 2);
            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);
        }
    }
}
