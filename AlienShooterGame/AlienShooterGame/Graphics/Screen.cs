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
    /// <summary>
    /// A screen is a visual element that can be rendered on the user's screen. There can be multiple screens handled by
    /// a screen manager.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// This event is fired when the menu recieves input to process.
        /// </summary>
        /// <param name="bind">The key binding who's state was changed.</param>
        public event InputRecievedEventHandler InputRecieved;
        public delegate void InputRecievedEventHandler(Screen sender, Bind bind);

        /// <summary>
        /// This event is fired when input is recieved but the screen is obscured by another screen.
        /// </summary>
        public event ObscureInputRecievedEventHandler ObscureInputRecieved;
        public delegate void ObscureInputRecievedEventHandler(Screen sender, Bind bind);

        /// <summary>
        /// This event is fired if the current screen becomes visibly obscured by another screen infront of it.
        /// </summary>
        /// <param name="sender">The screen being obscured.</param>
        public event ScreenVisiblyObscuredEventHandler ScreenVisiblyObscured;
        public delegate void ScreenVisiblyObscuredEventHandler(Screen sender);

        /// <summary>
        /// This event is fired if the current screen becomes visibly unobscured.
        /// </summary>
        /// <param name="sender">The screen being unobscured.</param>
        public event ScreenVisiblyActivatedEventHandler ScreenVisiblyActivated;
        public delegate void ScreenVisiblyActivatedEventHandler(Screen sender);

        /// <summary>
        /// This event is fired if the current screen becomes input obscured by another screen infront of it.
        /// </summary>
        /// <param name="sender">The screen being obscured.</param>
        public event ScreenInputObscuredEventHandler ScreenInputObscured;
        public delegate void ScreenInputObscuredEventHandler(Screen sender);

        /// <summary>
        /// This event is fired if the current screen becomes input unobscured.
        /// </summary>
        /// <param name="sender">The screen being unobscured.</param>
        public event ScreenInputActivatedEventHandler ScreenInputActivated;
        public delegate void ScreenInputActivatedEventHandler(Screen sender);

        /// <summary>
        /// This event is fired if the current screen becomes update obscured by another screen infront of it.
        /// </summary>
        /// <param name="sender">The screen being obscured.</param>
        public event ScreenUpdateObscuredEventHandler ScreenUpdateObscured;
        public delegate void ScreenUpdateObscuredEventHandler(Screen sender);

        /// <summary>
        /// This event is fired if the current screen becomes update unobscured.
        /// </summary>
        /// <param name="sender">The screen being unobscured.</param>
        public event ScreenUpdateActivatedEventHandler ScreenUpdateActivated;
        public delegate void ScreenUpdateActivatedEventHandler(Screen sender);

        /// <summary>
        /// This event is fired when the screen is removed from the screen manager.
        /// </summary>
        /// <param name="sender">The screen being removed.</param>
        public event ScreenRemovedEventHandler ScreenRemoved;
        public delegate void ScreenRemovedEventHandler(Screen sender);

        /// <summary>
        /// The viewport controls the screens position in world coordinates, this is used to transform world coordinates
        /// of UI elements and entities into pixel coordinates.
        /// </summary>
        public ViewPort ViewPort { get { return _ViewPort; } }
        protected ViewPort _ViewPort = new ViewPort();

        /// <summary>
        /// Gets or sets whether this screen should be visible.
        /// </summary>
        public bool Visible { get { return _Visible; } set { _Visible = value; } }
        protected bool _Visible = true;

        /// <summary>
        /// Gets or sets whether this screen should remain visible when it is obscured by other screens.
        /// </summary>
        public bool VisibleWhenObscured { get { return _VisibleWhenObscured; } set { _VisibleWhenObscured = value; } }
        protected bool _VisibleWhenObscured = false;

        /// <summary>
        /// Gets or sets whether this screen should remain visible when it is obscured by other screens.
        /// </summary>
        public bool UpdatesWhenObscured { get { return _UpdatesWhenObscured; } set { _UpdatesWhenObscured = value; } }
        protected bool _UpdatesWhenObscured = false;

        /// <summary>
        /// Gets or sets whether this screen should be paused (stops updating.)
        /// </summary>
        public bool Paused { get { return _Paused; } set { _Paused = value; } }
        protected bool _Paused = false;
        protected bool _InternalPaused = false;

        /// <summary>
        /// This is true if the screen is visibly obscured by at least one other screen in front of it.
        /// </summary>
        public bool VisiblyObscured { get { return _VisiblyObscured; } }
        protected bool _VisiblyObscured = false;

        /// <summary>
        /// This is true if the screen is input obscured by at least one other screen in front of it.
        /// </summary>
        public bool InputObscured { get { return _InputObscured; } }
        protected bool _InputObscured = false;

        /// <summary>
        /// This is true if the screen is update obscured by at least one other screen in front of it.
        /// </summary>
        public bool UpdateObscured { get { return _UpdateObscured; } }
        protected bool _UpdateObscured = false;

        /// <summary>
        /// Gets or sets whether or not this screen blocks the visibility of those behind it.
        /// </summary>
        public bool BlocksVisibility { get { return _BlocksVisibility; } set { _BlocksVisibility = value; } }
        protected bool _BlocksVisibility = true;

        /// <summary>
        /// Gets or sets whether this screen blocks input to those behind it.
        /// </summary>
        public bool BlocksInput { get { return _BlocksInput; } set { _BlocksInput = value; } }
        protected bool _BlocksInput = true;

        /// <summary>
        /// Gets or sets whether this screen blocks updates to those behind it, (effectively pauses screens behind it.)
        /// </summary>
        public bool BlocksUpdates { get { return _BlocksUpdates; } set { _BlocksUpdates = value; } }
        protected bool _BlocksUpdates = true;

        /// <summary>
        /// Gets or sets the depth of this screen. (0.0f --> front, 1.0f --> back)
        /// </summary>
        public float Depth { get { return _Depth; } set { _Depth = value; _Manager.DepthCheck(); } }
        protected float _Depth = 0.0f;

        /// <summary>
        /// Gets or sets whether this screen should be responsive to user input.
        /// </summary>
        public bool Responsive { get { return _Responsive; } set { _Responsive = value; } }
        protected bool _Responsive = true;
        protected bool _InternalResponsive = true;

        /// <summary>
        /// Gets a reference to the screen manager.
        /// </summary>
        public ScreenManager Manager { get { return _Manager; } }
        protected ScreenManager _Manager;

        /// <summary>
        /// Gets the name of this screen.
        /// </summary>
        public String Name { get { return _Name; } }
        protected String _Name;

        /// <summary>
        /// Gets or sets whether the screen requires an MSN Live account signed in to continue.
        /// </summary>
        public bool RequiresSignIn { get { return _RequiresSignIn; } set { _RequiresSignIn = value; } }
        protected bool _RequiresSignIn = false;
        protected bool _HasShownGuide = false;

        /// <summary>
        /// If true, the game will exit when the user presses the back button on this screen.
        /// </summary>
        public ActionOnBack BackBehaviour { get { return _BackBehaviour; } set { _BackBehaviour = value; } }
        protected ActionOnBack _BackBehaviour = ActionOnBack.CloseScreen;

        /// <summary>
        /// A list of entities that belong to this screen.
        /// </summary>
        public ThreadDictionary<UInt64, Entity> Entities { get { return _Entities; } }
        protected ThreadDictionary<UInt64, Entity> _Entities = new ThreadDictionary<ulong,Entity>();

        /// <summary>
        /// The name of the sound cue to play when the back button is pressed for this menu.
        /// </summary>
        public String SoundBack { get { return _SoundBack; } set { _SoundBack = value; } }
        protected String _SoundBack = _DefaultSoundBack;
        public static String DefaultSoundBack { get { return _DefaultSoundBack; } set { _DefaultSoundBack = value; } }
        protected static String _DefaultSoundBack = null;

        /// <summary>
        /// The location to fade in from.
        /// </summary>
        public Vector2 FadeInFrom { get { return _FadeInFrom; } set { _FadeInFrom = value; } }
        protected Vector2 _FadeInFrom = _DefaultFadeInFrom;
        public static Vector2 DefaultFadeInFrom { get { return _DefaultFadeInFrom; } set { _DefaultFadeInFrom = value; } }
        protected static Vector2 _DefaultFadeInFrom = new Vector2(-200, 0);

        /// <summary>
        /// The time taken for the fade in effect when this menu spawns.
        /// </summary>
        public float FadeInTime { get { return _FadeInTime; } set { _FadeInTime = value; } }
        protected float _FadeInTime = _DefaultFadeInTime;
        public static float DefaultFadeInTime { get { return _DefaultFadeInTime; } set { _DefaultFadeInTime = value; } }
        protected static float _DefaultFadeInTime = 0.00f;

        /// <summary>
        /// The location to fade out to.
        /// </summary>
        public Vector2 FadeOutTo { get { return _FadeOutTo; } set { _FadeOutTo = value; } }
        protected Vector2 _FadeOutTo = _DefaultFadeOutTo;
        public static Vector2 DefaultFadeOutTo { get { return _DefaultFadeOutTo; } set { _DefaultFadeOutTo = value; } }
        protected static Vector2 _DefaultFadeOutTo = new Vector2(200, 0);

        /// <summary>
        /// The time taken for the fade out effect when this menu dies.
        /// </summary>
        public float FadeOutTime { get { return _FadeOutTime; } set { _FadeOutTime = value; } }
        protected float _FadeOutTime = _DefaultFadeOutTime;
        public static float DefaultFadeOutTime { get { return _DefaultFadeOutTime; } set { _DefaultFadeOutTime = value; } }
        protected static float _DefaultFadeOutTime = 0.00f;

        protected bool _Dying = false;
        protected bool _Fading = false;

        /// <summary>
        /// Allows the user to display a message to the screen.
        /// </summary>
        public String Message { get { return _Message; } set { _Message = value; } }
        protected String _Message = null;

        /// <summary>
        /// The font of the menu Message.
        /// </summary>
        public SpriteFont MessageFont { get { return _MessageFont; } set { _MessageFont = value; } }
        protected SpriteFont _MessageFont = _DefaultMessageFont;
        public static SpriteFont DefaultMessageFont { get { return _DefaultMessageFont; } set { _DefaultMessageFont = value; } }
        protected static SpriteFont _DefaultMessageFont = null;

        /// <summary>
        /// The colour of the menu Message.
        /// </summary>
        public Color MessageColour { get { return _MessageColour; } set { _MessageColour = value; } }
        protected Color _MessageColour = _DefaultMessageColour;
        public static Color DefaultMessageColour { get { return _DefaultMessageColour; } set { _DefaultMessageColour = value; } }
        protected static Color _DefaultMessageColour = Color.Tomato;

        /// <summary>
        /// The screen location of the menu's Message.
        /// </summary>
        public Vector2 MessageLocation { get { return _MessageLocation; } set { _MessageLocation = value; } }
        protected Vector2 _MessageLocation = _DefaultMessageLocation;
        public static Vector2 DefaultMessageLocation { get { return _DefaultMessageLocation; } set { _DefaultMessageLocation = value; } }
        protected static Vector2 _DefaultMessageLocation = new Vector2(50, 536);


        public List<LightSource> Lights { get { return _Lights; } }
        protected List<LightSource> _Lights = new List<LightSource>();


        /// <summary>
        /// Creates a new Screen object.
        /// </summary>
        /// <param name="manager">The screen manager looking after the screen.</param>
        /// <param name="name">The name of this screen.</param>
        public Screen(ScreenManager manager, string name)
        {
            _Manager = manager;
            _Name = name;
            _Manager.ScreenRemoved += OnScreenRemoved;
            Initialize();
            Manager.AddScreen(this);
        }

        public virtual void Initialize()
        { }

        /// <summary>
        /// Fires the screen removed event.
        /// </summary>
        /// <param name="s">The screen that has been removed.</param>
        protected void OnScreenRemoved(Screen s)
        {
            if (s == null) return;
            if (s != this) return;
            if (ScreenRemoved != null)
                ScreenRemoved(s);
        }

        /// <summary>
        /// Handles the input for this screen.
        /// </summary>
        /// <param name="bind">The key bind who's state has changed.</param>
        public virtual void HandleInput(Bind bind)
        {
            if (!_Responsive || !_InternalResponsive || _Fading)
                return;
            if (!_InputObscured)
                HandleInputActive(bind);
            else
                HandleInputObscured(bind);
        }

        /// <summary>
        /// Handles the input for the screen when this screen is not obscured.
        /// </summary>
        /// <param name="bind">The key bind who's state has changed.</param>
        protected virtual void HandleInputActive(Bind bind) 
        {
            // Fire input recieved event
            if (InputRecieved != null)
                InputRecieved(this, bind);

            if (bind.State == KeyState.Up) return;

            // Handle "back" button pressed
            if (_BackBehaviour == ActionOnBack.None) return;
            if (bind.Name.CompareTo("back") == 0)
            {
                if (_SoundBack != null && Application.AppReference.SoundBank != null && !Application.AppReference.SoundBank.IsDisposed)
                    Application.AppReference.SoundBank.PlayCue(_SoundBack);
                if (_BackBehaviour == ActionOnBack.ExitApplication)
                    Application.AppReference.Exit();
                else
                    Remove();
            }
        }

        /// <summary>
        /// Handles the input for the screen when this screen is obscured.
        /// </summary>
        /// <param name="bind">The key bind who's state has changed.</param>
        protected virtual void HandleInputObscured(Bind bind) 
        {
            // Fire obscure input recieved event
            if (ObscureInputRecieved != null)
                ObscureInputRecieved(this, bind);
        }

        /// <summary>
        /// Renders this screen.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        /// <param name="batch">The spritebatch on which to render the screen.</param>
        public virtual void Draw(GameTime time, SpriteBatch batch) 
        {
            // Break early if user made menu invisible
            if (!Visible)
                return;

            // Check if this menu is hidden behind other screens
            if (_VisiblyObscured && !_VisibleWhenObscured && !_Fading)
                return;

            // Draw each of the screen's entities
            _Entities.ForEach(DrawEntity, time, batch, null);

            // Draw the message string
            if (_Message != null && _MessageFont != null)
            {
                Vector2 TopLeft_Pixels = ViewPort.Transform_UnitPosition_To_PixelPosition(MessageLocation);
                Vector2 Size_Pixels = ViewPort.Transform_UnitSize_To_PixelSize(MessageFont.MeasureString(Message));
                Vector2 Scale = Size_Pixels / MessageFont.MeasureString(Message);

                batch.DrawString(_MessageFont, _Message, TopLeft_Pixels, _MessageColour, 0.0f, new Vector2(0, 0), Scale.X, SpriteEffects.None, Depth);
            }
        }

        private object DrawEntity(Entity ent, object time, object batch, object p3) { ent.Draw(time as GameTime, batch as SpriteBatch); return null; }

        /// <summary>
        /// Updates the screen state.
        /// </summary>
        /// <param name="time">The GameTime object from the XNA framework.</param>
        public virtual void Update(GameTime time) 
        {
            // Check for fadeout completion
            if (_Fading && !_ViewPort.IsSliding)
                _Fading = false;
            if (_Dying && !_ViewPort.IsSliding)
                Manager.RemoveScreen(this);

            _ViewPort.Update(time); 

            // Check that the player has signed in.
            if (_RequiresSignIn)
            {
                if (SignedInGamer.SignedInGamers[0] == null)
                {
                    _InternalResponsive = false;
                    _InternalPaused = true;

                    if (Guide.IsVisible == false)
                    {
                        if (_HasShownGuide)
                            Remove();
                        else
                            Guide.ShowSignIn(1, false);
                        _HasShownGuide = true;
                    }
                }
                else
                {
                    _InternalResponsive = true;
                    _InternalPaused = false;
                }
            }

            // Break early from the update if the screen is paused
            if (_Paused || _InternalPaused) return;

            // Break if the screen is update obscured
            if (_UpdateObscured && !_UpdatesWhenObscured) return;

            // Update screens entities
            _Entities.ForEach(UpdateEntity, time, null, null);
        }

        private object UpdateEntity(Entity ent, object time, object p2, object p3) { ent.Update(time as GameTime); return null; }

        /// <summary>
        /// Removes (kills) this screen.
        /// </summary>
        public virtual void Remove()
        {
            if (_Dying) return;
            _Dying = true;
            FadeOut();
        }

        public virtual void FadeIn()
        {
            _Fading = true;
            _ViewPort.Slide(_FadeInFrom, new Vector2(0, 0), _FadeInTime);
        }

        public virtual void FadeOut()
        {
            _Fading = true;
            _ViewPort.Slide(new Vector2(0, 0), _FadeOutTo, _FadeOutTime);
        }

        /// <summary>
        /// Obscures this screen.
        /// </summary>
        public virtual void ObscureVisibility()
        {
            if (!_VisiblyObscured)
                FadeOut();
            _VisiblyObscured = true;
            if (ScreenVisiblyObscured != null)
                ScreenVisiblyObscured(this);
        }

        /// <summary>
        /// Obscures this screen.
        /// </summary>
        public virtual void ObscureInput()
        {
            _InputObscured = true;
            if (ScreenInputObscured != null)
                ScreenInputObscured(this);
        }

        /// <summary>
        /// Obscures this screen.
        /// </summary>
        public virtual void ObscureUpdates()
        {
            _UpdateObscured = true;
            if (ScreenUpdateObscured != null)
                ScreenUpdateObscured(this);
        }

        /// <summary>
        /// Activates this screens visibility.
        /// </summary>
        public virtual void ActivateVisibility()
        {
            if (_VisiblyObscured)
                FadeIn();
            _VisiblyObscured = false;
            if (ScreenVisiblyActivated != null)
                ScreenVisiblyActivated(this);
        }

        /// <summary>
        /// Activates this screens visibility.
        /// </summary>
        public virtual void ActivateInput()
        {
            _InputObscured = false;
            if (ScreenInputActivated != null)
                ScreenInputActivated(this);
        }

        /// <summary>
        /// Activates this screens visibility.
        /// </summary>
        public virtual void ActivateUpdates()
        {
            _UpdateObscured = false;
            if (ScreenUpdateActivated != null)
                ScreenUpdateActivated(this);
        }
    }

    public enum ActionOnBack
    {
        None,
        CloseScreen,
        ExitApplication
    }

}
