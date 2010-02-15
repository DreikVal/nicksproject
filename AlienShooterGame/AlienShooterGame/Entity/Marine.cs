using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    class Marine : Entity
    {
        public float Speed { get { return _Speed; } set { _Speed = value; } }
        protected float _Speed = 5.0f;

        public bool MoveForward { get { return _MoveForward; } set { _MoveForward = value; } }
        protected bool _MoveForward = false;

        public Marine(Screen parent) : base(parent) { }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            Geometry = Geometry.CreateCircularGeometry(this, 20.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("soldier", "Normal", 1, 1, 10.0f));

            // Return the name for this class
            return "Marine";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);
            if (_MoveForward)
            {               
                //Geometry.Position.X += Speed * (float)Math.Sin(Geometry.Direction);
                //Geometry.Position.Y += Speed * (float)Math.Cos(Geometry.Direction);
            }
        }
    }
}
