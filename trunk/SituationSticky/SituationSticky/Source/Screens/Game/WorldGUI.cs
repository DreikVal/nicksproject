using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    class WorldGUI : Screen
    {
        #region Members

        /// <summary>
        /// The ammoy gui displays player's current ammo and weapon type.
        /// </summary>
        protected Ammo_GUI _AmmoGUI;

        /// <summary>
        /// The health gui displays player's current HP.
        /// </summary>
        protected Health_GUI _HealthGUI;

        /// <summary>
        /// The radar gui tracks nearby aliens.
        /// </summary>
        protected Radar_GUI _RadarGUI;

        /// <summary>
        /// The score gui displays player's score.
        /// </summary>
        protected Score_GUI _ScoreGUI;

        #endregion

        #region Init and Disposal

        public WorldGUI(ScreenManager manager)
            : base(manager, "WorldGUI")
        {
            // Screen settings
            _Depth = 0.2f;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _BlocksInput = false;
            _BlocksUpdates = false;
            _BlocksVisibility = false;
            _DynamicLighting = false;
            _ViewPort.Size = new Vector2(800, 440);
            
            // Setup GUI entities
            _AmmoGUI = new Ammo_GUI(this, new Vector3(700,400,0));
            _HealthGUI = new Health_GUI(this, new Vector3(22,420,0));
            _RadarGUI = new Radar_GUI(this, new Vector3(725,75,0));
            _ScoreGUI = new Score_GUI(this, new Vector3(20,30,0));
        }

        #endregion
    }
}