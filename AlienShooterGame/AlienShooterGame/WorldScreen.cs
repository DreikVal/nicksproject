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
        protected LightSource _RedLight, _GreenLight;

        public const int TileCols = 50;
        public const int TileRows = 50;
        protected TileMap tileMap;

        public WorldScreen(ScreenManager manager)
            : base(manager, "World")
        {
            // Create player
            _Player = new Marine(this);
            _Player.Geometry.Position.X = 500;
            _Player.Geometry.Position.Y = 500;

            // Create crosshair
            _Crosshair = new Crosshair(this);

            // Create ambient lights
            _RedLight = new LightSource(this, new Color(255,120,120), 400f, (float)Math.PI*2, 0.0f, new Vector2(400f,300f));
            _GreenLight = new LightSource(this, new Color(100,255,100), 1200f, 1.2f, 0.0f, new Vector2(600f, 700f));

            // Setup tiles
            tileMap = new TileMap(this, TileCols, TileRows, 1);

            // Setup screen behavior
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = "Press F to toggle flashlight";
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
            else if (bind.Name.CompareTo("FlashLight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.FlashLight.Active = !_Player.FlashLight.Active;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _ViewPort.TargetLocation.X = _Player.Geometry.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = _Player.Geometry.Position.Y - (_ViewPort.Size.Y / 2);
            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);

            if (time.TotalGameTime.Milliseconds % 37 == 0)
                _RedLight.Active = !_RedLight.Active;
            _GreenLight.Direction += 0.02;
            if (_GreenLight.Direction > 3 * Math.PI / 2) _GreenLight.Direction -= 2 * Math.PI;
        }        
    }
}
