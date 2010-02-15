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
    public class Animation
    {
        /// <summary>
        /// This event is fired when an animation has finished playing and the animation has entered a freezeframe state.
        /// </summary>
        public event AnimationFinishedEventHandler AnimationFinished;
        public delegate void AnimationFinishedEventHandler(Animation animation);

        /// <summary>
        /// The image file for the texture or animation.
        /// </summary>
        public Texture2D Texture { get { return _Texture; } set { _Texture = value; } }
        protected Texture2D _Texture = null;

        public int WidthPerCell { get { return _Texture.Width / _Columns; } }

        public int HeightPerCell { get { return _Texture.Height / _Rows; } }

        /// <summary>
        /// The number of rows in the animation.
        /// </summary>
        public int Rows { get { return _Rows; } }
        protected int _Rows = 1;

        /// <summary>
        /// The number of columns in the animation.
        /// </summary>
        public int Columns { get { return _Columns; } }
        protected int _Columns = 1;

        /// <summary>
        /// The number of frames in the animation.
        /// </summary>
        public int Frames { get { return _Rows * _Columns; } }

        /// <summary>
        /// The number of frames per second to play for an animation.
        /// </summary>
        public float FrameRate { get { return _FrameRate; } set { _FrameRate = value; } }
        protected float _FrameRate = 10.0f;

        /// <summary>
        /// The number of times that the animation should be played. 1 --> Play Once, 2 --> Twice etc..
        /// 0 --> Loop forever.
        /// </summary>
        public int Loop { get { return _Loop; } set { _Loop = value; } }
        protected int _Loop = 0;
        protected int _PlayCount = 0;

        /// <summary>
        /// Gets or sets whether the animation should be frozen.
        /// </summary>
        public bool FreezeFrame { get { return _FreezeFrame; } set { _FreezeFrame = value; } }
        protected bool _FreezeFrame = false;

        /// <summary>
        /// Gets the name of this animation.
        /// </summary>
        public String AnimationName { get { return _AnimationName; } }
        protected String _AnimationName = "Animation";

        protected int _NextFrame;
        protected int _CurrentFrame = 0;


        public Animation(String textureName, String animationName, int rows, int cols, float framerate)
        {
            _Texture = Application.AppReference.Content.Load<Texture2D>(textureName);
            _Rows = rows;
            _Columns = cols;
            _FrameRate = framerate;
            _AnimationName = animationName;
            _NextFrame = (int)(1000.0f / _FrameRate);
        }

        public virtual void Play()
        {
            _PlayCount = 0;
            _CurrentFrame = 0;
            _FreezeFrame = false;
            _NextFrame = (int)(1000.0f / _FrameRate);
        }

        public virtual Rectangle UpdateSource(GameTime time)
        {
            // Update time till next frame
            if (!_FreezeFrame)
            {
                _NextFrame -= time.ElapsedGameTime.Milliseconds;
                if (_NextFrame < 0)
                {
                    _CurrentFrame++;
                    _NextFrame += (int)(1000.0f / _FrameRate);

                    if (_CurrentFrame >= Frames)
                    {
                        _PlayCount++;
                        if (_PlayCount < _Loop || _Loop == 0)
                            _CurrentFrame = 0;
                        else
                        {
                            _CurrentFrame = Frames - 1;
                            _FreezeFrame = true;
                            if (AnimationFinished != null)
                                AnimationFinished(this);
                        }
                    }
                }
            }

            // Generate source rectangle
            int width = _Texture.Width / _Columns;
            int height = _Texture.Height / _Rows;
            return new Rectangle(width * (_CurrentFrame % _Columns), height * (_CurrentFrame / _Columns), width, height);
        }
    }
}
