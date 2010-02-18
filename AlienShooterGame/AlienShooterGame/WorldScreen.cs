﻿using System;
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

        public const int TileCols = 120;
        public const int TileRows = 120;
        public const int NumAliens = 8;

        protected int _Frames = 60;
        protected int _NextFPSUpdate = 1000;
        protected bool _FPSDisplay = false;

        protected String _HelpMessage = "New Features:  (F)lashlight, (R)eload, (N)ightVision, (F9)FPS, (F12) WorldEditor";

        public WorldScreen(ScreenManager manager)
            : base(manager, "World")
        {
            // Create player
            _Player = new Marine(this);
            _Player.Geometry.Position.X = TileCols*Tile.TileWidth/2;
            _Player.Geometry.Position.Y = TileRows*Tile.TileHeight/2;

            // Create crosshair
            _Crosshair = new Crosshair(this);

            // Create aliens
            for (int i = 0; i < NumAliens; i++)
            {
                float dist = (float)Application.AppReference.Random.NextDouble() * 300 + 200f;
                Alien.CreateNearbyAlien(this, _Player, dist, _Player);
            }

            // Create ambient lights
            //_RedLight = new LightSource(this, new Color(255,150,150), 650f, (float)Math.PI*2, 0.0f, new Vector2(400f,300f));
            //_GreenLight = new LightSource(this, new Color(100,255,100), 1200f, 1.2f, 0.0f, new Vector2(600f, 700f));

            // Setup tiles
            for (int row = 0; row < TileRows; row++)
            {
                for (int col = 0; col < TileCols; col++)
                {
                    new Tile(this, "detail_tile", false, row, col);
                }
            }

            // Setup screen behavior
            Depth = 0.9f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = _HelpMessage;
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Font");
            _MessageColour = Color.White;
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
            else if (bind.Name.CompareTo("FlashLight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.FlashLight.Active = !_Player.FlashLight.Active;
            }
            else if (bind.Name.CompareTo("PrimaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.Fire();
            }
            else if (bind.Name.CompareTo("Reload") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.Reload();
            }
            else if (bind.Name.CompareTo("NightVision") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.NightVision.Active = !_Player.NightVision.Active;
            }
            else if (bind.Name.CompareTo("FPS") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (_FPSDisplay)
                    {
                        _FPSDisplay = false;
                        _Message = _HelpMessage;
                    }
                    else _FPSDisplay = true;
                }
            }
            else if (bind.Name.CompareTo("Editor") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Manager.AddScreen(new EditorScreen(_Manager));
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _ViewPort.TargetLocation.X = _Player.Geometry.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = _Player.Geometry.Position.Y - (_ViewPort.Size.Y / 2);
            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);
            /*
            if (time.TotalGameTime.Milliseconds % 37 == 0)
                _RedLight.Active = !_RedLight.Active;
            _GreenLight.Direction += 0.002 * time.ElapsedGameTime.Milliseconds;
            if (_GreenLight.Direction > 3 * Math.PI / 2) _GreenLight.Direction -= 2 * Math.PI;
            */
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);

            _Frames++;

            _NextFPSUpdate -= time.ElapsedRealTime.Milliseconds;
            if (_NextFPSUpdate < 0)
            {
                _NextFPSUpdate += 1000;
                if (_FPSDisplay)
                    _Message = "FPS: " + _Frames;
                _Frames = 0;
            }
        }
    }
}
