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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //player
        Player player = new Player();

        //misc textures
        Texture2D crosshairTexture;
        Texture2D floorTexture;
        Texture2D bulletTexture;

        //states
        MouseState mState;
        double shotCooldown = 0.0;

        //bullet
        public class Bullet
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public int Power;

            public Bullet(Vector2 Position, Vector2 Velocity, int Power)
            {
                this.Position = Position;
                this.Velocity = Velocity;
                this.Power = Power;
            }

            public void Update()
            {
                Position += Velocity;
            }
        }

        List<Bullet> bulletList = new List<Bullet>();
        List<Object> dropList = new List<Object>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Texture = Content.Load<Texture2D>("soldier");
            floorTexture = Content.Load<Texture2D>("sand_tile");
            crosshairTexture = Content.Load<Texture2D>("crosshair");
            bulletTexture = Content.Load<Texture2D>("mg_bullet");
        }

        protected override void Update(GameTime gameTime)
        {
            //timer
            shotCooldown += gameTime.ElapsedGameTime.TotalMilliseconds;

            //controls
            mState = Mouse.GetState();
            player.Rotation = (float)Math.Atan2(mState.Y - player.Position.Y, mState.X - player.Position.X) + MathHelper.ToRadians(90);

            double shotCooldownDefault = 400;
            int shotSpeed = 5;
            if (mState.LeftButton == ButtonState.Pressed)
                if (shotCooldown >= shotCooldownDefault)
                {
                    shotCooldown = 0.0;
                    bulletList.Add(new Bullet(player.Position, Vector2.Normalize(new Vector2(mState.X - player.Position.X, mState.Y - player.Position.Y)) * shotSpeed, 1));
                }

            float speed = 2;

            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.W))
                player.Position.Y -= speed;
            if (kbState.IsKeyDown(Keys.A))
                player.Position.X -= speed;
            if (kbState.IsKeyDown(Keys.S))
                player.Position.Y += speed;
            if (kbState.IsKeyDown(Keys.D))
                player.Position.X += speed;

            foreach (Bullet bullet in bulletList)
                bullet.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            
            //draw floor
            for (int x = 0; x < graphics.GraphicsDevice.Viewport.Width / floorTexture.Width + 1; x++)
                for (int y = 0; y < graphics.GraphicsDevice.Viewport.Height / floorTexture.Height + 1; y++)
                spriteBatch.Draw(floorTexture, new Vector2(x * floorTexture.Width, y * floorTexture.Height), Color.White);

            //draw player
            spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.Rotation, new Vector2(33, 33), 1.5f, SpriteEffects.None, 1.0f);

            //draw crosshair
            spriteBatch.Draw(crosshairTexture, new Vector2(mState.X - crosshairTexture.Width / 2, mState.Y - crosshairTexture.Height / 2), Color.White);

            //draw bullets
            foreach (Bullet bullet in bulletList)
                spriteBatch.Draw(bulletTexture, new Vector2(bullet.Position.X - bulletTexture.Width / 2, bullet.Position.Y - bulletTexture.Height / 2), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
