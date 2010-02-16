using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AlienShooterGame
{
    /// <summary>
    /// The viewport is a class that describes where the screen is in world coordinates, which is used for to calculate
    /// the pixel of locations by taking their position and size relative to that of the viewport.
    /// </summary>
    public class ViewPort
    {
        /// <summary>
        /// The size in game units of the screen.
        /// </summary>
        public Vector2 Size = new Vector2(800, 440);

        /// <summary>
        /// The exact location of the top left corner of the screen in game units.
        /// </summary>
        public Vector2 ActualLocation { get { return TargetLocation + EffectOffset; } }

        /// <summary>
        /// The amount the screen has moved from its target location due to some sort of graphics effect such as screen
        /// shaking.
        /// </summary>
        public Vector2 EffectOffset = new Vector2(0, 0);

        /// <summary>
        /// The location of the top left of the screen before EffectOffset is applied.
        /// </summary>
        public Vector2 TargetLocation = new Vector2(0, 0);

        // Variables for handling screen shakes
        protected float _ShakeDamping = 0.90f;
        protected float _ShakeRate = 1.00f;
        protected float _ShakeMagnitude = 0.0f;
        protected bool _ShakeLeft = true;

        // Variables for handling screen slides
        protected Vector2 _SlideFrom = new Vector2(0, 0);
        //protected Vector2 _SlideTo = new Vector2(0, 0);
        protected float _SlideTime = 0.60f;
        protected float _SlideTimeLeft = -0.10f;

        /// <summary>
        /// Gets whether or not the viewport is currently sliding.
        /// </summary>
        public bool IsSliding { get { return _IsSliding; } }
        protected bool _IsSliding = false;

        /// <summary>
        /// Creates a viewport with default position and size.
        /// </summary>
        public ViewPort()
        {
        }

        /// <summary>
        /// Creates a viewport with specified position and size.
        /// </summary>
        /// <param name="position">Location of the top left corner of the screen.</param>
        /// <param name="size">The size in game units of the screen.</param>
        public ViewPort(Vector2 position, Vector2 size)
        {
            TargetLocation = position;
            Size = size;
        }

        public virtual void Update(GameTime time)
        {
            if (_ShakeDamping < 0.001f)
            {
                _ShakeMagnitude = 0;
                EffectOffset = new Vector2(0, 0);
            }
            if (_ShakeLeft)
            {
                if (EffectOffset.X < -_ShakeMagnitude * _ShakeDamping)
                {
                    _ShakeLeft = false;
                    EffectOffset.X = -_ShakeMagnitude * _ShakeDamping;
                    _ShakeDamping *= _ShakeDamping;
                }
                else
                    EffectOffset.X -= _ShakeMagnitude * _ShakeRate;
            }
            else
            {
                if (EffectOffset.X > _ShakeMagnitude * _ShakeDamping)
                {
                    _ShakeLeft = true;
                    EffectOffset.X = _ShakeMagnitude * _ShakeDamping;
                    _ShakeDamping *= _ShakeDamping;
                }
                else
                    EffectOffset.X += _ShakeMagnitude * _ShakeRate;
            }

            if (_SlideTimeLeft > 0.0f)
            {
                EffectOffset = (_SlideFrom - TargetLocation) * _SlideTimeLeft / _SlideTime;
                //_EffectOffset -= (_SlideFrom - _TargetLocation) * (float)time.ElapsedGameTime.Milliseconds / 1000.0f / _SlideTime;
                _SlideTimeLeft -= (float)time.ElapsedGameTime.Milliseconds / 1000.0f;

                if (_SlideTimeLeft < 0.0f)
                {
                    _IsSliding = false;
                    EffectOffset = new Vector2(0, 0);
                }
            }
            else
                _IsSliding = false;
                
        }

        public virtual void Shake(float magnitude, float rate, float damping)
        {
            _ShakeDamping = damping;
            _ShakeMagnitude = magnitude;
            _ShakeRate = rate;
        }

        public virtual void Slide(Vector2 from, Vector2 to, float time)
        {
            _SlideFrom = from;
            _SlideTime = time;
            TargetLocation = to;
            _SlideTimeLeft = time;
            EffectOffset = _SlideFrom - TargetLocation;
            _IsSliding = true;
        }

        /// <summary>
        /// Given the position of some arbitrary entity, this function returns its pixel location on the screen.
        /// </summary>
        /// <param name="position">The position to be converted to pixel coordinates</param>
        /// <returns>Returns the pixel coordinates.</returns>
        public Vector2 Transform_UnitPosition_To_PixelPosition(Vector2 position)
        {
            return (position - ActualLocation) / Size * Application.AppReference.ScreenManager.Resolution;
        }

        /// <summary>
        /// Given an entity size, this function returns its size in terms of pixels on the screen.
        /// </summary>
        /// <param name="size">The size in game units to be converted.</param>
        /// <returns>The size in pixels.</returns>
        public Vector2 Transform_UnitSize_To_PixelSize(Vector2 size)
        {
            return (size / Size * Application.AppReference.ScreenManager.Resolution);
        }
    }
}
