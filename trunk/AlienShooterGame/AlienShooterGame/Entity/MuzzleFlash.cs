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
        public static int LifeTime = 4;

        protected int _Remaining = LifeTime;
        protected Entity _Owner = null;
        protected Vector2 _Offset;

        public MuzzleFlash(Screen parent, Vector2 position, Entity owner)
            : base(parent)
        {
            _Offset = position - owner.Geometry.Position;
            _Owner = owner;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateRectangularGeometry(this, 74.0f, 20.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            Animation normal = new Animation("muzzle", "Normal", 1, 3, 6.0f);
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

            if (_Remaining-- < 0) Dispose();
            _Geometry.Position = _Owner.Geometry.Position + _Offset;
            _Geometry.Direction = _Owner.Geometry.Direction;
        }
    }
}
