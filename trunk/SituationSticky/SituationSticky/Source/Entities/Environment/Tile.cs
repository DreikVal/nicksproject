using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Tile : Entity
    {
        #region Constants

        public static float         TileWidth           = 24.0f;
        public static float         TileHeight          = 24.0f;

        #endregion

        #region Members

        /// <summary>
        /// Gets or sets the TileGen index used to create this tile.
        /// </summary>
        public int TileIndex { get { return _TileIndex; } set { _TileIndex = value; } }
        protected int _TileIndex;

        /// <summary>
        /// An array of TileGen functions.
        /// </summary>
        public static Func<Screen, int, int, int, Tile>[] TileGen = {
                                                            Tile_Floor01,
                                                            Tile_Floor02,
                                                            Tile_Floor03,
                                                            Tile_Floor04,
                                                            Tile_Floor05,
                                                            Tile_Floor06,
                                                            Tile_Floor07,
                                                            Tile_Wall01,
                                                            Tile_Wall02,
                                                            Tile_Wall03,
                                                            Tile_Wall04,
                                                            Tile_Wall05,
                                                            Tile_Wall06,
                                                            Tile_Wall07
                                                        };

        #endregion

        #region Init and Disposal

        public Tile(Screen parent, String texName, bool collidable, int row, int col, int tileIndex)
            : base( ((collidable) ? parent.Walls : parent.Tiles), new Vector2(col*TileWidth, row*TileHeight), TileWidth, TileHeight, 0f)
        {
            _TileIndex = tileIndex;

            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation(texName, "Normal", 1, 1, 1.0f));

            if (collidable)
            {
                // Collision settings
                _CollisionType = CollisionType.Passive;
                _CollisionRadius = _Radius * 0.7f;
                _Shadow = new ShadowRegion(this, _Position, _Radius);
            }
            else CollisionType = CollisionType.None;
        }

        public override string Initialize()
        {
            // Settings
            _ColourOverlay = Color.White;
            _DynamicLighting = true;
            _Depth = 0.95f;

            return "Tile";
        }

        #endregion

        #region Utility

        protected override void HandleCollision(Entity ent)
        {
            base.HandleCollision(ent);

            // Only consider collisions between marines and drones
            if (ent as Marine == null && ent as Drone == null) return;

            // Calculate angle between tile and entity
            Vector2 diff = _Position - ent.Position;
            double angle = Math.Atan2(diff.Y, diff.X);

            // Bounce entity back
            ent.SetXPosition( (float)( _Position.X + (ent.CollisionRadius + _CollisionRadius) * -Math.Cos(angle)) );
            ent.SetYPosition( (float)( _Position.Y + (ent.CollisionRadius + _CollisionRadius) * -Math.Sin(angle)) );
        }

        #endregion

        #region TileGen Functions

        public static Tile Tile_Floor01(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile01_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Wall01(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile01_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Wall02(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile02_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Wall03(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile03_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor02(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile02_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Wall04(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile04_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Wall05(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile05_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor03(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile03_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor04(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile04_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor05(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile05_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Floor06(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile06_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Wall06(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile06_1x1", true, row, col, tileIndex);
        }
        public static Tile Tile_Floor07(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/FloorTile07_1x1", false, row, col, tileIndex);
        }
        public static Tile Tile_Wall07(Screen parent, int row, int col, int tileIndex)
        {
            return new Tile(parent, "Textures/Environment/WallTile07_1x1", true, row, col, tileIndex);
        }

        #endregion
    }
}
