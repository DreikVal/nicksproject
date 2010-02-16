using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class TileType
    {
        public bool Collidable { get { return _Collidable; } set { _Collidable = value; } }
        protected bool _Collidable = false;

        public bool Disposed { get { return _Disposed; } }
        private bool _Disposed = false;

        public Rectangle CurrentSource { get { return _CurrentSource; } set { _CurrentSource = value; } }
        protected Rectangle _CurrentSource = new Rectangle();

        public AnimationSet Animations { get { return _Animations; } }
        protected AnimationSet _Animations = new AnimationSet();

        public TileType(String tex)
        {
            _Animations.AddAnimation(new Animation(tex, "Normal", 1, 1, 8.0f));
        }

        public string Initialize()
        {

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

            // Return the name for this class
            return "Tile";
        }

        public void Update(Microsoft.Xna.Framework.GameTime time, WorldScreen scr)
        {
            if (_Disposed) return;

            if (_Animations.Current == null) return;
            else
                CurrentSource = _Animations.Current.UpdateSource(time);
        }
    }
}
