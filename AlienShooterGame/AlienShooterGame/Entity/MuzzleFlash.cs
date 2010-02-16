using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class MuzzleFlash : Entity
    {
        public static float Speed = 1.5f;
        public static int LifeTime = 5;

        protected int _Remaining = LifeTime;

        public MuzzleFlash(Screen parent, Vector2 position, double direction)
            : base(parent)
        {
            _Geometry.Position = position;
            _Geometry.Direction = direction;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateRectangularGeometry(this, 74.0f, 20.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            Animation normal = new Animation("muzzle", "Normal", 1, 3, 15.0f);
            normal.Loop = 1;
            _Animations.AddAnimation(normal);
            _Animations.PlayAnimation("Normal");

            // Set crosshair to front of screen
            _Depth = 0.18f;

            // Return the name for this class
            return "MuzzleFlash";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            _Remaining--;

            if (_Remaining < 0) Dispose();
        }
    }
}
