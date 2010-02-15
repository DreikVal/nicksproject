using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    class WorldScreen : Screen
    {
        protected Marine _Player;
        protected Crosshair _Crosshair;


        public WorldScreen(ScreenManager manager)
            : base(manager, "World")
        {
            _Player = new Marine(this);
            //_Entities.Add(_Player.ID, _Player);

            _Crosshair = new Crosshair(this);

            _Player.Geometry.Position.X = 200;
            _Player.Geometry.Position.Y = 200;

            _BackBehaviour = ActionOnBack.ExitApplication;
        }

        protected override void HandleInputActive(Bind bind)
        {
            base.HandleInputActive(bind);

            if (bind.Name.CompareTo("Forward") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                    _Player.MoveForward = true;
                else
                    _Player.MoveForward = false;
            }
        }
    }
}
