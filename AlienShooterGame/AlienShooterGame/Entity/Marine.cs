using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class Marine : Entity
    {
        public float Speed { get { return _Speed; } set { _Speed = value; } }
        protected float _Speed = 0.15f;

        public LightSource FlashLight { get { return _FlashLight; } }
        protected LightSource _FlashLight;

        public LightSource Muzzle { get { return _Muzzle; } }
        protected LightSource _Muzzle;
        protected int _MuzzleFrames;

        public LightSource NightVision { get { return _NightVision; } }
        protected LightSource _NightVision;

        public const int ReloadTime = 2500;
        protected int _Reloading = -1;

        public bool Reloading { get { return _Reloading > 0; } }

        public const int MaxHP = 100;
        public int CurrentHP { get { return _CurrentHP; } set { _CurrentHP = value; } }
        protected int _CurrentHP = MaxHP;

        public const int ClipSize = 25;
        public int Ammo { get { return _Ammo; } set { _Ammo = value; } }
        protected int _Ammo = ClipSize;

        public bool MoveForward { get { return _MoveForward; } 
            set { _MoveForward = value; 
                if (_MoveForward) _Speed *= 0.707106781187f;
                else _Speed /= 0.707106781187f;
            }
        }
        protected bool _MoveForward = false;

        public bool MoveBack { get { return _MoveBack; } 
            set { _MoveBack = value;
                if (_MoveBack) _Speed *= 0.707106781187f;
                else _Speed /= 0.707106781187f;
            }}
        protected bool _MoveBack = false;

        public bool MoveLeft { get { return _MoveLeft; } 
            set { _MoveLeft = value;
                if (_MoveLeft) _Speed *= 0.707106781187f;
                else _Speed /= 0.707106781187f;
            }
        }
        protected bool _MoveLeft = false;

        public bool MoveRight { get { return _MoveRight; } 
            set { _MoveRight = value;
                if (_MoveRight) _Speed *= 0.707106781187f;
                else _Speed /= 0.707106781187f;
            }
        }
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
            _Depth = 0.8f;

            // Create flashlight for marine
            _FlashLight = new LightSource(_Parent, Color.Yellow, 500.0f, 1.5f, 0.0, _Geometry.Position);

            // Create small glow radius
            _Muzzle = new LightSource(_Parent, Color.White, 700.0f, 4.0f, 0.0, _Geometry.Position);
            _Muzzle.Active = false;

            // Create night vision
            _NightVision = new LightSource(_Parent, new Color(100, 255, 100), 280.0f, 4.8f, 0.0, _Geometry.Position);
            _NightVision.Active = false;

            // Activate dynamic lighting for the marine
            _DynamicLighting = true;

            // Flag as an active collision entity
            CollisionType = CollisionType.Active;

            // Return the name for this class
            return "Marine";
        }

        public void Fire()
        {
            if (_Reloading >= 0) return;

            _Muzzle.Active = true;
            _MuzzleFrames = 5;
            Vector2 bulletPos = _Geometry.Position;
            bulletPos.X += (float)Math.Sin(_Geometry.Direction) * 25.0f;
            bulletPos.Y += -(float)Math.Cos(_Geometry.Direction) * 25.0f;
            new Bullet(_Parent, bulletPos, _Geometry.Direction);
            _Parent.ViewPort.Shake(1.5f, 0.8f, 0.95f);
            new MuzzleFlash(_Parent, bulletPos, this);
            if (--_Ammo <= 0)
                _Reloading = ReloadTime;
        }

        public void Reload()
        {
            _Reloading = ReloadTime;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            MouseState mState = Mouse.GetState();

            Vector2 mLoc = new Vector2();
            mLoc.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            mLoc.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;

            _Geometry.Direction = ((float)Math.Atan2(mLoc.Y - Geometry.Position.Y, mLoc.X - Geometry.Position.X) + Math.PI/2);

            _FlashLight.Position = _Geometry.Position;
            _FlashLight.Direction = _Geometry.Direction;
            _Muzzle.Position = _Geometry.Position;
            _Muzzle.Direction = _Geometry.Direction;
            _NightVision.Position = _Geometry.Position;
            _NightVision.Direction = _Geometry.Direction;

            if (_Reloading >= 0)
            {
                _Reloading -= time.ElapsedGameTime.Milliseconds;
                if (_Reloading < 0)
                    _Ammo = ClipSize;
            }

            if (_MuzzleFrames < 0) _Muzzle.Active = false;
            else _MuzzleFrames--;

            if (_Parent.Manager.Input.AbsoluteMovement)
            {
                if (_MoveForward)
                {
                    _Geometry.Position.Y -= _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveBack)
                {
                    _Geometry.Position.Y += _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveLeft)
                {
                    _Geometry.Position.X -= _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveRight)
                {
                    _Geometry.Position.X += _Speed * time.ElapsedGameTime.Milliseconds;
                }
            }
            else
            {
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
                    _Geometry.Position.X += -(float)Math.Cos(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Geometry.Position.Y += -(float)Math.Sin(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveRight)
                {
                    _Geometry.Position.X += (float)Math.Cos(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Geometry.Position.Y += (float)Math.Sin(Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
            }
        }
    }
}
