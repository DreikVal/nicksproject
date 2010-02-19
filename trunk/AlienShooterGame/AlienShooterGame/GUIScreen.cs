using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class GUIScreen : Screen
    {
        Ammo_Gui _AmmoGUI;

        public GUIScreen(ScreenManager manager)
            : base(manager, "GUI")
        {
            _ViewPort.Size = new Vector2(800, 440);
            _AmmoGUI = new Ammo_Gui(this);

            
            _AmmoGUI.Geometry.Position.X = this.ViewPort.Size.X - 90;
            _AmmoGUI.Geometry.Position.Y = this.ViewPort.Size.Y - 35;

            Depth = 0.2f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            this.BlocksInput = false;
            this.BlocksUpdates = false;
            this.BlocksVisibility = false;
            this.Lights.Clear();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }      
    }
}