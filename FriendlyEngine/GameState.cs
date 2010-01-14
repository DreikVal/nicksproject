using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FriendlyEngine
{
    public abstract class GameState
    {
        public int Selected;

        public GameState(Game game)
        {
            this.game = game;
            manager = game.Services.GetService(typeof(GameStateManager)) as GameStateManager;
            content = game.Services.GetService(typeof(ContentManager)) as ContentManager;
            graphics = game.Services.GetService(typeof(IGraphicsDeviceService)) as GraphicsDeviceManager;

            Selected = -1;
        }

        GameStateManager manager;
        ContentManager content;
        public GraphicsDeviceManager graphics;
        Game game;

        public Game Game
        {
            get { return game; }
        }

        public GameStateManager Manager
        {
            get { return manager; }
        }



        public ContentManager Content
        {
            get { return content; }
        }



        public GraphicsDevice GraphicsDevice
        {
            get { return graphics.GraphicsDevice; }
        }



        public GraphicsDeviceManager GraphicsManager
        {
            get { return graphics; }
        }

        //public abstract void Initialize();
        //public abstract void LoadContent();
        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
