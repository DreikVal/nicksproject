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
    public class ScreenManager
    {
        /// <summary>
        /// This event is fired when a screen is removed from the screen manager.
        /// </summary>
        /// <param name="screen">The screen being removed.</param>
        public event ScreenRemovedEventHandler ScreenRemoved;
        public delegate void ScreenRemovedEventHandler(Screen screen);

        /// <summary>
        /// This event is fired when the screen manager recieves input from the input manager.
        /// </summary>
        /// <param name="bind">The key binding who's state has changed.</param>
        public event InputRecievedEventHandler InputRecieved;
        public delegate void InputRecievedEventHandler(Bind bind);

        /// <summary>
        /// This event is fired when a screen is added to the screen manager.
        /// </summary>
        /// <param name="screen">The screen being added.</param>
        public event ScreenAddedEventHandler ScreenAdded;
        public delegate void ScreenAddedEventHandler(Screen screen);

        /// <summary>
        /// The default horizontal screen resolution.
        /// </summary>
        public const int DefaultHorizontalResolution = 1600;

        /// <summary>
        /// The default horizontal to vertical screen ratio.
        /// </summary>
        public const float DefaultScreenRatio = 16f / 10f;

        /// <summary>
        /// Gets or sets the screen resolution for this game.
        /// </summary>
        public Vector2 Resolution { get { return _Resolution; } set { SetResolution(value); } }
        protected Vector2 _Resolution;

        /// <summary>
        /// The collection of screens.
        /// </summary>
        protected ThreadDictionary<string, Screen> _Screens;

        /// <summary>
        /// Gets a reference to the input manager which handles input events.
        /// </summary>
        public InputManager Input { get { return _Input; } }
        protected InputManager _Input;


        /// <summary>
        /// Creates a new screen manager.
        /// </summary>
        public ScreenManager()
        {
            // Setup input
            _Input = new InputManager();
            _Input.AddBind(new Bind("down", Keys.Down));
            _Input.AddBind(new Bind("up", Keys.Up));
            _Input.AddBind(new Bind("enter", Keys.Enter));
            _Input.AddBind(new Bind("back", Keys.Escape));
            _Input.StateChanged += OnInput;

            // Setup screens
            _Screens = new ThreadDictionary<string,Screen>();
            _Screens.QueuesEmptied += DepthCheck;
            SetResolution(new Vector2(DefaultHorizontalResolution, DefaultHorizontalResolution / DefaultScreenRatio));

            // Register events
            _Screens.ItemAdded += OnScreenAdded;
            _Screens.ItemRemoved += OnScreenRemoved;
        }

        /// <summary>
        /// Fires the screen added event.
        /// </summary>
        /// <param name="s">The screen being added.</param>
        private void OnScreenAdded(Screen s)
        {
            if (ScreenAdded != null)
                ScreenAdded(s);
        }

        /// <summary>
        /// Fires the screen removed event.
        /// </summary>
        /// <param name="s">The screen being removed.</param>
        private void OnScreenRemoved(Screen s)
        {
            if (ScreenRemoved != null)
                ScreenRemoved(s);
        }

        /// <summary>
        /// Fires the input recieved event.
        /// </summary>
        /// <param name="bind">The key binding who's state has changed.</param>
        protected virtual void OnInput(Bind bind)
        {
            if (InputRecieved != null)
                InputRecieved(bind);
            _Screens.ForEach(ForEachInput, bind, null, null);
        }
        private object ForEachInput(Screen screen, object bind, object p2, object p3)
        {
            screen.HandleInput(bind as Bind); return null;
        }


        /// <summary>
        /// Updates all screens in the screen manager.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        public virtual void Update(GameTime time)
        {
            // Update input manager
            _Input.Update(time);

            // Update screens
            _Screens.ForEach(ForEachUpdate, time, null, null);
        }
        private object ForEachUpdate(Screen screen, object time, object p3, object p4)
        {
            screen.Update(time as GameTime);
            return null;
        }


        /// <summary>
        /// Draws all the screens.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        /// <param name="batch">The spritebatch on which to draw.</param>
        public virtual void Draw(GameTime time, SpriteBatch batch)
        {            
            _Screens.ForEach(ForEachDraw, time, batch, null);
        }
        private object ForEachDraw(Screen screen, object time, object batch, object p4)
        {
            if (screen.Visible)
            {
                SpriteBatch bat = batch as SpriteBatch;
                screen.BeginDraw(bat);
                screen.Draw(time as GameTime, bat);
                screen.EndDraw(bat);
            }
            return null;
        }


        /// <summary>
        /// Sets the resolution vector and registers the change with the graphics device.
        /// </summary>
        /// <param name="resolution">A vector containing horizontal and vertical resolution.</param>
        private void SetResolution(Vector2 resolution)
        {
            _Resolution = resolution;
            Application.AppReference.GraphicsManager.PreferredBackBufferWidth = (int)resolution.X;
            Application.AppReference.GraphicsManager.PreferredBackBufferHeight = (int)resolution.Y;
        }


        /// <summary>
        /// Removes the specified screen from the game.
        /// </summary>
        /// <param name="screen">The screen to be removed.</param>
        public virtual void RemoveScreen(Screen screen)
        {
            RemoveScreen(screen.Name);
        }

        /// <summary>
        /// Removes the screen with the specified name from the game.
        /// </summary>
        /// <param name="screen_name">The screen name of the screen to be removed.</param>
        public virtual void RemoveScreen(string screen_name)
        {
            _Screens.Remove(screen_name);
            DepthCheck();
        }

        /// <summary>
        /// Attempts to find a screen via its name.
        /// </summary>
        /// <param name="name">The name of the screen to lookup.</param>
        /// <param name="screen">The variable to store the screen result in.</param>
        /// <returns>Returns true if the screen was found, false otherwise.</returns>
        public virtual bool LookupScreen(string name, out Screen screen)
        {
            return _Screens.TryGetValue(name, out screen);
        }

        /// <summary>
        /// Adds the specified screen to the screen manager.
        /// </summary>
        /// <param name="screen">The screen to be added.</param>
        public virtual void AddScreen(Screen screen)
        {
            _Screens.Add(screen.Name, screen);
            DepthCheck();
        }

        /// <summary>
        /// This method is called when a variable update has been recieved through the network.
        /// </summary>
        /// <param name="entityID">The ID of the entity who's variable has been updated.</param>
        /// <param name="varID">The ID of the variable.</param>
        /// <param name="value">The new value for the variable.</param>
        public virtual void UpdateVariable(UInt64 entityID, UInt16 varID, object value)
        {

        }

        /// <summary>
        /// Obscures all screens visibility.
        /// </summary>
        protected virtual void ObscureAllVisibily() { _Screens.ForEach(ForEachObscureAllVisibly, null, null, null); }
        private object ForEachObscureAllVisibly(Screen screen, object p1, object p2, object p3) { screen.ObscureVisibility(); return null; }

        /// <summary>
        /// Obscures all screens input.
        /// </summary>
        protected virtual void ObscureAllInput() { _Screens.ForEach(ForEachObscureAllInput, null, null, null); }
        private object ForEachObscureAllInput(Screen screen, object p1, object p2, object p3) { screen.ObscureInput(); return null; }

        /// <summary>
        /// Obscures all screens updates.
        /// </summary>
        protected virtual void ObscureAllUpdates() { _Screens.ForEach(ForEachObscureAllUpdates, null, null, null); }
        private object ForEachObscureAllUpdates(Screen screen, object p1, object p2, object p3) { screen.ObscureUpdates(); return null; }

        public virtual void DepthCheck()
        {
            VisibilityCheck();
            InputCheck();
            UpdateCheck();
        }

        /// <summary>
        /// Calculates which screens should be visible and which should be visibly obscured.
        /// </summary>
        public virtual void VisibilityCheck()
        {
            vblocker = null;
            _Screens.ForEach(ForEachVisBlocker, null, null, null);
            ObscureAllVisibily();
            _Screens.ForEach(ForEachVisPopup, null, null, null);
            if (vblocker != null)
                vblocker.ActivateVisibility();
        }
        private Screen vblocker;
        private object ForEachVisBlocker(Screen screen, object p1, object p2, object p3)
        {
            if (!screen.BlocksVisibility) return null;
            if (vblocker == null)
                vblocker = screen;
            else
            {
                if (screen.Depth < vblocker.Depth)
                    vblocker = screen;
            }
            return null;
        }
        private object ForEachVisPopup(Screen screen, object p1, object p2, object p3)
        {
            if (vblocker == null) { screen.ActivateVisibility(); return null; }
            if (screen.BlocksVisibility) return null;
            if (screen.Depth < vblocker.Depth)
                screen.ActivateVisibility();
            return null;
        }

        /// <summary>
        /// Calculates which screens should receive input and which should be input obscured.
        /// </summary>
        public virtual void InputCheck()
        {
            iblocker = null;
            _Screens.ForEach(ForEachInputBlocker, null, null, null);
            ObscureAllInput();
            _Screens.ForEach(ForEachInputPopup, null, null, null);
            if (iblocker != null)
                iblocker.ActivateInput();
        }
        private Screen iblocker;
        private object ForEachInputBlocker(Screen screen, object p1, object p2, object p3)
        {
            if (!screen.BlocksInput) return null;
            if (iblocker == null)
                iblocker = screen;
            else
            {
                if (screen.Depth < iblocker.Depth)
                    iblocker = screen;
            }
            return null;
        }
        private object ForEachInputPopup(Screen screen, object p1, object p2, object p3)
        {
            if (iblocker == null) { screen.ActivateInput(); return null; }
            if (screen.BlocksInput) return null;
            if (screen.Depth < iblocker.Depth)
                screen.ActivateInput();
            return null;
        }

        /// <summary>
        /// Calculates which screens should receive updates and which should be update obscured.
        /// </summary>
        public virtual void UpdateCheck()
        {
            ublocker = null;
            _Screens.ForEach(ForEachUpdateBlocker, null, null, null);
            ObscureAllUpdates();
            _Screens.ForEach(ForEachUpdatePopup, null, null, null);
            if (ublocker != null)
                ublocker.ActivateUpdates();
        }
        private Screen ublocker;
        private object ForEachUpdateBlocker(Screen screen, object p1, object p2, object p3)
        {
            if (!screen.BlocksUpdates) return null;
            if (ublocker == null)
                ublocker = screen;
            else
            {
                if (screen.Depth < ublocker.Depth)
                    ublocker = screen;
            }
            return null;
        }
        private object ForEachUpdatePopup(Screen screen, object p1, object p2, object p3)
        {
            if (ublocker == null) { screen.ActivateUpdates(); return null; }
            if (screen.BlocksUpdates) return null;
            if (screen.Depth < ublocker.Depth)
                screen.ActivateUpdates();
            return null;
        }

    }
}
