using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Alien : Entity
    {
        public const int BloodPerDeath = 24;
        private const int alienHurt = 2;

        public Entity Target { get { return _Target; } set { _Target = value; } } 
        protected Entity _Target;

        public const float AlienSpeed = 0.08f;

        public Alien(Screen parent, Vector2 position, Entity target) : base(parent) 
        {
            _Geometry.Position = position;
            _Target = target;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 48.0f, 48.0f, 0.0f, 18.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("alien3", "Normal", 1, 5, 8.0f));
            _Animations.PlayAnimation("Normal");

            // Set crosshair to front of screen
            _Depth = 0.79f;

            _DynamicLighting = true;

            // Flag as an active collision entity
            CollisionType = CollisionType.Active;

            // Return the name for this class
            return "Alien";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            float x_diff = _Geometry.Position.X - _Target.Geometry.Position.X;
            float y_diff = _Geometry.Position.Y - _Target.Geometry.Position.Y;
            _Geometry.Direction = Math.Atan2(y_diff, x_diff) - Math.PI/2;

            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * AlienSpeed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * AlienSpeed * time.ElapsedGameTime.Milliseconds;
        }

        protected override void HandleCollision(Entity otherEnt)
        {
            base.HandleCollision(otherEnt);

            if (otherEnt as Bullet != null)
            Dispose();

            if (otherEnt as Marine != null)
                hurtPlayer((Marine)otherEnt);
        }

        private void hurtPlayer(Marine player)
        {
            double knockbackFactor = 1.3;
            player.CurrentHP -= alienHurt;
            Parent.ViewPort.Shake(3.0f, 0.8f, 0.95f);
            for (int i = 0; i < BloodPerDeath; i++)
                new Blood(_Parent, _Geometry.Position, Color.Red);

            //bump em
            Vector2 diff = Geometry.Position - player.Geometry.Position;
            double angle = Math.Atan2(diff.Y, diff.X);
            player.Geometry.Position.X = (float)(Geometry.Position.X + ((player.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Cos(angle) * knockbackFactor));
            player.Geometry.Position.Y = (float)(Geometry.Position.Y + ((player.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Sin(angle) * knockbackFactor));
        }

        public override void Dispose()
        {
            base.Dispose();

            for (int i = 0; i < BloodPerDeath; i++)
                new Blood(_Parent, _Geometry.Position, Color.Green);

            Alien.CreateNearbyAlien(_Parent, ((WorldScreen)_Parent).Player, 350, _Target);
        }

        public static Alien CreateNearbyAlien(Screen scr, Entity nearTo, float distance, Entity target)
        {
            double angle = Application.AppReference.Random.NextDouble() * 2 * Math.PI;
            Vector2 disp = new Vector2(distance * (float)Math.Cos(angle), distance * (float)Math.Sin(angle));
            return new Alien(scr, nearTo.Geometry.Position + disp, target);
        }

        class AlienBlood : Entity
        {
            public const int LifeTime = 19;
            public const float BaseSize = 4.0f;
            public const float SizeVariation = 8.0f;
            public const float SpeedBase = 0.1f;
            public const float SpeedVariation = 0.2f;
            public const float SpeedDamping = 0.92f;
            protected int _Remaining = LifeTime;
            protected float _Speed;

            public AlienBlood(Screen parent, Vector2 position) : base(parent) 
            {
                _Geometry.Position = position;
            }

            public override string Initialize()
            {
                // Create collision geometry for the marine
                float size = (float)Application.AppReference.Random.NextDouble() * SizeVariation + BaseSize;
                _Geometry = new Geometry(this, new Vector2(), size, size, 0.0f, size);

                // Create an animation set for the marine
                _Animations = new AnimationSet();

                // Add the default animation
                Animation normal = new Animation("alien_blood", "Normal", 1, 4, 4.5f);
                normal.Loop = 1;
                _Animations.AddAnimation(normal);
                _Animations.PlayAnimation("Normal");

                // Set crosshair to front of screen
                _Depth = 0.75f;

                //_DynamicLighting = true;

                _Geometry.Direction = Application.AppReference.Random.NextDouble() * Math.PI * 2;
                _Speed = (float)Application.AppReference.Random.NextDouble() * SpeedVariation + SpeedBase;

                // Return the name for this class
                return "AlienBlood";
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
}
