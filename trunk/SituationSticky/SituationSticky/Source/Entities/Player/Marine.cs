using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SituationSticky
{
    public class Marine : Entity
    {
        #region Constants

        public const float      PlayerSpeed                 = 0.13f;
        public const int        MaxHP                       = 100;
        public const float      DefaultCollisionRadius      = 18f;
        public static int[]     DefaultCollisionPeriod      = { 80, 60, 40, 25, 16 };
        public static int[]     BloodParticles              = { 3, 5, 8, 13, 18 };       
        public static Color     BloodColour                 = Color.Red;
        public const float      BloodSizeBase               = 3f;
        public const float      BloodSizeVar                = 26f;
        public const int        BloodLifeTime               = 600;
        public const float      BloodSpeedBase              = 0.1f;
        public const float      BloodSpeedVar               = 0.3f;
        public const float      BloodSpeedDamp              = 0.91f;
        public static int[]     DeathParticles              = { 9, 20, 45, 90, 180 };
        public static Color     DeathColour                 = Color.Red;
        public const float      DeathSizeBase               = 12f;
        public const float      DeathSizeVar                = 78f;
        public const int        DeathLifeTime               = 900;
        public const float      DeathSpeedBase              = 0.2f;
        public const float      DeathSpeedVar               = 1.6f;
        public const float      DeathSpeedDamp              = 0.88f;
        public const float      ScoreMulti_Growth           = 1.4f;
        public const int        ScoreMulti_Time             = 1200;
        public const float      FlashLight_Range            = 650f;
        public const float      FlashLight_Bandwidth        = 1.8f;
        public static Color     FlashLight_Colour           = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        public const float      MuzzleFlash_Range           = 280f;
        public const float      MuzzleFlash_Bandwidth       = 5.8f;
        public static Color     MuzzleFlash_Colour          = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public const int        MuzzleFlash_Duration        = 200;
        public const float      NightVision_Range           = 480f;
        public const float      NightVision_Bandwidth       = 6.28f;
        public static Color     NightVision_Colour          = new Color(0.0f, 0.8f, 0.0f, 1.0f);

        #endregion

        #region Members

        /// <summary>
        /// Gets a reference to the marine's flashlight entity.
        /// </summary>
        public LightSource FlashLight { get { return _FlashLight; } }
        protected LightSource _FlashLight;

        /// <summary>
        /// Gets a reference to the marine's muzzle flash.
        /// </summary>
        public LightSource Muzzle { get { return _Muzzle; } }
        protected LightSource _Muzzle;

        /// <summary>
        /// Gets the remaining life time of the muzzle flash.
        /// </summary>
        public int MuzzleLifeTime { get { return _MuzzleLifeTime; } set { _MuzzleLifeTime = value; } }
        protected int _MuzzleLifeTime;

        /// <summary>
        /// Gets a reference to the marine's night vision entity.
        /// </summary>
        public LightSource NightVision { get { return _NightVision; } }
        protected LightSource _NightVision;

        /// <summary>
        /// Gets or sets the value for the player's score.
        /// </summary>
        public int Score { get { return _Score; } set { _Score = value; } }
        protected int _Score;

        /// <summary>
        /// Gets or sets the marine's current HP.
        /// </summary>
        public int CurrentHP { get { return _CurrentHP; } set { _CurrentHP = value; } }
        protected int _CurrentHP = MaxHP;

        /// <summary>
        /// Gets the marine's current score multiplier
        /// </summary>
        public float ScoreMultiplier { get { return _ScoreMultiplier; } set { _ScoreMultiplier = value; } }
        protected float _ScoreMultiplier = 1.0f;
        protected int _MultiReset = 0;

        /// <summary>
        /// Used to track how many direction's the player is moving in, and adjusts speed accordingly.
        /// </summary>
        protected int _NumDirections = 0;

        /// <summary>
        /// Gets or sets whether the marine is moving forward.
        /// </summary>
        public bool MoveForward { get { return _MoveForward; } 
            set {
                if (value == _MoveForward) return;
                _MoveForward = value; 
                if (_MoveForward) _NumDirections++;
                else _NumDirections--;
            }
        }
        protected bool _MoveForward = false;

        /// <summary>
        /// Gets or sets whether the marine is moving backwards.
        /// </summary>
        public bool MoveBack { get { return _MoveBack; }
            set
            {
                if (value == _MoveBack) return; 
                _MoveBack = value;
                if (_MoveBack) _NumDirections++;
                else _NumDirections--;
            }}
        protected bool _MoveBack = false;

        /// <summary>
        /// Gets or sets whether the marine is moving left.
        /// </summary>
        public bool MoveLeft { get { return _MoveLeft; } 
            set {
                if (value == _MoveLeft) return;
                _MoveLeft = value;
                if (_MoveLeft) _NumDirections++;
                else _NumDirections--; ;
            }
        }
        protected bool _MoveLeft = false;

        /// <summary>
        /// Gets or sets whether the marine is moving right.
        /// </summary>
        public bool MoveRight { get { return _MoveRight; } 
            set {
                if (value == _MoveRight) return;
                _MoveRight = value;
                if (_MoveRight) _NumDirections++;
                else _Speed /= _NumDirections--;
            }
        }
        protected bool _MoveRight = false;

        //weapons
        //public List<Weapon> weaponList = new List<Weapon>();
        public Weapon CurrentWeapon { get { return _CurrentWeapon; } set { _CurrentWeapon = value; } }
        protected Weapon _CurrentWeapon;

        #endregion

        #region Init and Disposal

        public Marine(Screen parent, Vector3 position) : base(parent.Entities, position, new Vector3(48,48,48), Vector2.Zero) { }

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/Player/Marine01_1x1", "Normal", 1, 1, 18.0f));

            // Entity settings
            _Depth = 0.8f;
            _DynamicLighting = false;
            _Speed = PlayerSpeed;
            _CollisionType = CollisionType.Active;
            _CollisionRadius = DefaultCollisionRadius;
            _CollisionPeriod = DefaultCollisionPeriod[Application.AppReference.GfxLevel];

            // Light sources
            _FlashLight = new LightSource(_Parent, this, _Position, new Color(1.0f, 1.0f, 0.7f), 625f, 1.6f, Vector2.Zero);
            _Muzzle = new LightSource(_Parent, this, _Position, Color.White, 325f, 4.0f, Vector2.Zero);
            _Muzzle.Active = false;
            _NightVision = new LightSource(_Parent, this, _Position, new Color(100, 255, 100), 280f, 4.8f, Vector2.Zero);
            _NightVision.Active = false;

            // Weapons
            _CurrentWeapon = new Weapon(_Parent, this);
            //weaponList.Add(new MachineGun(this));
            //weaponList.Add(new AutoHandGun(this));
            //currentWeapon = weaponList[0];

            return "Marine";
        }

        public override void Dispose()
        {
            base.Dispose();

            // Create blood explosion
            for (int i = 0; i < DeathParticles[Application.AppReference.GfxLevel]; i++)
                new Blood(_Parent, Position, DeathColour, DeathSizeBase, DeathSizeVar, DeathSpeedBase, DeathSpeedVar, DeathSpeedDamp, DeathLifeTime);

            // Set respawn message
            _Parent.Message = "Press secondary fire to respawn...";
        }

        public static Marine CreateMarine(Screen parent, Vector3 position) { return new Marine(parent, position); }

        #endregion

        #region Utility
        
        /// <summary>
        /// Awards the player with a score. The base score supplied is multiplied by the player's score multiplier.
        /// </summary>
        /// <param name="baseScore">The raw score that is awarded to the player.</param>
        /// <returns>The score added after the score multiplier has been applied.</returns>
        public virtual int GiveScore(int baseScore)
        {
            int score = (int) (baseScore * _ScoreMultiplier);
            _Score += score;
            _MultiReset = ScoreMulti_Time;
            _ScoreMultiplier *= ScoreMulti_Growth;
            return score;
        }

        #endregion

        #region Update

        public override void Update(GameTime time) 
        {
            // Set speed to 0 so that base.Update() doesn't move the marine in the direction he's facing
            _Speed = 0f;
            base.Update(time);

            if (Disposed) return;

            // Point marine towards the mouse
            MouseState mState = Mouse.GetState();
            Vector2 mLoc = new Vector2();
            mLoc.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            mLoc.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;
            _Direction.X = (float)(Math.Atan2(mLoc.Y - _Position.Y, mLoc.X - _Position.X) + Math.PI / 2);

            // Check HP
            if (_CurrentHP <= 0)
            {
                Dispose();
                return;
            }

            // Check score multipliers
            _MultiReset -= time.ElapsedGameTime.Milliseconds;
            if (_MultiReset <= 0)
            {
                _ScoreMultiplier = 1.0f;
            }

            // Handle muzzle flash
            if (_MuzzleLifeTime <= 0) _Muzzle.Active = false;
            else _MuzzleLifeTime -= time.ElapsedGameTime.Milliseconds;

            // Handle movement
            _Speed = PlayerSpeed;
            if (_NumDirections >= 2) // Apply speed modifier for diagnol movement (else player moves faster when moving diagnolly)
                _Speed *= 0.707106781187f;
            if (_Parent.Manager.Input.AbsoluteMovement)
            {
                if (_MoveForward)
                {
                    _Position.Y -= _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveBack)
                {
                    _Position.Y += _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveLeft)
                {
                    _Position.X -= _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveRight)
                {
                    _Position.X += _Speed * time.ElapsedGameTime.Milliseconds;
                }
            }
            else
            {
                if (_MoveForward)
                {
                    _Position.X += (float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Position.Y += -(float)Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveBack)
                {
                    _Position.X -= (float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Position.Y -= -(float)Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveLeft)
                {
                    _Position.X += -(float)Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Position.Y += -(float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
                if (_MoveRight)
                {
                    _Position.X += (float)Math.Cos(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                    _Position.Y += (float)Math.Sin(_Direction.X) * _Speed * time.ElapsedGameTime.Milliseconds;
                }
            }
        }

        #endregion
    }
}
