using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AlienShooterGame
{
    class Crosshair : Entity
    {
        public Crosshair(Screen parent) : base(parent) { }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 28.0f, 50.0f, 0.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("crosshair", "Normal", 1, 1, 8.0f));

            // Set crosshair to front of screen
            _Depth = 0.1f;

            // Return the name for this class
            return "Crosshair";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            MouseState mState = Mouse.GetState();

            _Geometry.Position.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            _Geometry.Position.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;
        }
    }
}
