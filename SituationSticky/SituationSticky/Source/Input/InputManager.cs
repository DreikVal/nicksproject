using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SituationSticky
{
    

    public class InputManager
    {
        #region Members

        /// <summary>
        /// This event is fired when a binding's state changes (from KeyUp to KeyDown)
        /// </summary>
        /// <param name="bind">The bind that has changed.</param>
        public event StateChangedEventHandler StateChanged;
        public delegate void StateChangedEventHandler(Bind bind);

        /// <summary>
        /// We use a dictionary to store the list of user binds.
        /// </summary>
        protected ThreadDictionary<string, Bind> _Binds;

        /// <summary>
        /// Gets or sets whether the movement style is absolute or relative
        /// </summary>
        public bool AbsoluteMovement { get { return _AbsoluteMovement; } set { _AbsoluteMovement = value; } }
        protected bool _AbsoluteMovement = true;

        /// <summary>
        /// We use this to track the previous mouse wheel position to track changes.
        /// </summary>
        protected int _ScrollValue;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates an input manager to monitor the keyboard state.
        /// </summary>
        public InputManager()
        {
            _Binds = new ThreadDictionary<string, Bind>();
            _ScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the keyboard state and fires any necessary binding events.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        public virtual void Update(GameTime time) 
        {
            KeyboardState keystate = Keyboard.GetState();
            MouseState mState = Mouse.GetState();
            _Binds.ForEach(ForEachUpdate, keystate, mState, null);
            _ScrollValue = mState.ScrollWheelValue;
        }
        private bool ForEachUpdate(Bind bind, object keystate, object mState, object p3)
        {
            if (bind.MouseBind)
            {
                MouseState state = (MouseState)mState;

                // Check left mouse button
                if (bind.MouseButton == MouseButtons.LeftButton)
                {
                    if (state.LeftButton == ButtonState.Pressed && bind.State == KeyState.Up)
                    {
                        bind.State = KeyState.Down;
                        OnStateChanged(bind);
                    }
                    else if (state.LeftButton == ButtonState.Released && bind.State == KeyState.Down)
                    {
                        bind.State = KeyState.Up;
                        OnStateChanged(bind);
                    }
                }
                // Check right mouse button
                else if (bind.MouseButton == MouseButtons.RightButton)
                {
                    if (state.RightButton == ButtonState.Pressed && bind.State == KeyState.Up)
                    {
                        bind.State = KeyState.Down;
                        OnStateChanged(bind);
                    }
                    else if (state.RightButton == ButtonState.Released && bind.State == KeyState.Down)
                    {
                        bind.State = KeyState.Up;
                        OnStateChanged(bind);
                    }
                }
                // Check middle mouse button
                else if (bind.MouseButton == MouseButtons.MiddleButton)
                {
                    if (state.MiddleButton == ButtonState.Pressed && bind.State == KeyState.Up)
                    {
                        bind.State = KeyState.Down;
                        OnStateChanged(bind);
                    }
                    else if (state.MiddleButton == ButtonState.Released && bind.State == KeyState.Down)
                    {
                        bind.State = KeyState.Up;
                        OnStateChanged(bind);
                    }
                }
                // Check scroll up
                else if (bind.MouseButton == MouseButtons.ScrollUp)
                {
                    if (_ScrollValue < state.ScrollWheelValue)
                        OnStateChanged(bind);
                }
                // Check scroll down
                else if (bind.MouseButton == MouseButtons.ScrollDown)
                {
                    if (_ScrollValue > state.ScrollWheelValue)
                        OnStateChanged(bind);
                }
            }
            else
            {
                // Check keyboard binds
                KeyboardState state = (KeyboardState)keystate;
                if (state.IsKeyDown(bind.Key) && bind.State == KeyState.Up)
                {
                    bind.State = KeyState.Down;
                    OnStateChanged(bind);
                }
                else if (state.IsKeyUp(bind.Key) && bind.State == KeyState.Down)
                {
                    bind.State = KeyState.Up;
                    OnStateChanged(bind);
                }
            }
            return true;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Adds a binding to the list to be monitored.
        /// </summary>
        /// <param name="bind">The new key binding to monitor.</param>
        public virtual void AddBind(Bind bind)
        {
            _Binds.Add(bind.Name, bind);
        }

        /// <summary>
        /// Removes a binding from the list.
        /// </summary>
        /// <param name="name">The name of the bind to be removed.</param>
        public virtual void RemoveBind(String name)
        {
            _Binds.Remove(name);
        }

        /// <summary>
        /// Attempts to find the binding for the given binding name.
        /// </summary>
        /// <param name="name">The name of the binding to lookup.</param>
        /// <param name="bind">The binding where the result will be stored if it finds the bind.</param>
        /// <returns>True if the binding was found and false otherwise.</returns>
        public virtual bool LookupBind(String name, out Bind bind)
        {
            return _Binds.TryGetValue(name, out bind);
        }

        /// <summary>
        /// Fires the state changed event for a given binding.
        /// </summary>
        /// <param name="b">The binding who's state was changed.</param>
        protected virtual void OnStateChanged(Bind b)
        {
            if (StateChanged != null)
                StateChanged(b);
        }

        #endregion
    }
}
