using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class Tile : Entity
    {
        public const float TileWidth = 16.0f;
        public const float TileHeight = 16.0f;

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
            if (r > 0.30f)
                _Animations.AddAnimation(new Animation("sand_tile", "Normal", 1, 1, 8.0f));
            else if (r > 0.20f)
                _Animations.AddAnimation(new Animation("crosshair", "Normal", 2, 2, 5.0f));
            else
                _Animations.AddAnimation(new Animation("mg_bullet", "Normal", 1, 1, 5.0f));

            _Animations.PlayAnimation("Normal");

            // Set tiles towards back of screen
            _Depth = 0.95f;

            // Return the name for this class
            return "Tile";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            
            WorldScreen scr = (WorldScreen)_Parent;
            Marine player = scr.Player;
            float range = player.VisRange;
            float x_diff = _Geometry.Position.X-player.Geometry.Position.X;
            float y_diff = _Geometry.Position.Y-player.Geometry.Position.Y;
            float dist = (float)Math.Sqrt( (x_diff*x_diff) + (y_diff*y_diff) );
            float val = 1.0f - (dist / range);
            if (val < 0.0f) val = 0.0f;

            _ColourOverlay = new Color(new Vector3(val, val, val));
        }
    }
}
