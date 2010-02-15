using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AlienShooterGame
{
    class Marine : Entity
    {
        public float Speed { get { return _Speed; } set { _Speed = value; } }
        protected float _Speed = 0.1f;

        public float StrafeSpeed { get { return _StrafeSpeed; } set { _StrafeSpeed = value; } }
        protected float _StrafeSpeed = 0.06f;

        public bool MoveForward { get { return _MoveForward; } set { _MoveForward = value; } }
        protected bool _MoveForward = false;

        public bool MoveBack { get { return _MoveBack; } set { _MoveBack = value; } }
        protected bool _MoveBack = false;

        public bool MoveLeft { get { return _MoveLeft; } set { _MoveLeft = value; } }
        protected bool _MoveLeft = false;

        public bool MoveRight { get { return _MoveRight; } set { _MoveRight = value; } }
        protected bool _MoveRight = false;

        public Marine(Screen parent) : base(parent) { }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateCircularGeometry(this, 30.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("soldier", "Normal", 1, 1, 18.0f));

            // Set marine towards front of screen
            _Depth = 0.2f;

            // Return the name for this class
            return "Marine";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            MouseState mState = Mouse.GetState();

            Vector2 mLoc = new Vector2();
            mLoc.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            mLoc.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;

            _Geometry.Direction = (float)Math.Atan2(mLoc.Y - Geometry.Position.Y, mLoc.X - Geometry.Position.X) + MathHelper.ToRadians(90);

            if (_MoveForward)
            {
                _Geometry.Position.X += (float)Math.Sin(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                _Geometry.Position.Y += -(float)Math.Cos(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            }
            if (_MoveBack)
            {
                _Geometry.Position.X -= (float)Math.Sin(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                _Geometry.Position.Y -= -(float)Math.Cos(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            }
            if (_MoveLeft)
            {
                _Geometry.Position.X += -(float)Math.Cos(Geometry.Direction) * _StrafeSpeed * time.ElapsedGameTime.Milliseconds;
                _Geometry.Position.Y += -(float)Math.Sin(Geometry.Direction) * _StrafeSpeed * time.ElapsedGameTime.Milliseconds;
            }
            if (_MoveRight)
            {
                _Geometry.Position.X += (float)Math.Cos(Geometry.Direction) * _StrafeSpeed * time.ElapsedGameTime.Milliseconds;
                _Geometry.Position.Y += (float)Math.Sin(Geometry.Direction) * _StrafeSpeed * time.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
