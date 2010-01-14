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

    public enum PGGameState
    {
        Title,
        Options,
        InGame,
    }

    public class GameStateManager : DrawableGameComponent
    {
        public Dictionary<PGGameState, GameState> GameStates = new Dictionary<PGGameState, GameState>();
        public PGGameState CurrentState = PGGameState.Title;

        public GameStateManager(Game game)
            : base(game)
        {

        }



        public override void Update(GameTime gameTime)
        {
            GameState state;
            if (GameStates.TryGetValue(CurrentState, out state))
                state.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            GameState state;
            if (GameStates.TryGetValue(CurrentState, out state))
                state.Draw(gameTime);
        }

    }
}
