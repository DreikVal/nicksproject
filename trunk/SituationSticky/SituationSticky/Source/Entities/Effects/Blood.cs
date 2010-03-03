using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    class Blood : Entity_3D
    {
        #region Members

        // Defining features of the blood
        protected float _BaseSize, _SizeVar, _BaseSpeed, _SpeedVar, _SpeedDamp;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new blood particle.
        /// </summary>
        /// <param name="parent">The screen.</param>
        /// <param name="position">The location to spawn at.</param>
        /// <param name="overlay">The colour of the blood.</param>
        /// <param name="baseSize">The minimum size of the blood.</param>
        /// <param name="sizeVar">The random variance in blood size.</param>
        /// <param name="baseSpeed">The minimum velocity of the blood.</param>
        /// <param name="speedVar">The random variance in blood velocity.</param>
        /// <param name="speedDamp">The factor by which blood maintains its velocity.</param>
        /// <param name="lifeTime">The time in milliseconds until the blood disappears.</param>
        public Blood(Screen parent, Vector3 position, Color overlay, float baseSize, float sizeVar, float baseSpeed, float speedVar, float speedDamp, int lifeTime)
            : base(parent.Blood, position, new Vector3(20, 20, 20), Vector3.Zero)
        {
            _ColourOverlay = overlay;
            _BaseSize = baseSize;
            _SizeVar = sizeVar;
            _BaseSpeed = baseSpeed;
            _SpeedVar = speedVar;
            _SpeedDamp = speedDamp;
            _LifeTime = lifeTime;
            float size = (float)Application.AppReference.Random.NextDouble() * _SizeVar + _BaseSize;
            _Size = new Vector3(size, size, size);
            _Direction.Z = (float)(Application.AppReference.Random.NextDouble() * Math.PI * 2);
            _Direction.Y = (float)(0);
            _Direction.X = (float)(Application.AppReference.Random.NextDouble() * Math.PI / 3 + Math.PI / 6);
            _Speed = (float)Application.AppReference.Random.NextDouble() * _SpeedVar + _BaseSpeed;
            _Spin.X = -0.002f;
        }

        public override string Initialize()
        {
            base.Initialize();

            // Animations
            _Model = Application.AppReference.Content.Load<Model>("Models/Effects/Blood02");
            _ModelScale = 0.002f;
            _ModelRotation = new Vector3((float)Math.PI / 2, 0, 0);

            // Entity settings
            _Depth = 0.00f;
            _DynamicLighting = false;
            _Temporary = true;

            return "Blood";
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);
            _Speed *= _SpeedDamp;
            if (_Direction.X < -Math.PI / 2 + 0.1) _Spin = Vector3.Zero;
            if (_Direction.X < 0) _Speed *= 1.1f;
        }

        #endregion
    }
}
