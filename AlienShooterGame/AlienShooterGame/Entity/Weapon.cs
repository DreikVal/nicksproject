using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Weapon
    {
        public double weaponCooldown = 200;
        public int ReloadTime = 1200;
        protected Marine player;
        protected SoundEffect _Sound;

        public Weapon(Marine Player)
        {
            this.player = Player;
            _Sound = Application.AppReference.Content.Load<SoundEffect>("Sounds\\bullet");
        }

        public void Fire()
        {

            // return;

            player.Muzzle.Active = true;
            player.MuzzleFrames = 5;
            Vector2 bulletPos = player.Geometry.Position;
            bulletPos.X += (float)Math.Sin(player.Geometry.Direction) * 25.0f;
            bulletPos.Y += -(float)Math.Cos(player.Geometry.Direction) * 25.0f;
            new Bullet(player.Parent, player, bulletPos, player.Geometry.Direction);
            player.Parent.ViewPort.Shake(1.5f, 0.8f, 0.95f);
            new MuzzleFlash(player.Parent, bulletPos, player);
            if (--player.Ammo <= 0)
                player._Reloading = ReloadTime;

            _Sound.Play(0.6f, 0.2f, 0.0f);
        }

        public virtual string getName()
        {
            return "Handgun";
        }
    }
}
