using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SituationSticky
{
    public class Crosshair : Entity
    {
        #region Constants

        public const float      CrosshairWidth          = 50f;
        public const float      CrosshairHeight         = 28f;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new crosshair object.
        /// </summary>
        /// <param name="parent">Screen for the crosshair.</param>
        public Crosshair(Screen parent) : base(parent.Entities, new Vector2(), CrosshairWidth, CrosshairHeight, 0f) { }

        public override string Initialize()
        {
            // Animstions
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/Misc/Crosshair01_1x1", "Normal", 1, 1, 8.0f));

            // Settings
            _Depth = 0.1f;
            _CollisionType = CollisionType.None;

            // Return the name for this class
            return "Crosshair";
        }

        #endregion

        #region Update

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
 	        base.Update(time);

            // Set location of the crosshair to the mouse position.
            MouseState mState = Mouse.GetState();
            _Position.X = mState.X / _Parent.Manager.Resolution.X * _Parent.ViewPort.Size.X + _Parent.ViewPort.ActualLocation.X;
            _Position.Y = mState.Y / _Parent.Manager.Resolution.Y * _Parent.ViewPort.Size.Y + _Parent.ViewPort.ActualLocation.Y;
        }

        #endregion
    }
}
