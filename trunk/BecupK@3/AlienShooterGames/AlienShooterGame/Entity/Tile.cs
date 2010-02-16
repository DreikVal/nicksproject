using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Tile : Entity
    {
        public static float TileWidth = 24.0f;
        public static float TileHeight = 24.0f;

        public bool Collidable { get { return _Collidable; } set { _Collidable = value; } }
        protected bool _Collidable = false;

        protected int _Row = 0;
        protected int _Col = 0;

        public Tile(Screen parent, int row, int col)
            : base(parent)
        {
            _Row = row;
            _Col = col;

            // Set tile location
            Geometry.Position.X = _Col * TileWidth;
            Geometry.Position.Y = _Row * TileHeight;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateRectangularGeometry(this, TileWidth, TileHeight);        

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Set tile type
            double r = Application.AppReference.Random.NextDouble();
            if (r > 0.60f)
                _Animations.AddAnimation(new Animation("road_tile", "Normal", 1, 1, 8.0f));
            else if (r > 0.50f)
                _Animations.AddAnimation(new Animation("dirt_tile", "Normal", 1, 1, 8.0f));
            else if (r > 0.15f)
                _Animations.AddAnimation(new Animation("grass_tile", "Normal", 1, 1, 8.0f));
            else
                _Animations.AddAnimation(new Animation("ground_tile", "Normal", 1, 1, 5.0f));

            _Animations.PlayAnimation("Normal");

            _ColourOverlay = Color.White;
            _DynamicLighting = true;

            // Set tiles towards back of screen
            _Depth = 0.95f;

            // Return the name for this class
            return "Tile";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }
    }
}
