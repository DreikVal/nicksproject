using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using FriendlyEngine;

namespace MusicShmup
{
    public class InGameState : GameState
    {
        SpriteBatch spriteBatch;
        ExperimentEngine engine;
        BackgroundTexture background;
        AnimatedSprite waterblip;
        List<NpcPlane> npcPlanes = new List<NpcPlane>();
        PlayerPlane player;
        PlayerHuman human;
        Texture2D panel;
        LoadScreen load;
        Vector2 motion = Vector2.Zero;
        Dialog dialog;

        TileMap tileMap;
        Camera camera = new Camera();
        int tempMod = 1;
        SpriteFont calibri;
        int Score = 0;
        bool paused = true;
        bool loading = true;
        bool zooming = false;
        bool landing = false;
        bool flying = false;

        static int renderSpriteCompare(AnimatedSprite a, AnimatedSprite b)
        {
            return a.Origin.Y.CompareTo(b.Origin.Y);
        }

        public InGameState(Game game)
            : base(game)
        {
            #region LoadContent

            calibri = Content.Load<SpriteFont>("Fonts/Palatino");

            load = new LoadScreen(60, Content.Load<Texture2D>("Sprites/TitleScreen/Load"), calibri);
            engine = new ExperimentEngine();

            panel = Content.Load<Texture2D>("Sprites/HUD/bottomPanel");

            dialog = new Dialog(this.Game, this.Content);
            this.Game.Components.Add(dialog);
            dialog.Enabled = false;
            dialog.Visible = false;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileMap = Content.Load<TileMap>("Layers/b");

            human = new PlayerHuman(Content.Load<Texture2D>("Sprites/humana"));
            human.Position = new Vector2(0, 312);

            player = PlayerPlane.FromFile(Content, "Content/NPCs/Player.npc");
            player.Position = new Vector2(412, 312);
            player.IsAlive = false;
            player.Scale = 4f;
            player.Speed = 4f;
            player.OriginOffset = new Vector2(0, 0);
            player.childAnimations[0].Visible = false;
            player.childAnimations[2].Visible = false;
            player.childAnimations[2].SE = SpriteEffects.None;

            player.Indicator = Content.Load<Texture2D>("Sprites/missileIndicator");
            player.Indicator1 = Content.Load<Texture2D>("Sprites/missileIndicator");
            player.halfWhite = new Color(255, 255, 255, 0.5f);

            for (int a = 0; a < 50; a++)
            {
                player.MissileList.Add(new Missile(Content.Load<Texture2D>("Sprites/missile")));
                player.MissileList[a].childAnimations.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprites/exhaust")));
                player.MissileList[a].childAnimations[0].Scale = 0.5f;
                player.MissileList[a].childAnimations[0].TrackParent = true;
                player.MissileList[a].childOffsets.Add(new Point(-8, 1));
                for (int i = 0; i < player.MissileList[a].smokeListCount; i++)
                {
                    player.MissileList[a].smokeList.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprites/minismoke")));
                    player.MissileList[a].smokeList[i].Visible = false;
                    player.MissileList[a].smokeList[i].Scale = .75f;
                }
            }


            for (int i = 0; i < 5; i++)
            {
                npcPlanes.Add(NpcPlane.FromFile(Content, "Content/NPCs/Red.npc"));
                npcPlanes.Add(NpcPlane.FromFile(Content, "Content/NPCs/Blue.npc"));
                npcPlanes.Add(NpcPlane.FromFile(Content, "Content/NPCs/Green.npc"));
                npcPlanes.Add(NpcPlane.FromFile(Content, "Content/NPCs/Mig.npc"));
            }

            for (int j = 0; j < npcPlanes.Count; j++)
            {
                npcPlanes[j].childAnimations[2].Visible = false;
                npcPlanes[j].OriginOffset = Vector2.Zero;
                for (int i = 0; i < npcPlanes[j].FireArray.Length; i++)
                {
                    npcPlanes[j].FireArray[i] = new Projectile(Content.Load<Texture2D>("Sprites/projectileb"));
                }
                npcPlanes[j].Missile = new Missile(Content.Load<Texture2D>("Sprites/missile"));
                npcPlanes[j].Missile.targetHair = new AnimatedSprite(Content.Load<Texture2D>("Sprites/targethaira"));
                npcPlanes[j].Missile.missileTriangle = Content.Load<Texture2D>("Sprites/missilething");
                npcPlanes[j].Missile.childAnimations.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprites/exhaust")));
                npcPlanes[j].Missile.childAnimations[0].Scale = 0.5f;
                npcPlanes[j].Missile.childAnimations[0].TrackParent = true;
                npcPlanes[j].Missile.childAnimations[0].SE = SpriteEffects.FlipHorizontally;
                npcPlanes[j].Missile.childOffsets.Add(new Point(10, 1));

                for (int i = 0; i < npcPlanes[j].Missile.smokeListCount; i++)
                {
                    npcPlanes[j].Missile.smokeList.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprites/minismoke")));
                    npcPlanes[j].Missile.smokeList[i].Visible = false;
                    npcPlanes[j].Missile.smokeList[i].Scale = .75f;
                }
            }

            for (int i = 0; i < player.FireArray.Length; i++)
            {
                player.FireArray[i] = new Projectile(Content.Load<Texture2D>("Sprites/projectileb"));
            }

            waterblip = new AnimatedSprite(Content.Load<Texture2D>("Sprites/Water/waterblip"));
            waterblip.Position = new Vector2(640, 150);

            background = new BackgroundTexture(Content.Load<Texture2D>("Sprites/Water/water1"),
                                               Content.Load<Texture2D>("Sprites/Water/water2"),
                                               Content.Load<Texture2D>("Sprites/Water/water3"),
                                               Content.Load<Texture2D>("Sprites/Water/water4"),
                                               Content.Load<Texture2D>("Sprites/Water/water5"),
                                               Content.Load<Texture2D>("Sprites/Water/water6"));
            #endregion


            #region Initialize

            camera.ScrollRate.X = 0;
            FrameAnimation hup = new FrameAnimation(8, 32, 32, 0, 0);
            hup.FramesPerSecond = 8;
            FrameAnimation hdown = new FrameAnimation(8, 32, 32, 0, 32);
            hdown.FramesPerSecond = 8;
            FrameAnimation hleft = new FrameAnimation(8, 32, 32, 0, 64);
            hleft.FramesPerSecond = 8;
            FrameAnimation hright = new FrameAnimation(8, 32, 32, 0, 96);
            hright.FramesPerSecond = 8;
            human.Animations.Add("HUp", hup);
            human.Animations.Add("HDown", hdown);
            human.Animations.Add("HLeft", hleft);
            human.Animations.Add("HRight", hright);
            human.CurrentAnimationName = "HRight";
            FrameAnimation exhau = new FrameAnimation(2, 12, 8, 0, 0);
            exhau.FramesPerSecond = 24;

            FrameAnimation missileframe = new FrameAnimation(5, 10, 6, 0, 0);
            missileframe.FramesPerSecond = 24;

            FrameAnimation target = new FrameAnimation(8, 17, 17, 0, 0);
            target.FramesPerSecond = 24;

            FrameAnimation projanim = new FrameAnimation(4, 5, 5, 0, 0);
            projanim.FramesPerSecond = 24;
            foreach (Projectile a in player.FireArray)
            {
                FrameAnimation b = (FrameAnimation)projanim.Clone();
                a.Animations.Add("Projectile", b);
            }

            FrameAnimation smoke = new FrameAnimation(16, 13, 12, 0, 0);
            smoke.FramesPerSecond = 48;

            for (int j = 0; j < npcPlanes.Count; j++)
            {
                foreach (Projectile a in npcPlanes[j].FireArray)
                {
                    FrameAnimation b = (FrameAnimation)projanim.Clone();
                    a.Animations.Add("Projectile", b);
                }
                FrameAnimation c = (FrameAnimation)missileframe.Clone();
                npcPlanes[j].Missile.Animations.Add("Missile", c);

                FrameAnimation e = (FrameAnimation)exhau.Clone();
                npcPlanes[j].Missile.childAnimations[0].Animations.Add("Exhaust", e);
                npcPlanes[j].Missile.childAnimations[0].CurrentAnimationName = "Exhaust";

                FrameAnimation t = (FrameAnimation)target.Clone();
                npcPlanes[j].Missile.targetHair.Animations.Add("Flash", t);
                npcPlanes[j].Missile.targetHair.CurrentAnimationName = "Flash";

                for (int i = 0; i < npcPlanes[j].Missile.smokeListCount; i++)
                {
                    FrameAnimation b = (FrameAnimation)smoke.Clone();
                    npcPlanes[j].Missile.smokeList[i].Animations.Add("Smoke", b);
                    npcPlanes[j].Missile.smokeList[i].CurrentAnimationName = "Smoke";
                    npcPlanes[j].Missile.smokeList[i].OriginOffset = new Vector2(npcPlanes[j].Missile.smokeList[i].Bounds.Width / 2, npcPlanes[j].Missile.smokeList[i].Bounds.Height / 2);
                }

                npcPlanes[j].Missile.InitPath(GraphicsDevice);
            }

            for (int v = 0; v < player.MissileList.Count; v++)
            {
                for (int i = 0; i < player.MissileList[v].smokeListCount; i++)
                {
                    FrameAnimation b = (FrameAnimation)smoke.Clone();
                    player.MissileList[v].smokeList[i].Animations.Add("Smoke", b);
                    player.MissileList[v].smokeList[i].CurrentAnimationName = "Smoke";
                    player.MissileList[v].smokeList[i].OriginOffset = new Vector2(player.MissileList[v].smokeList[i].Bounds.Width / 2, player.MissileList[v].smokeList[i].Bounds.Height / 2);
                }

                FrameAnimation g = (FrameAnimation)missileframe.Clone();
                player.MissileList[v].Animations.Add("Missile", g);

                FrameAnimation f = (FrameAnimation)exhau.Clone();
                player.MissileList[v].childAnimations[0].Animations.Add("Exhaust", f);
                player.MissileList[v].childAnimations[0].CurrentAnimationName = "Exhaust";

                player.MissileList[v].PlayerMissile = true;
            }

            FrameAnimation wb = new FrameAnimation(2, 32, 32, 0, 0);
            wb.FramesPerSecond = 12;
            waterblip.Animations.Add("Blip", wb);
            waterblip.CurrentAnimationName = "Blip";
            camera.Position.Y = 0;
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            if (!paused)
            {
                tileMap.Update(camera, screenWidth, screenHeight, gameTime, engine);

                camera.LockTargetToCamera(player);
                camera.ClampToArea(
                    tileMap.GetWidthInPixels() - screenWidth,
                    tileMap.GetHeightInPixels() - screenHeight);


                camera.Update();

                // Allows the game to exit
                #region inputMotion
                if (!dialog.Enabled)
                {
                    motion = Vector2.Zero;

                    if (player.IsAlive == true)
                    {
                        if (InputHelper.IsKeyDown(Keys.W))
                        {
                            motion.Y--;
                            player.CurrentAnimationName = "Up";
                        }
                        if (InputHelper.IsKeyDown(Keys.S))
                        {
                            motion.Y++;
                            player.CurrentAnimationName = "Down";
                        }
                        if (InputHelper.IsKeyDown(Keys.A))
                        {
                            motion.X--;
                            player.CurrentAnimationName = "Left";
                        }
                        if (InputHelper.IsKeyDown(Keys.D))
                        {
                            motion.X++;
                            player.CurrentAnimationName = "Right";
                        }

                        if (player.CurrentAnimationName == "Up" && InputHelper.IsKeyUp(Keys.W))
                            player.CurrentAnimationName = "Right";
                        if (player.CurrentAnimationName == "Down" && InputHelper.IsKeyUp(Keys.S))
                            player.CurrentAnimationName = "Right";
                        if (player.CurrentAnimationName == "Left" && InputHelper.IsKeyUp(Keys.A))
                            player.CurrentAnimationName = "Right";


                        if (motion != Vector2.Zero)
                        {
                            motion.Normalize();

                            motion = CheckCollisionForMotion(motion, player);

                            player.Position += motion * player.Speed;

                            //UpdateSpriteAnimation(motion);
                            player.IsAnimating = true;

                            CheckForUnwalkableTiles(player);
                        }
                    }
                    else if (human.IsAlive == true)
                    {
                        if (InputHelper.IsKeyDown(Keys.W))
                        {
                            motion.Y--;
                            human.CurrentAnimationName = "Up";
                        }
                        if (InputHelper.IsKeyDown(Keys.S))
                        {
                            motion.Y++;
                            human.CurrentAnimationName = "Down";
                        }
                        if (InputHelper.IsKeyDown(Keys.A))
                        {
                            motion.X--;
                            human.CurrentAnimationName = "Left";
                        }
                        if (InputHelper.IsKeyDown(Keys.D))
                        {
                            motion.X++;
                            human.CurrentAnimationName = "Right";
                        }

                        if (motion != Vector2.Zero)
                        {
                            motion.Normalize();

                            motion = CheckCollisionForMotion(motion, human);

                            human.Position += motion * human.Speed;

                            UpdateSpriteAnimation(motion);
                            human.IsAnimating = true;

                            CheckForUnwalkableTiles(human);
                        }
                        else
                        {
                            human.IsAnimating = false;
                        }
                    }
                    #region scrollRate
                    //if (camera.ScrollRate.X > 0)
                    //    tempMod = 1;
                    //else if (camera.ScrollRate.X < 0)
                    //    tempMod = -1;
                    if (InputHelper.IsKeyDown(Keys.D1))
                        camera.ScrollRate.X = 1 * tempMod;
                    if (InputHelper.IsKeyDown(Keys.D2))
                        camera.ScrollRate.X = 2 * tempMod;
                    if (InputHelper.IsKeyDown(Keys.D3))
                        camera.ScrollRate.X = 3 * tempMod;
                    if (InputHelper.IsKeyDown(Keys.D4))
                        camera.ScrollRate.X = 4 * tempMod;
                    if (InputHelper.IsKeyDown(Keys.D5))
                        camera.ScrollRate.X = 5 * tempMod;
                    if (InputHelper.IsKeyDown(Keys.D0))
                        camera.ScrollRate.X = 0;
                    if (InputHelper.IsNewPress(Keys.D6))
                        camera.ScrollRate = Vector2.Negate(camera.ScrollRate);
                    #endregion

                    if (InputHelper.IsKeyDown(Keys.Space))
                    {
                        player.FireWeapon();
                    }
                    if (InputHelper.IsNewPress(Keys.V))
                    {
                        zooming = false;
                        landing = true;
                    }
                    if (InputHelper.IsNewPress(Keys.B))
                    {
                        zooming = true;
                        landing = false;
                    }

                    if (InputHelper.IsNewPress(Keys.R))
                    {
                        player.FireMissile();
                    }
                    human.ClampToArea(tileMap.GetWidthInPixels(), tileMap.GetHeightInPixels());
                    player.ClampToArea(tileMap.GetWidthInPixels(), tileMap.GetHeightInPixels());

                }
                #endregion
                player.UpdateAnimations(gameTime, player);
                player.Update(gameTime);
                human.Update(gameTime);

                waterblip.Update(gameTime);

                if (zooming)
                {
                    if (engine.TileWidth > 24)
                    {
                        engine.TileHeight -= 1;
                        engine.TileWidth -= 1;

                        if (camera.Position.X <= 0)
                            player.Position.X -= 2.47f;
                        else if (camera.Position.X >= 1500)
                            player.Position.X -= 2.47f;
                        else
                        {
                            if (camera.Position.X > player.Position.X)
                                camera.Position.X--;
                            if (camera.Position.X < player.Position.X)
                                camera.Position.X++;
                        }
                        if (camera.Position.Y <= 0)
                            player.Position.Y -= 2.47f;
                        else if (camera.Position.Y >= 1500)
                            player.Position.Y -= 2.47f;
                        else
                        {
                            if (camera.Position.Y > player.Position.Y)
                                camera.Position.Y--;
                            if (camera.Position.Y < player.Position.Y)
                                camera.Position.Y++;
                        }
                        player.Scale -= 0.02884f;
                        player.childAnimations[0].Scale -= 0.02884f;
                        float i = player.childAnimations[0].Scale;
                        player.childOffsets[0] = new Vector2(i - (13f * i), i - (-12f * i));
                    }
                    else
                    {
                        zooming = false;
                    }
                }

                if (landing)
                {
                    if (engine.TileWidth < 128)
                    {
                        engine.TileHeight += 1;
                        engine.TileWidth += 1;

                        if (camera.Position.X <= 0)
                            player.Position.X += 2.47f;
                        else if (camera.Position.X >= 1500)
                            player.Position.X += 2.47f;
                        else
                        {
                            if (camera.Position.X > player.Position.X)
                                camera.Position.X++;
                            if (camera.Position.X < player.Position.X)
                                camera.Position.X--;
                        }
                        if (camera.Position.Y <= 0)
                            player.Position.Y += 2.47f;
                        else if (camera.Position.Y >= 1500)
                            player.Position.Y += 2.47f;
                        else
                        {
                            if (camera.Position.Y > player.Position.Y)
                                camera.Position.Y++;
                            if (camera.Position.Y < player.Position.Y)
                                camera.Position.Y--;
                        }

                        player.Scale += 0.02884f;
                        player.childAnimations[0].Scale += 0.02884f;
                        float i = player.childAnimations[0].Scale;
                        player.childOffsets[0] = new Vector2(i - (13 * i), i - (-12 * i));
                    }
                    else
                    {
                        landing = false;
                    }
                }

                for (int i = 0; i < player.FireArray.Length; i++)
                {
                    player.FireArray[i].Update(gameTime);
                    foreach (NpcPlane npc in npcPlanes)
                    {
                        if (Projectile.IsRectColliding(player.FireArray[i], npc))
                        {
                            npc.ProjectileCollision(player.FireArray[i], npc);
                        }
                    }
                    ClampProjectileToScreen(player.FireArray[i], camera.Position.X, (int)(player.Position.X - camera.Position.X) + screenWidth, (int)camera.Position.Y + screenHeight);
                }

                for (int i = 0; i < player.MissileList.Count; i++)
                    ClampMissileToScreen(player.MissileList[i], camera.Position.X, (int)(player.Position.X - camera.Position.X) + screenWidth, (int)camera.Position.Y + screenHeight);


                foreach (NpcPlane npc in npcPlanes)
                {
                    npc.UpdateAnimations(gameTime, npc);
                    npc.Update(gameTime);

                    if (npc.incrementScore)
                    {
                        Score++;
                        npc.incrementScore = false;
                    }
                    npc.Missile.line.CreateLine(new Vector2(0, 0), new Vector2((camera.Position.X + screenWidth), 0));
                    npc.Missile.mpPosi = new Vector2(player.Position.X, npc.Missile.mpPosi.Y);
                    npc.Missile.targetHair.Update(gameTime);

                    if (npc.IsAlive)
                    {
                        for (int g = 0; g < player.MissileList.Count; g++)
                        {
                            if (player.MissileList[g].Visible == true)
                            {
                                if (Missile.IsRectColliding(player.MissileList[g], npc.homingSquare))
                                {
                                    if (player.MissileList[g].Position.Y < npc.Position.Y + (npc.Bounds.Height / 2))
                                        player.MissileList[g].direction.Y = 0.13f;

                                    else if (player.MissileList[g].Position.Y > npc.Position.Y + (npc.Bounds.Height / 2))
                                        player.MissileList[g].direction.Y = -0.13f;

                                    else if (player.MissileList[g].Position.Y <= npc.Position.Y + (npc.Bounds.Height / 2) + 4 && player.MissileList[g].Position.Y >= npc.Position.Y + (npc.Bounds.Height / 2) - 4)
                                        player.MissileList[g].direction.Y = 0f;
                                }

                                if (Missile.IsRectColliding(player.MissileList[g], npc))
                                {
                                    npc.ProjectileCollision(player.MissileList[g], npc);
                                }
                            }
                        }
                    }

                    if (npc.Position.X < camera.Position.X - npc.Bounds.Width - 12)
                        npc.ReSpawnPlane(npc);

                    if (npc.Position.Y >= player.Position.Y - 150 && npc.Position.Y <= player.Position.Y + 150)
                    {
                        if (npc.reloaded == true)
                            npc.FireMissile();
                        else if ((npc.Position.X - camera.Position.X) <= screenWidth)
                            npc.FireWeapon();
                    }

                    for (int i = 0; i < npc.FireArray.Length; i++)
                    {
                        npc.FireArray[i].Update(gameTime);
                        ClampProjectileToScreenNpc(npc.FireArray[i], camera.Position.X, (int)(npc.Position.X - camera.Position.X) - screenWidth, (int)camera.Position.Y + screenHeight);
                        if (Projectile.IsRectColliding(npc.FireArray[i], player))
                        {
                            player.ProjectileCollision(npc.FireArray[i]);
                        }
                    }
                    ClampMissileToScreenNpc(npc.Missile, (int)camera.Position.X + screenWidth, (int)camera.Position.Y + screenHeight);

                    if (npc.Missile.Position.X >= (int)camera.Position.X + screenWidth)
                    {
                        npc.Missile.targetHair.Position = new Vector2((int)camera.Position.X + screenWidth - 16, npc.Missile.Position.Y - 6);
                        AnimatedSprite.ReFresh(npc.Missile.targetHair);
                    }
                    else if (npc.Missile.Position.X <= (int)camera.Position.X + screenWidth)
                        npc.Missile.targetHair.Visible = false;

                    if (Missile.IsRectColliding(npc.Missile, player.homingSquare))
                    {
                        if (npc.Missile.Position.Y < player.Position.Y + (player.Bounds.Height / 2))
                            npc.Missile.direction.Y = -0.13f;
                        else if (npc.Missile.Position.Y > player.Position.Y + (player.Bounds.Height / 2))
                            npc.Missile.direction.Y = 0.13f;
                        else if (npc.Missile.Position.Y <= player.Position.Y + (player.Bounds.Height / 2) + 4 && npc.Missile.Position.Y >= player.Position.Y + (player.Bounds.Height / 2) - 4)
                            npc.Missile.direction.Y = 0f;
                    }
                    if (Missile.IsRectColliding(npc.Missile, player))
                    {
                        player.ProjectileCollision(npc.Missile);
                    }

                }

                if (player.MissileList[0].Visible == true)
                {
                    player.indicatorColor = player.halfWhite;
                }
                else
                {
                    player.indicatorColor = Color.White;
                }
                if (player.MissileList[1].Visible == true)
                {
                    player.indicatorColor1 = player.halfWhite;
                }
                else
                {
                    player.indicatorColor1 = Color.White;
                }

                background.Update(gameTime, camera.Position);
            }
            else
            {
                load.Update();
                if (load.loaded == true)
                {
                    loading = false;
                    paused = false;
                }
            }

            if (human.Position.X >= player.Position.X && human.Position.X <= player.Position.X + player.Bounds.Width && flying == false)
            {
                if (human.Position.Y >= player.Position.Y && human.Position.Y <= player.Position.Y + player.Bounds.Height)
                {
                    flying = true;
                    player.IsAlive = true;
                    human.IsAlive = false;
                    //camera.ScrollRate.X = 1;
                    zooming = true;
                    landing = false;
                    human.Visible = false;
                    player.childAnimations[0].Visible = true;
                    player.childAnimations[0].Scale = 4f;
                    player.childOffsets[0] = new Vector2(-12 * 4, 13 * 4);
                }
            }
            //littlehack
            player.Position += camera.ScrollRate;
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            if (!loading)
            {
                spriteBatch.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Immediate,
                    SaveStateMode.None,
                    camera.TransformMatrix);
                background.Draw(spriteBatch);
                spriteBatch.End();

                tileMap.DrawExperiment(spriteBatch, camera, engine);

                spriteBatch.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Immediate,
                    SaveStateMode.None,
                    camera.TransformMatrix);

