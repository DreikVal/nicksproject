using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SituationSticky
{
    class WorldScreen : Screen
    {
        #region Constants

        public const int    NumDrones           = 15;
        public const String DefaultHelpMessage  = "Situation: Sticky (Demo)";

        #endregion

        #region Members

        /// <summary>
        /// Gets the player's crosshair.
        /// </summary>
        public Crosshair Crosshair { get { return _Crosshair; } }
        protected Crosshair _Crosshair;

        /// <summary>
        /// Gets a reference to the player's Marine.
        /// </summary>
        public Marine PlayerMarine { get { return _PlayerEntity as Marine; } set { _PlayerEntity = value; } }

        /// <summary>
        /// Gets the number of columns in the tile grid.
        /// </summary>
        public int TileCols { get { return _TileCols; } }
        protected int _TileCols;

        /// <summary>
        /// Gets the number of rows in the tile grid.
        /// </summary>
        public int TileRows { get { return _TileRows; } }
        protected int _TileRows;

        /// <summary>
        /// FPS logging / display variables
        /// </summary>
        protected int _Frames = 60;
        protected int _NextFPSUpdate = 1000;
        protected bool _FPSDisplay = false;

        /// <summary>
        /// Gets the path to the current world map.
        /// </summary>
        public String WorldMap { get { return _WorldMap; } }
        protected String _WorldMap;

        /// <summary>
        /// Gets or sets the help message for the screen.
        /// </summary>
        public String HelpMessage { get { return _HelpMessage; } set { _HelpMessage = value; } }
        protected String _HelpMessage = DefaultHelpMessage;

        /// <summary>
        /// Gets whether or not the Marine is firing his weapon.
        /// </summary>
        public bool IsFiring { get { return _IsFiring; } }
        protected bool _IsFiring = false;

        #endregion

        #region Init and Disposal

        public WorldScreen(ScreenManager manager, String worldMap)
            : base(manager, "World")
        {
            // Load world
            _WorldMap = worldMap;
            FileStream fs = File.OpenRead(_WorldMap);
            BinaryReader bin = new BinaryReader(fs);
            _TileRows = bin.ReadInt32();
            _TileCols = bin.ReadInt32();
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
            PlayerMarine = new Marine(this, new Vector2(TileCols*Tile.TileWidth / 2, TileRows*Tile.TileHeight / 2));

            // Create crosshair
            _Crosshair = new Crosshair(this);

            // Create aliens
            for (int i = 0; i < NumDrones; i++)
            {
                float dist = (float)Application.AppReference.Random.NextDouble() * 300 + 200f;
                Drone.CreateNearbyDrone(this, PlayerMarine, dist, PlayerMarine);
            }


            // Setup screen behavior
            _Depth = 0.9f;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _MessageFont = Application.AppReference.Content.Load<SpriteFont>("Fonts/DefaultFont");
            _MessageColour = Color.White;
            _DynamicLighting = true;

            // Create loadport
            LoadPort = new LoadPort(this, new Vector2(), new Vector2(1050, 750), 100f);

            // Play music
            MediaPlayer.Play(Application.AppReference.Content.Load<Song>("Audio/Music/GameMusic01"));

            // Start dynamic lighting thread
            StartBackgroundThread();
        }

        #endregion

        #region Update

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            // Lock viewport to Marine's position
            _ViewPort.TargetLocation.X = PlayerMarine.Position.X - (_ViewPort.Size.X / 2);
            _ViewPort.TargetLocation.Y = PlayerMarine.Position.Y - (_ViewPort.Size.Y / 2);

            // Lock message location to viewport location
            _MessageLocation = new Vector2(_ViewPort.ActualLocation.X + 20.0f, _ViewPort.ActualLocation.Y + 20.0f);

            // Display help message
            if (!_FPSDisplay && _PlayerEntity != null && !_PlayerEntity.Disposed)
                _Message = DefaultHelpMessage;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);

            // Calculate frames per second
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

        #endregion

        #region Utility

        /// <summary>
        /// This method handles the player input for this world.
        /// </summary>
        /// <param name="bind">The bind who's state has changed.</param>
        protected override void HandleInputActive(Bind bind)
        {
            base.HandleInputActive(bind);

            // Process move forward bind
            if (bind.Name.CompareTo("FWD") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.MoveForward = true;
                else
                    PlayerMarine.MoveForward = false;
            }
            // Process move back bind
            else if (bind.Name.CompareTo("BAC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.MoveBack = true;
                else
                    PlayerMarine.MoveBack = false;
            }
            // Process move left bind
            else if (bind.Name.CompareTo("LFT") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.MoveLeft = true;
                else
                    PlayerMarine.MoveLeft = false;
            }
            // Process move right bind
            else if (bind.Name.CompareTo("RHT") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.MoveRight = true;
                else
                    PlayerMarine.MoveRight = false;
            }
            // Switch movement style (relative/absolute)
            else if (bind.Name.CompareTo("MOV") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Manager.Input.AbsoluteMovement = !_Manager.Input.AbsoluteMovement;
            }
            // Switch on/off flashlight
            else if (bind.Name.CompareTo("FLI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.FlashLight.Active = !PlayerMarine.FlashLight.Active;
            }
            // Primary fire
            else if (bind.Name.CompareTo("PRI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.CurrentWeapon.IsFiring = true;
                else
                    PlayerMarine.CurrentWeapon.IsFiring = false;
            }
            // Secondary fire
            else if (bind.Name.CompareTo("SEC") == 0) 
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (PlayerMarine.Disposed)
                        PlayerMarine = new Marine(this, new Vector2(TileCols * Tile.TileWidth / 2, TileRows * Tile.TileHeight / 2));
                }
            }
            // Reload
            else if (bind.Name.CompareTo("RLD") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.CurrentWeapon.Reload();
            }
            // Night vision on/off
            else if (bind.Name.CompareTo("NVI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    PlayerMarine.NightVision.Active = !PlayerMarine.NightVision.Active;
            }
            // Toggle fps display
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
            // Load world editor
            else if (bind.Name.CompareTo("EDI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    //Screen gui;
                    //_Manager.LookupScreen("GUI", out gui);
                    //gui.Remove();
                    Remove();
                    //_Manager.AddScreen(new EditorScreen(_Manager));
                    //_Manager.AddScreen(new GUIEditor(_Manager));
                }
            }
            // Exit game
            else if (bind.Name.CompareTo("ESC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Up)
                    Application.AppReference.Exit();
            }
            /*
            else if (bind.Name.CompareTo("WP1") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (Player.weaponList.Count > 0)
                    {
                        Player.currentWeapon = Player.weaponList[0];
                    }
                }
            }
            else if (bind.Name.CompareTo("WP2") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (Player.weaponList.Count > 1)
                    {
                        Player.currentWeapon = Player.weaponList[1];
                    }
                }
            }
            else if (bind.Name.CompareTo("WP3") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (Player.weaponList.Count > 2)
                    {
                        Player.currentWeapon = Player.weaponList[2];
                    }
                }
            }
            else if (bind.Name.CompareTo("WP4") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (Player.weaponList.Count > 3)
                    {
                        Player.currentWeapon = Player.weaponList[3];
                    }
                }
            }
            else if (bind.Name.CompareTo("WP5") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (Player.weaponList.Count > 4)
                    {
                        Player.currentWeapon = Player.weaponList[4];
                    }
                }
            }*/
        }

        #endregion
    }
}
