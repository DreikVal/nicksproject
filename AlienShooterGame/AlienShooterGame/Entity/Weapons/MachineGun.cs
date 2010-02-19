using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienShooterGame
{
    public class MachineGun : Weapon
    {
        public MachineGun(Marine player)
            : base(player)
        {
            this.weaponCooldown = 50;
            this.ReloadTime = 2000;
        }

        public override string getName()
        {
            return "Machinegun";
        }
    }
}
