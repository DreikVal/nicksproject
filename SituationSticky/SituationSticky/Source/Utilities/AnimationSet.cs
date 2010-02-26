using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SituationSticky
{
    public class AnimationSet
    {
        /// <summary>
        /// A set of animations that belong to this entity.
        /// </summary>
        public Dictionary<String, Animation> Animations { get { return _Animations; } }
        protected Dictionary<String, Animation> _Animations = null;

        public Animation Current { get { return _Current; } }
        protected Animation _Current = null;

        public event AnimationFinishedEventHandler AnimationFinished;
        public delegate void AnimationFinishedEventHandler(AnimationSet set, Animation animation);

        public AnimationSet()
        {
        }

        public virtual void AddAnimation(Animation animation)
        {
            if (_Current == null)
            {
                _Current = animation;
                //_Current.FreezeFrame = true;
            }
            else
            {
                if (_Animations == null)
                    _Animations = new Dictionary<string, Animation>();
                _Animations.Add(animation.AnimationName, animation);
            }
        }

        public virtual void PlayAnimation(String animationName)
        {
            if (_Current.AnimationName.CompareTo(animationName) == 0) { }
            else if (_Animations != null)
            {
                Animation animation;
                if (_Animations.TryGetValue(animationName, out animation))
                    _Current = animation;
                else
                    throw new Exception("Could not locate animation: " + animationName);
            }
            else return;
            _Current.Play();
            _Current.AnimationFinished += OnAnimationFinished;
        }

        private void OnAnimationFinished(Animation animation)
        {
            _Current.AnimationFinished -= OnAnimationFinished;
            if (AnimationFinished != null) 
                AnimationFinished(this, _Current);
        }
    }
}
