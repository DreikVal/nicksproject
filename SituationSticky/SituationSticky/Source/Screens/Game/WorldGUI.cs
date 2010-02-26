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
        Ammo_GUI _AmmoGUI;
        Health_GUI _HealthGUI;
        Radar_GUI _RadarGUI;
        //Score_GUI _ScoreGUI;

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
            _AmmoGUI = new Ammo_GUI(this, new Vector2(700,400));
            _HealthGUI = new Health_GUI(this, new Vector2(35,400));
            _RadarGUI = new Radar_GUI(this, new Vector2(725,75));
            //_ScoreGUI = new Score_GUI(this, new Vector2(400,400));           
        }
    }
}