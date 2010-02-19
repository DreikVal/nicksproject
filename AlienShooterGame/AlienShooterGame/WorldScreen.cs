using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace AlienShooterGame
{
    class WorldScreen : Screen
    {
        public Marine Player { get { return _Player; } }
        protected Marine _Player;

        protected Crosshair _Crosshair;

        public int TileCols;
        public int TileRows;
        public int NumAliens = 20;

        protected int _Frames = 60;
        protected int _NextFPSUpdate = 1000;
        protected bool _FPSDisplay = false;

        protected bool isFiring = false;
        protected double shotCooldown = 0.0;
        protected double weaponCooldown = 200;

        protected String _WorldMap;

        protected String _HelpMessage = "New Features:  (F)lashlight, (R)eload, (N)ightVision, (F9)FPS, (F12) WorldEditor";

        public WorldScreen(ScreenManager manager, String worldMap)
            : base(manager, "World")
        {
            // Load world
            _WorldMap = worldMap;
            FileStream fs = File.OpenRead(_WorldMap);
            BinaryReader bin = new BinaryReader(fs);

            TileRows = bin.ReadInt32();
            TileCols = bin.ReadInt32();
            Tile.TileWidth = bin.ReadSingle();
            Tile.TileHeight = bin.ReadSingle();

            for (int row = 0; row < TileRows; row++)
            {
                for (int col = 0; col < TileCols; col++)
                {
                    int index = bin.ReadInt32();
                    Tile.TileGen[index](this, row, col, index);
                }
            }

            bin.Close();
            fs.Close();

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


            // Setup screen behavior
            Depth = 0.9f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _Message = _HelpMessage;
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Font");
            _MessageColour = Color.White;
            Application.AppReference.DynamicLighting = false;

            // Create loadport
            LoadPort = new LoadPort(this, new Vector2(), new Vector2(1600, 1000), 240f);

            //music time
            Random random = new Random();
            int song = random.Next(0, 2);
            if (song == 0)
                MediaPlayer.Play(Application.AppReference.Content.Load<Song>("Sounds\\03 - Teardrop"));
            if (song == 1)
                MediaPlayer.Play(Application.AppReference.Content.Load<Song>("Sounds\\leanonme"));
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
                    isFiring = true;
                else
                    isFiring = false;
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
                {
                    Screen gui;
                    _Manager.LookupScreen("GUI", out gui);
                    gui.Remove();
                    Remove();
                    _Manager.AddScreen(new EditorScreen(_Manager));
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            shotCooldown += time.ElapsedGameTime.Milliseconds;
            if (shotCooldown > weaponCooldown && isFiring)
            {
                shotCooldown = 0.0;
                Player.Fire();
            }

            base.Update(time);

            _ViewPort.TargetLocation.X = _Player.Geometry.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = _Player.Geometry.Position.Y - (_ViewPort.Size.Y / 2);
            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);

            //_Message = "Active: " + _Entities.Count + "  Inactive: " + _InactiveEntities.Count;
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
