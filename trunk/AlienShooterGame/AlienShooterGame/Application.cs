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

namespace AlienShooterGame
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
        /// Gets a library that contains a function that creates an entity based on its entity class name.
        /// </summary>
        public Dictionary<String, Func<UInt64?, Screen, bool, Entity>> EntityDefinitions { get { return _EntityDefinitions; } }
        protected Dictionary<String, Func<UInt64?, Screen, bool, Entity>> _EntityDefinitions = new Dictionary<string, Func<UInt64?, Screen, bool, Entity>>();

        public AudioEngine AudioEngine { get { return _AudioEngine; } }
        protected AudioEngine _AudioEngine;

        public SoundBank SoundBank { get { return _SoundBank; } }
        protected SoundBank _SoundBank;

        public WaveBank WaveBank { get { return _WaveBank; } }
        protected WaveBank _WaveBank;

        public Random Random { get { return _Random; } }
        protected Random _Random = new Random(DateTime.Now.Millisecond);

        public bool DynamicLighting { get { return _DynamicLighting; } set { _DynamicLighting = value; } }
        public bool _DynamicLighting = true;

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
            Content.RootDirectory = "Content";
            //_GamerServices = new GamerServicesComponent(this);
            //Components.Add(_GamerServices);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.IsFixedTimeStep = false;
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

            // Setup audio system
            InitializeAudio();

            // Load game binds
            _ScreenManager.Input.AddBind(new Bind("MoveForward", Keys.W));
            _ScreenManager.Input.AddBind(new Bind("MoveBack", Keys.S));
            _ScreenManager.Input.AddBind(new Bind("StrafeLeft", Keys.A));
            _ScreenManager.Input.AddBind(new Bind("StrafeRight", Keys.D));
            _ScreenManager.Input.AddBind(new Bind("FlashLight", Keys.F));
            _ScreenManager.Input.AddBind(new Bind("MoveMode", Keys.M));
            _ScreenManager.Input.AddBind(new Bind("PrimaryFire", MouseButtons.LeftButton));
            _ScreenManager.Input.AddBind(new Bind("SecondaryFire", MouseButtons.RightButton));
            _ScreenManager.Input.AddBind(new Bind("Reload", Keys.R));
            _ScreenManager.Input.AddBind(new Bind("NightVision", Keys.N));
            _ScreenManager.Input.AddBind(new Bind("Editor", Keys.F12));
            _ScreenManager.Input.AddBind(new Bind("FPS", Keys.F9));
            _ScreenManager.Input.AddBind(new Bind("Save", Keys.F11));
            _ScreenManager.Input.AddBind(new Bind("Weapon1", Keys.D1));
            _ScreenManager.Input.AddBind(new Bind("Weapon2", Keys.D2));
            _ScreenManager.Input.AddBind(new Bind("Weapon3", Keys.D3));
            _ScreenManager.Input.AddBind(new Bind("Weapon4", Keys.D4));
            _ScreenManager.Input.AddBind(new Bind("Weapon5", Keys.D5));

            // Load entity definitions

            // Add game screens
            _ScreenManager.AddScreen(new WorldScreen(_ScreenManager, "world.awo"));
            _ScreenManager.AddScreen(new GUIScreen(_ScreenManager));
            
        }

        protected virtual void InitializeAudio()
        {
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

            // Update audio
            if (_AudioEngine != null && !_AudioEngine.IsDisposed)
                _AudioEngine.Update();
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
        /// Adds the assosciated network stream with the new gamer that joined the network session.
        /// </summary>
        /// <param name="sender">The network.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void GamerJoined(object sender, GamerJoinedEventArgs e)
        {
        }

        /// <summary>
        /// Removes the assosciated network stream of the new gamer that left the network session.
        /// </summary>
        /// <param name="sender">The network.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void GamerLeft(object sender, GamerLeftEventArgs e)
        {
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            MediaPlayer.Stop();
        }
    }
}
