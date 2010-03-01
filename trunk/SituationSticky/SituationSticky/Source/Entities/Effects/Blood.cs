using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    class Blood : Entity
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
            : base(parent.Blood, position, new Vector3(20,20,20), Vector2.Zero)
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
            _Direction.X = (float)(Application.AppReference.Random.NextDouble() * Math.PI * 2);
            _Speed = (float)Application.AppReference.Random.NextDouble() * _SpeedVar + _BaseSpeed;
        }

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/Effects/Blood01_1x7", "Normal", 1, 7, 6.0f));

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
        }

        #endregion
    }
}