                //graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
                graphics.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
                graphics.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;

                //waterblip.Draw(spriteBatch);
                foreach (NpcPlane npc in npcPlanes)
                {
                    npc.Missile.DrawPaths(spriteBatch);
                }
                foreach (NpcPlane npc in npcPlanes)
                {
                    for (int i = 0; i < npc.childAnimations.Count; i++)
                        npc.childAnimations[i].Draw(spriteBatch);
                    for (int i = 0; i < npc.FireArray.Length; i++)
                        npc.FireArray[i].Draw(spriteBatch);
                    npc.Missile.Draw(spriteBatch);
                    npc.Draw(spriteBatch);
                }
                foreach (Missile a in player.MissileList)
                    a.Draw(spriteBatch);

                player.Draw(spriteBatch);
                human.Draw(spriteBatch);
                for (int i = 0; i < player.childAnimations.Count; i++)
                    player.childAnimations[i].Draw(spriteBatch);
                //spriteBatch.DrawString(calibri, Score.ToString(), new Vector2(camera.Position.X + 10, 10), Color.White);
                foreach (Projectile a in player.FireArray)
                    a.Draw(spriteBatch);
                spriteBatch.Draw(panel, new Rectangle((int)camera.Position.X, (int)camera.Position.Y + 624, 1024, 144), Color.White);
                spriteBatch.Draw(player.Indicator, new Rectangle((int)camera.Position.X + 954, (int)camera.Position.Y + 683, 16, 35), player.indicatorColor);
                spriteBatch.Draw(player.Indicator, new Rectangle((int)camera.Position.X + 914, (int)camera.Position.Y + 683, 16, 35), player.indicatorColor1);

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Immediate,
                SaveStateMode.None,
                camera.TransformMatrix);
                load.Draw(spriteBatch);
                spriteBatch.End();
            }

            //base.Draw(gameTime);
        }

        private void ClampProjectileToScreen(Projectile p, float camerax, int width, int height)
        {
            if (p.Position.X - camerax < width - 1024)
                p.CleanUp(p);
            if (p.Position.Y < height - 768)
                p.Position.Y = 0;
            if (p.Position.X - camerax > (width) - p.CurrentAnimation.CurrentRect.Width)
                p.CleanUp(p);
            if (p.Position.Y > height - p.CurrentAnimation.CurrentRect.Height)
                p.Position.Y = height - p.CurrentAnimation.CurrentRect.Height;
        }

        private void ClampMissileToScreen(Missile p, float camerax, int width, int height)
        {
            if (p.Position.X - camerax < width - 1024)
                p.CleanUp();
            if (p.Position.Y < height - 768)
                p.Position.Y = 0;
            if (p.Position.X - camerax > (width * 2) - p.CurrentAnimation.CurrentRect.Width)
                p.CleanUp();
            if (p.Position.Y > height - p.CurrentAnimation.CurrentRect.Height)
                p.Position.Y = height - p.CurrentAnimation.CurrentRect.Height;
        }

        private void ClampProjectileToScreenNpc(Projectile p, float camerax, int width, int height)
        {
            if (p.Position.X - camerax < width - 1024)
                p.CleanUp(p);
            if (p.Position.Y < height - 768)
                p.Position.Y = 0;
            //if (p.Position.X > width - p.CurrentAnimation.CurrentRect.Width)
            //    p.CleanUp(p);
            if (p.Position.Y > height - p.CurrentAnimation.CurrentRect.Height)
                p.Position.Y = height - p.CurrentAnimation.CurrentRect.Height;
        }

        private void ClampMissileToScreenNpc(Missile p, int width, int height)
        {
            if (p.Position.X < width - 1024)
                p.CleanUp();
            if (p.Position.Y < height - 768)
                p.Position.Y = 0;
            //if (p.Position.X > width - p.CurrentAnimation.CurrentRect.Width)
            //    p.CleanUp(p);
            if (p.Position.Y > height - p.CurrentAnimation.CurrentRect.Height)
                p.Position.Y = height - p.CurrentAnimation.CurrentRect.Height;
        }

        private void CheckForUnwalkableTiles(AnimatedSprite player)
        {
            Point spriteCell = Engine.ConvertPositionToCell(player.Center);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null,
                downLeft = null, down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y - 1);

            if (spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1 &&
                spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);



            if (up != null && tileMap.CollisionLayer.GetCellIndex(up.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position.Y += -motion.Y * player.Speed;
                }
            }
            if (down != null && tileMap.CollisionLayer.GetCellIndex(down.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position.Y += -motion.Y * player.Speed;
                }
            }
            if (left != null && tileMap.CollisionLayer.GetCellIndex(left.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position.X += -motion.X * player.Speed;
                }
            }
            if (right != null && tileMap.CollisionLayer.GetCellIndex(right.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position.X += -motion.X * player.Speed;
                }
            }

            if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position += -motion * player.Speed;
                }
            }

            if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position += -motion * player.Speed;
                }
            }

            if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position += -motion * player.Speed;
                }
            }
            if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 2)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = player.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    player.Position += -motion * player.Speed;
                }
            }


        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
                human.CurrentAnimationName = "HRight";
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
                human.CurrentAnimationName = "HDown";
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
                human.CurrentAnimationName = "HUp";
            else
                human.CurrentAnimationName = "HLeft";
        }

        private Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite player)
        {
            Point cell = Engine.ConvertPositionToCell(player.Origin);

            int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 1)
                return motion * 0.5f;

            return motion;
        }
    }
}
