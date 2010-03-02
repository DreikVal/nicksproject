using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SituationSticky
{
    public class Crosshair : Entity_Quad
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
        public Crosshair(Screen parent) : base(parent.Entities, new Vector3(), new Vector3(50, 28, 0), Vector3.Zero) { }

        public override string Initialize()
        {
            base.Initialize();

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
            Vector3 diff = _Parent.ViewPort.Location - _Parent.ViewPort.LookAt;
            float size = diff.Z * (float)Math.Sin(_Parent.ViewPort.FieldOfView);
            _Position.X = mState.X / _Parent.Manager.Resolution.X * size + _Parent.ViewPort.Location.X - size / 2;
            _Position.Y = -mState.Y / _Parent.Manager.Resolution.Y * size + _Parent.ViewPort.Location.Y + size / 2;
            _Position.Z = 100f;
        }

        #endregion
    }
}
