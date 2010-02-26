using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            _Geometry = new Geometry(this, new Vector2(), 20.0f, 20.0f, 0.0f, 0.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            Animation normal = new Animation("muzzle_flash", "Normal", 1, 5, 10.0f);
            normal.Loop = 1;
            _Animations.AddAnimation(normal);
            _Animations.PlayAnimation("Normal");

            // Set crosshair to front of screen
            _Depth = 0.18f;

            _ColourOverlay = new Color(1.0f, 0.9f, 0.7f, 0.6f);

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
