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
        public const int LifeTime = 19;
        public const float BaseSize = 8.0f;
        public const float SizeVariation = 32.0f;
        public const float SpeedBase = 0.1f;
        public const float SpeedVariation = 0.2f;
        public const float SpeedDamping = 0.92f;
        protected int _Remaining = LifeTime;
        protected float _Speed;

        public Blood(Screen parent, Vector2 position, Color overlay)
            : base(parent)
        {
            _Geometry.Position = position;
            _ColourOverlay = overlay;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            float size = (float)Application.AppReference.Random.NextDouble() * SizeVariation + BaseSize;
            _Geometry = new Geometry(this, new Vector2(), size, size, 0.0f, size);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            Animation normal = new Animation("blood", "Normal", 1, 7, 5.0f);
            normal.Loop = 1;
            _Animations.AddAnimation(normal);
            _Animations.PlayAnimation("Normal");

            // Set crosshair to front of screen
            _Depth = 0.75f;

            //_DynamicLighting = true;

            _Geometry.Direction = Application.AppReference.Random.NextDouble() * Math.PI * 2;
            _Speed = (float)Application.AppReference.Random.NextDouble() * SpeedVariation + SpeedBase;

            // Return the name for this class
            return "Blood";
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (_Remaining-- < 0) Dispose();
            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Speed *= SpeedDamping;
        }
    }
}
