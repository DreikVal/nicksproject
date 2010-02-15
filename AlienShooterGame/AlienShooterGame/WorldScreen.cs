using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    class WorldScreen : Screen
    {
        protected Marine _Player;


        public WorldScreen(ScreenManager manager)
            : base(manager, "World")
        {
            _Player = new Marine(this);
            _Entities.Add(_Player.ID, _Player);
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
