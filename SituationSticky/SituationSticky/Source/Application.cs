using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SituationSticky
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Application : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Gets a reference to the graphics manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsManager { get { return _Graphics; } }
        private GraphicsDeviceManager _Graphics;

        /// <summary>
        /// Gets the sprite batch for the game.
        /// </summary>
        public SpriteBatch Batch { get { return _Batch; } }
        private SpriteBatch _Batch;

        /// <summary>
        /// Gets the screen manager used for this game.
        /// </summary>
        public ScreenManager ScreenManager { get { return _ScreenManager; } }
        protected ScreenManager _ScreenManager;

        /// <summary>
        /// Gets a reference to the instance of the running Application.
        /// </summary>
        public static Application AppReference { get { return _AppReference; } }
        private static Application _AppReference = null;

        /// <summary>
        /// The colour with which the backbuffer is cleared each time a frame is rendered.
        /// </summary>
        public Color BackgroundColour { get { return _BackgroundColour; } set { _BackgroundColour = value; } }
        protected Color _BackgroundColour = Color.Black;

        /// <summary>
        /// Gets a reference to the gamer services for the application.
        /// </summary>
        public GamerServicesComponent GamerService { get { return _GamerServices; } }
        protected GamerServicesComponent _GamerServices = null;

        /// <summary>
        /// Gets a dictionary that contains every entity controlled by the application.
        /// </summary>
        public ThreadDictionary<UInt64, Entity> AllEntities { get { return _AllEntities; } }
        protected ThreadDictionary<UInt64, Entity> _AllEntities = new ThreadDictionary<ulong, Entity>();

        /// <summary>
        /// Gets the applications random number generator.
        /// </summary>
        public Random Random { get { return _Random; } }
        protected Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Gets or sets the quality of graphics settings for the game.
        /// </summary>
        public int GfxLevel { get { return _GfxLevel; } set { _GfxLevel = value; } }
        protected int _GfxLevel = 4;

        /// <summary>
        /// Gets a library that contains a function that creates an entity based on its entity class name.
        /// </summary>
        public static Func<Screen, Vector3, Entity>[] _EntityDefinitions = 
        { 
            Tile.Tile_Floor01,
            Tile.Tile_Floor02,
            Tile.Tile_Floor03,
            Tile.Tile_Floor04,
            Tile.Tile_Floor05,
            Tile.Tile_Floor06,
            Tile.Tile_Wall01,
            Tile.Tile_Wall02,
            Tile.Tile_Wall03,
            Tile.Tile_Wall04,
            Tile.Tile_Wall05,
            Tile.Tile_Wall06,
            Marine.CreateMarine,
            Drone.CreateDrone
        };


        /// <summary>
        /// Creates a new Game Application.
        /// </summary>
        public Application()
        {
            // Only allow one instance of a game per process.
            if (_AppReference != null)
                throw new Exception("Multiple application instances created in single process!");
            _AppReference = this;
            
            // Initialize local components
            _Graphics = new GraphicsDeviceManager(this);
            _ScreenManager = new ScreenManager();

            // Set application options
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args) { using (Application app = new Application()) { app.Run(); } }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Load game binds
            _ScreenManager.Input.AddBind(new Bind("FWD", Keys.W));
            _ScreenManager.Input.AddBind(new Bind("BAC", Keys.S));
            _ScreenManager.Input.AddBind(new Bind("LFT", Keys.A));
            _ScreenManager.Input.AddBind(new Bind("RHT", Keys.D));
            _ScreenManager.Input.AddBind(new Bind("FLA", Keys.F));
            _ScreenManager.Input.AddBind(new Bind("MOV", Keys.M));
            _ScreenManager.Input.AddBind(new Bind("PRI", MouseButtons.LeftButton));
            _ScreenManager.Input.AddBind(new Bind("SEC", MouseButtons.RightButton));
            _ScreenManager.Input.AddBind(new Bind("ZIN", MouseButtons.ScrollUp));
            _ScreenManager.Input.AddBind(new Bind("ZOU", MouseButtons.ScrollDown));
            _ScreenManager.Input.AddBind(new Bind("RLD", Keys.R));
            _ScreenManager.Input.AddBind(new Bind("NVI", Keys.N));
            _ScreenManager.Input.AddBind(new Bind("EDI", Keys.F12));
            _ScreenManager.Input.AddBind(new Bind("FPS", Keys.F9));
            _ScreenManager.Input.AddBind(new Bind("SAV", Keys.F11));
            _ScreenManager.Input.AddBind(new Bind("WP1", Keys.D1));
            _ScreenManager.Input.AddBind(new Bind("WP2", Keys.D2));
            _ScreenManager.Input.AddBind(new Bind("WP3", Keys.D3));
            _ScreenManager.Input.AddBind(new Bind("WP4", Keys.D4));
            _ScreenManager.Input.AddBind(new Bind("WP5", Keys.D5));
            _ScreenManager.Input.AddBind(new Bind("ESC", Keys.Escape));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            _Batch = new SpriteBatch(GraphicsDevice);

            // Add game screens
            _ScreenManager.AddScreen(new WorldScreen(_ScreenManager, "Content/Maps/world.awo"));
            _ScreenManager.AddScreen(new WorldGUI(_ScreenManager));     
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, playing audio and updates the network.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime time)
        {
            base.Update(time);

            // Update the screen manager
            _ScreenManager.Update(time);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            // Clear the screen
            GraphicsDevice.Clear(_BackgroundColour);

            // Draw the screens
            _ScreenManager.Draw(time, _Batch);
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            MediaPlayer.Stop();
        }
    }
}
