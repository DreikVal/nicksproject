using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    public class AutoHandGun : Weapon
    {
        public AutoHandGun(Marine player)
            : base(player)
        {
        }

        public override string getName()
        {
            return "Auto Handgun";
        }
    }
}
