﻿using System;
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
        public Vector2 Size { get { return _Size; } set { _Size = value; } }
        protected Vector2 _Size = new Vector2(1024, 576);

        /// <summary>
        /// The exact location of the top left corner of the screen in game units.
        /// </summary>
        public Vector2 ActualLocation { get { return _TargetLocation + _EffectOffset; } }

        /// <summary>
        /// The amount the screen has moved from its target location due to some sort of graphics effect such as screen
        /// shaking.
        /// </summary>
        public Vector2 EffectOffset { get { return _EffectOffset; } set { _EffectOffset = value; } }
        protected Vector2 _EffectOffset = new Vector2(0, 0);

        /// <summary>
        /// The location of the top left of the screen before EffectOffset is applied.
        /// </summary>
        public Vector2 TargetLocation { get { return _TargetLocation; } set { _TargetLocation = value; } }
        protected Vector2 _TargetLocation = new Vector2(0, 0);

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
            _TargetLocation = position;
            _Size = size;
        }

        public virtual void Update(GameTime time)
        {
            if (_ShakeDamping < 0.001f)
            {
                _ShakeMagnitude = 0;
                _EffectOffset = new Vector2(0, 0);
            }
            if (_ShakeLeft)
            {
                if (_EffectOffset.X < -_ShakeMagnitude * _ShakeDamping)
                {
                    _ShakeLeft = false;
                    _EffectOffset.X = -_ShakeMagnitude * _ShakeDamping;
                    _ShakeDamping *= _ShakeDamping;
                }
                else
                    _EffectOffset.X -= _ShakeMagnitude * _ShakeRate;
            }
            else
            {
                if (_EffectOffset.X > _ShakeMagnitude * _ShakeDamping)
                {
                    _ShakeLeft = true;
                    _EffectOffset.X = _ShakeMagnitude * _ShakeDamping;
                    _ShakeDamping *= _ShakeDamping;
                }
                else
                    _EffectOffset.X += _ShakeMagnitude * _ShakeRate;
            }

            if (_SlideTimeLeft > 0.0f)
            {
                _EffectOffset = (_SlideFrom - _TargetLocation) * _SlideTimeLeft / _SlideTime;
                //_EffectOffset -= (_SlideFrom - _TargetLocation) * (float)time.ElapsedGameTime.Milliseconds / 1000.0f / _SlideTime;
                _SlideTimeLeft -= (float)time.ElapsedGameTime.Milliseconds / 1000.0f;

                if (_SlideTimeLeft < 0.0f)
                {
                    _IsSliding = false;
                    _EffectOffset = new Vector2(0, 0);
                }
            }
                
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
            _TargetLocation = to;
            _SlideTimeLeft = time;
            _EffectOffset = _SlideFrom - _TargetLocation;
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
