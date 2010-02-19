using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class Blood : Entity
    {
        protected int _Remaining, _LifeTime;
        protected float _Speed, _BaseSize, _SizeVar, _BaseSpeed, _SpeedVar, _SpeedDamp;

        public Blood(Screen parent, Vector2 position, Color overlay, float baseSize, float sizeVar, float baseSpeed, float speedVar, float speedDamp, int lifeTime)
            : base(parent)
        {
            _ColourOverlay = overlay;
            _BaseSize = baseSize;
            _SizeVar = sizeVar;
            _BaseSpeed = baseSpeed;
            _SpeedVar = speedVar;
            _SpeedDamp = speedDamp;
            _LifeTime = lifeTime;
            _Remaining = _LifeTime;

            // Create collision geometry for the marine
            float size = (float)Application.AppReference.Random.NextDouble() * _SizeVar + _BaseSize;
            _Geometry = new Geometry(this, new Vector2(), size, size, 0.0f, size);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            Animation normal = new Animation("blood", "Normal", 1, 7, 6.0f);
            normal.Loop = 1;
            _Animations.AddAnimation(normal);
            _Animations.PlayAnimation("Normal");

            _Geometry.Direction = Application.AppReference.Random.NextDouble() * Math.PI * 2;
            _Speed = (float)Application.AppReference.Random.NextDouble() * _SpeedVar + _BaseSpeed;
            _Geometry.Position = position;
        }

        public override string Initialize()
        {          
            // Set crosshair to front of screen
            _Depth = 0.75f;

            //_DynamicLighting = true;

            // Return the name for this class
            return "Blood";
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (_Remaining-- < 0) Dispose();
            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Speed *= _SpeedDamp;
        }
    }
}
