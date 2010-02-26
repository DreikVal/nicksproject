using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SituationSticky
{
    /// <summary>
    /// The bind class represents a pairing between a bind name and a keyboard key or mouse button. It also saves the last recorded
    /// KeyState of that binding (KeyUp or KeyDown)
    /// </summary>
    public class Bind
    {
        #region Members

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

        /// <summary>
        /// Gets or sets the mouse button for this bind.
        /// </summary>
        public MouseButtons MouseButton { get { return _MouseButton; } }
        protected MouseButtons _MouseButton;

        /// <summary>
        /// Gets whether or not this bind is a mouse bind. (If not, it is a keyboard bind.)
        /// </summary>
        public bool MouseBind { get { return _MouseBind; } }
        protected bool _MouseBind;

        /// <summary>
        /// Gets or sets the keystate of this binding.
        /// </summary>
        public KeyState State { get { return _State; } set { _State = value; } }
        protected KeyState _State;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new key binding for a keyboard buton.
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

        /// <summary>
        /// Creates a new bind for a mouse button.
        /// </summary>
        /// <param name="name">Binding name.</param>
        /// <param name="mouse">The mouse button.</param>
        public Bind(String name, MouseButtons mouse)
        {
            _Name = name;
            _MouseButton = mouse;
            _State = KeyState.Up;
            _MouseBind = true;
        }

        #endregion
    }

    public enum MouseButtons
    {
        LeftButton,
        RightButton,
        MiddleButton,
        Button4,
        Button5,
        ScrollUp,
        ScrollDown
    }
}
