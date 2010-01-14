using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class Radar
    {
        public Texture2D Line;
        public Texture2D Backing;
        public Texture2D HalfWay;
        public Vector2 Position = new Vector2(0, 589);

        List<Texture2D> ships = new List<Texture2D>();
        List<Vector2> shipPositions = new List<Vector2>();

        public Texture2D player;
        Vector2 playerPosition = Vector2.Zero;

        public int Height;
        public int Width;

        public float rotation = 0f;
        public float scale = 1f;
        public SpriteEffects SE = SpriteEffects.None;

        public bool Visible = true;

        public Radar()
        { 

        }
        public void AddShips(List<NpcPlane> npcs, PlayerPlane play, GraphicsDevice graphics)
        {
            if (npcs == null && player == null)
                return;

            if (npcs != null)
            {
                foreach (NpcPlane n in npcs)
                {
                    ships.Add(Line);
                    shipPositions.Add(new Vector2(n.Position.X / 2, (graphics.Viewport.Height / n.Position.Y) + Position.Y));
                }
            }
            if (player != null)
            {
                playerPosition = new Vector2(play.Position.X / 2, (play.Position.Y / 32) + Position.Y);
            }
        }

        public void Update(List<NpcPlane> npcs, PlayerPlane play, GraphicsDevice graphics, int CamPosX)
        {
            Position.X = CamPosX;
            for(int i = 0; i < npcs.Count; i++)            
            {
                shipPositions[i] = new Vector2(((int)npcs[i].Position.X + CamPosX) / 2, (npcs[i].Position.Y / 32) + Position.Y);
            }

            playerPosition = new Vector2(((int)play.Position.X + CamPosX) / 2, (play.Position.Y / 32) + Position.Y);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            spriteBatch.Draw(
                Backing,
                Position,
                Color.White);

            for(int i = 0; i < ships.Count; i++)
            {
                spriteBatch.Draw(
                    ships[i],
                    shipPositions[i],
                    new Rectangle(0, 0, ships[i].Width, ships[i].Height),
                    Color.White,
                    rotation,
                    Vector2.Zero,
                    scale,
                    SE,
                    0f);
            }

            spriteBatch.Draw(
                player,
                playerPosition,
                new Rectangle(0, 0, player.Width, player.Height),
                Color.White,
                rotation,
                Vector2.Zero,
                scale,
                SE,
                0f);

            spriteBatch.Draw(
                HalfWay,
                new Vector2(Position.X + 400, 589),
                new Rectangle(0, 0, 1, 19),
                Color.Green,
                rotation,
                Vector2.Zero,
                scale,
                SE,
                0f);
        }
    }
}
