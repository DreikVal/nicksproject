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
        public const int BloodPerHit = 9;
        public const int BloodPerDeath = 24;
        private const int alienHurt = 2;

        public int ScoreValue = 50;

        public Entity Target { get { return _Target; } set { _Target = value; } } 
        protected Entity _Target;

        public const float AlienSpeed = 0.15f;

        protected int _MaxHP = 100;
        protected int _CurrentHP;
        protected float _Speed = AlienSpeed;

        public Alien(Screen parent, Vector2 position, Entity target) : base(parent) 
        {
            _Geometry.Position = position;
            _Target = target;
            _CurrentHP = _MaxHP;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 44.0f, 44.0f, 0.0f, 16.0f);

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

        public override void Update(GameTime time)
        {
 	        base.Update(time);

            if (Disposed) return;

            if (_CurrentHP <= 0)
                Dispose();

            if (_Target.Disposed) _Target = ((WorldScreen)_Parent).Player;

            float x_diff = _Geometry.Position.X - _Target.Geometry.Position.X;
            float y_diff = _Geometry.Position.Y - _Target.Geometry.Position.Y;
            _Geometry.Direction = Math.Atan2(y_diff, x_diff) - Math.PI/2;

            if (_Speed < AlienSpeed)
                _Speed *= 1.01f;

            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
        }

        protected override void HandleCollision(Entity otherEnt)
        {
            base.HandleCollision(otherEnt);

            if (otherEnt as Bullet != null)
            {
                int damage = 15 + Application.AppReference.Random.Next(25);
                _CurrentHP -= damage;
                for (int i = 0; i < BloodPerHit; i++)
                    new Blood(_Parent, _Geometry.Position, Color.Green, 8.0f, 16.0f, 0.1f, 0.2f, 0.92f, 22);
                Vector2 diff = otherEnt.Geometry.Position - Geometry.Position;
                double angle = Math.Atan2(diff.Y, diff.X);
                float knockbackFactor = 0.7f;
                Geometry.Position.X += (float)((Geometry.CollisionRadius + otherEnt.Geometry.CollisionRadius) * Math.Cos(Geometry.Direction - Math.PI) * knockbackFactor);
                Geometry.Position.Y += (float)((Geometry.CollisionRadius + otherEnt.Geometry.CollisionRadius) * -Math.Sin(Geometry.Direction - Math.PI) * knockbackFactor);
                _Speed *= 0.60f;
            }
            else if (otherEnt as Alien != null)
            {
                Vector2 diff = Geometry.Position - otherEnt.Geometry.Position;
                double angle = Math.Atan2(diff.Y, diff.X);
                otherEnt.Geometry.Position.X = (float)(Geometry.Position.X + ((otherEnt.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Cos(angle)));
                otherEnt.Geometry.Position.Y = (float)(Geometry.Position.Y + ((otherEnt.Geometry.CollisionRadius + Geometry.CollisionRadius) * -Math.Sin(angle)));
            }

            else if (otherEnt as Marine != null)
                hurtPlayer((Marine)otherEnt);

        }

        private void hurtPlayer(Marine player)
        {
            double knockbackFactor = 1.2;
            player.CurrentHP -= alienHurt;
            Parent.ViewPort.Shake(3.0f, 0.8f, 0.95f);
            for (int i = 0; i < player.BloodPerHit; i++)
                new Blood(_Parent, _Geometry.Position, Color.Red, 8.0f, 16.0f, 0.1f, 0.2f, 0.92f, 22);

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
                new Blood(_Parent, _Geometry.Position, Color.Green, 8.0f, 32.0f, 0.1f, 0.2f, 0.92f, 25);

            Alien.CreateNearbyAlien(_Parent, ((WorldScreen)_Parent).Player, 450, _Target);
            new FloatingText(_Parent, _Geometry.Position+new Vector2(0f,-20f), 0.12f, 0.95f, ((WorldScreen)_Parent).Player.GiveScore(ScoreValue).ToString(),
                "FloatingFont", new Color(0.6f, 0.7f, 1.0f, 0.7f), 1000);
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
