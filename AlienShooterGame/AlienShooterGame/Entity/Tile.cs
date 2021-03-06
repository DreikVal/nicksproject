﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Tile : Entity
    {
        public static float TileWidth = 8.0f;
        public static float TileHeight = 8.0f;

        protected int _Row = 0;
        protected int _Col = 0;

        public int TileIndex { get { return _TileIndex; } set { _TileIndex = value; } }
        protected int _TileIndex;

        public static Func<Screen, int, int, int, Tile>[] TileGen = {
                                                            Tile_Floor1,
                                                            Tile_Floor2,
                                                            Tile_Floor3,
                                                            Tile_Floor4,
                                                            Tile_Floor5,
                                                            Tile_Floor6,
                                                            Tile_Floor7,
                                                            Tile_Floor8,
                                                            Tile_Floor9,
                                                            Tile_Floor10,
                                                            Tile_Floor11,
                                                            Tile_Floor12,
                                                            Tile_Floor13,
                                                            Tile_Floor14
                                                        };

        public Tile(Screen parent, String texName, bool collidable, int row, int col, int tileIndex)
            : base(parent)
        {
            _Row = row;
            _Col = col;

            // Set tile location
            Geometry.Position.X = _Col * TileWidth;
            Geometry.Position.Y = _Row * TileHeight;

            _TileIndex = tileIndex;

            _Animations.AddAnimation(new Animation(texName, "Normal", 1, 1, 1.0f));

            if (collidable)
            {
                CollisionType = CollisionType.Passive;
                _Shadows.Add(new ShadowRegion(this, Geometry.Position, Geometry.CollisionRadius));
            }
            else CollisionType = CollisionType.None;

            _Parent.BGEntities.Add(_ID, this);
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), TileWidth, TileHeight, 0.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            _ColourOverlay = Color.White;
            _DynamicLighting = true;

            // Set tiles towards back of screen
            _Depth = 0.95f;

            // Return the name for this class
            return "Tile";
        }

        protected override void HandleCollision(Entity ent)
        {
            base.HandleCollision(ent);

            if (ent as Bullet != null)
                ent.Dispose();

            if (ent as Marine == null && ent as Alien == null) return;

            Vector2 diff = Geometry.Position - ent.Geometry.Position;
            double angle = Math.Atan2(diff.Y, diff.X);
            ent.Geometry.Position.X = (float)( Geometry.Position.X + ((ent.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Cos(angle)));
            ent.Geometry.Position.Y = (float)( Geometry.Position.Y + ((ent.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Sin(angle)));
        }

        public override void Draw(GameTime time, SpriteBatch batch) { }

        public override void BackgroundDraw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);
        }


        public static Tile Tile_Floor1(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "blade_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor2(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "detail_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor3(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "bar_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor4(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "para_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor5(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "caution_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor6(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "ball_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor7(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "cross_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor8(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "dirt_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor9(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "grass_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor10(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "sand_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor11(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "road_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor12(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "half_tile", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor13(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "old_tile", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor14(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "sci_tile", false, row, col, tileIndex);
        }
    }
}
