﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AlienShooterGame
{
    public class Marine : Entity
    {
        public float Speed { get { return _Speed; } set { _Speed = value; } }
        protected float _Speed = 0.15f;

        public LightSource FlashLight { get { return _FlashLight; } }
        protected LightSource _FlashLight;

        public LightSource Muzzle { get { return _Muzzle; } }
        protected LightSource _Muzzle;
        public int MuzzleFrames { get { return _MuzzleFrames; } set { this._MuzzleFrames = value; } }
        protected int _MuzzleFrames;

        public LightSource NightVision { get { return _NightVision; } }
        protected LightSource _NightVision;

        public int Score { get { return _Score; } set { _Score = value; } }
        protected int _Score;

        public int _Reloading = -1;

        public bool Reloading { get { return _Reloading > 0; } }

        public const int MaxHP = 100;
        public int CurrentHP { get { return _CurrentHP; } set { _CurrentHP = value; } }
        protected int _CurrentHP = MaxHP;

        public const int ClipSize = 45;
        public int Ammo { get { return _Ammo; } set { _Ammo = value; } }
        protected int _Ammo = ClipSize;

        public int BloodPerHit { get { return _BloodPerHit; } }
        protected int _BloodPerHit = 14;

        public int BloodOnDeath { get { return _BloodOnDeath; } }
        protected int _BloodOnDeath = 56;

        public float ScoreMultiplier { get { return _ScoreMultiplier; } }
        protected float _ScoreMultiplier = 1.0f;
        protected int _MultiReset = 0;
        public int MultiTime = 1000;
        protected float _MultiGrowth = 1.4f;

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


        //weapons
        public List<Weapon> weaponList = new List<Weapon>();
        public Weapon currentWeapon;

        public Marine(Screen parent) : base(parent) { }

        public override string Initialize()
        {

            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 54.0f, 54.0f, 0.0f, 16.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("soldier", "Normal", 1, 1, 18.0f));

            // Set marine towards front of screen
            _Depth = 0.8f;

            // Create flashlight for marine
            _FlashLight = new LightSource(_Parent, new Color(1.0f, 1.0f, 0.7f), 625.0f, 1.6f, 0.0, _Geometry.Position);

            // Create small glow radius
            _Muzzle = new LightSource(_Parent, Color.White, 325.0f, 4.0f, 0.0, _Geometry.Position);
            _Muzzle.Active = false;

            // Create night vision
            _NightVision = new LightSource(_Parent, new Color(100, 255, 100), 280.0f, 4.8f, 0.0, _Geometry.Position);
            _NightVision.Active = false;

            // Activate dynamic lighting for the marine
            _DynamicLighting = false;

            // Flag as an active collision entity
            CollisionType = CollisionType.Active;

            //give him a basic weapon
            weaponList.Add(new MachineGun(this));
            weaponList.Add(new AutoHandGun(this));
            currentWeapon = weaponList[0];

            // Return the name for this class
            return "Marine";
        }

        public void Fire()
        {
            if (!Reloading && !Disposed)
                currentWeapon.Fire();
        }

        public void Reload()
        {
            _Reloading = currentWeapon.ReloadTime;
        }
        public override void Dispose()
        {
            base.Dispose();

            for (int i = 0; i < BloodOnDeath; i++)
                new Blood(_Parent, _Geometry.Position, Color.Red, 12.0f, 56.0f, 0.1f, 0.8f, 0.88f, 42);

            _Parent.Message = "Press secondary fire to respawn...";

            _FlashLight.Dispose();
            _NightVision.Dispose();
            _Muzzle.Dispose();
        }

        public int GiveScore(int baseScore)
        {
            int score =(int) (baseScore * _ScoreMultiplier);
            _Score += score;
            _MultiReset = MultiTime;
            _ScoreMultiplier *= _MultiGrowth;
            return score;
        }

        public override void Update(GameTime time)
        {}

        public void UpdateFirst(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            if (Disposed) return;

            MouseState mState = Mouse.GetState();

            Vector2 mLoc = new Vector2();
            mLoc.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            mLoc.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;

            if (_CurrentHP <= 0)
            {
                _CurrentHP = 0;
                Dispose();
            }

            _MultiReset -= time.ElapsedGameTime.Milliseconds;
            if (_MultiReset <= 0)
            {
                _ScoreMultiplier = 1.0f;
            }

            _Geometry.Direction = ((float)Math.Atan2(mLoc.Y - Geometry.Position.Y, mLoc.X - Geometry.Position.X) + Math.PI/2);

            _FlashLight.Geometry.Position = _Geometry.Position;
            _FlashLight.Geometry.Direction = _Geometry.Direction;
            _Muzzle.Geometry.Position = _Geometry.Position;
            _Muzzle.Geometry.Direction = _Geometry.Direction;
            _NightVision.Geometry.Position = _Geometry.Position;
            _NightVision.Geometry.Direction = _Geometry.Direction;

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
