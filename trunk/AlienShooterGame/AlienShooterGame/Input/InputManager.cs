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

namespace AlienShooterGame
{
    

    public class InputManager
    {
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
        /// Creates an input manager to monitor the keyboard state.
        /// </summary>
        public InputManager()
        {
            _Binds = new ThreadDictionary<string, Bind>();
        }

        /// <summary>
        /// Adds a binding to the list to be monitored.
        /// </summary>
        /// <param name="bind">The new key binding to monitor.</param>
        public virtual void AddBind(Bind bind)
        {
            _Binds.Add(bind.Name, bind);
        }

        /// <summary>
        /// Updates the keyboard state and fires any necessary binding events.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        public virtual void Update(GameTime time) 
        {
            _Binds.ForEach(ForEachUpdate, Keyboard.GetState(), null, null);
        }
        private bool ForEachUpdate(Bind bind, object keystate, object p2, object p3)
        {
            if (bind.MouseBind)
            {
                MouseState state = Mouse.GetState();
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
                
            }
            else
            {
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
    }

    /// <summary>
    /// The bind class represents a pairing between a bind name and a keyboard key. It also saves the last recorded
    /// KeyState of that binding (KeyUp or KeyDown)
    /// </summary>
    public class Bind
    {
        /// <summary>
        /// Gets the name of this binding.
        /// </summary>
        public String Name { get { return _Name; } }
        protected String _Name;

        /// <summary>
        /// Gets or sets the key for this binding.
        /// </summary>
        public Keys Key { get { return _Key; } set { _Key = value; } }
        protected Keys _Key;

        public MouseButtons MouseButton { get { return _MouseButton; } }
        protected MouseButtons _MouseButton;

        public bool MouseBind { get { return _MouseBind; } }
        protected bool _MouseBind;

        /// <summary>
        /// Gets or sets the keystate of this binding.
        /// </summary>
        public KeyState State { get { return _State; } set { _State = value; } }
        protected KeyState _State;

        /// <summary>
        /// Creates a new key binding object.
        /// </summary>
        /// <param name="name">Binding name.</param>
        /// <param name="key">The bound key.</param>
        public Bind(String name, Keys key)
        {
            _Name = name;
            _Key = key;
            _State = KeyState.Up;
            _MouseBind = false;
        }

        public Bind(String name, MouseButtons mouse)
        {
            _Name = name;
            _MouseButton = mouse;
            _State = KeyState.Up;
            _MouseBind = true;
        }
    }

    public enum MouseButtons
    {
        LeftButton,
        RightButton,
        MiddleButton,
        Button4,
        Button5
    }
}
