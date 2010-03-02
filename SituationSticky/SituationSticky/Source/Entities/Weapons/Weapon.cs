using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace SituationSticky
{
    public class Weapon : Entity
    {
        #region Members

        /// <summary>
        /// The cooldown in milliseconds for this weapon.
        /// </summary>
        public int Cooldown { get { return _Cooldown; } set { _Cooldown = value; } }
        protected int _Cooldown = 50;

        /// <summary>
        /// The reload time in milliseconds for this weapon.
        /// </summary>
        public int ReloadTime { get { return _ReloadTime; } set { _ReloadTime = value; } }
        protected int _ReloadTime = 1250;

        /// <summary>
        /// The remaining milliseconds on cooldown.
        /// </summary>
        public int RemainingCooldown { get { return _RemainingCooldown; } set { _RemainingCooldown = value; } }
        protected int _RemainingCooldown = 0;

        /// <summary>
        /// The remaining milliseconds on reload.
        /// </summary>
        public int RemainingReload { get { return _RemainingReload; } set { _RemainingReload = value; } }
        protected int _RemainingReload = 0;

        /// <summary>
        /// The clip size for this weapon.
        /// </summary>
        public int ClipSize { get { return _ClipSize; } set { _ClipSize = value; } }
        protected int _ClipSize = 75;

        /// <summary>
        /// The current ammo in clip for this weapon.
        /// </summary>
        public int Ammo { get { return _Ammo; } set { _Ammo = value; } }
        protected int _Ammo;

        /// <summary>
        /// The marine who owns this weapon.
        /// </summary>
        public Marine Player { get { return _Player; } set { _Player = value; } }
        protected Marine _Player;

        /// <summary>
        /// The soundeffect of this weapon.
        /// </summary>
        public SoundEffect Sound { get { return _Sound; } set { _Sound = value; } }
        protected SoundEffect _Sound;

        /// <summary>
        /// Gets or sets whether the marine is pulling the trigger.
        /// </summary>
        public bool IsFiring { get { return _IsFiring; } set { _IsFiring = value; } }
        protected bool _IsFiring = false;

        /// <summary>
        /// Gets or whether the marine is reloading this weapon.
        /// </summary>
        public bool IsReloading { get { return _IsReloading; } }
        protected bool _IsReloading = false;

        public String Name { get { return _Name; } set { _Name = value; } }
        protected String _Name = "Default Weapon";

        #endregion

        #region Init and Disposal

        public Weapon(Screen parent, Marine player)
            : base(parent.Entities, player.Position, Vector3.Zero, Vector3.Zero)
        {
            _Player = player;
            _Sound = Application.AppReference.Content.Load<SoundEffect>("Audio/Effects/Bullet01");
            _Ammo = _ClipSize;
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            // Keep position OnScreen so weapon doesn't get unloaded.
            _Position = _Player.Position;

            // Update remaining cooldown/reload times
            if (_RemainingCooldown > 0)
                _RemainingCooldown -= time.ElapsedGameTime.Milliseconds;
            if (_RemainingReload > 0)
                _RemainingReload -= time.ElapsedGameTime.Milliseconds;

            // Reload finished
            if (_IsReloading && _RemainingReload <= 0)
            {
                _IsReloading = false;
                _Ammo = _ClipSize;
            }
            
            // Firing weapon
            if (_IsFiring && _RemainingCooldown <= 0 && !_IsReloading && _Ammo > 0 && !_Player.Disposed)
            {
                Fire();
            }
        }

        #endregion

        #region Utility

        public virtual void Reload()
        {
            _RemainingReload = _ReloadTime;
            _Ammo = 0;
            _IsReloading = true;
        }

        protected virtual void Fire()
        {
            // Create muzzle light source
            _Player.Muzzle.Active = true;
            _Player.MuzzleLifeTime = 100;

            // Create muzzle flash sprite
            //new MuzzleFlash(_Parent, _Player, new Vector3(20, 0, 0));

            // Apply weapon cooldown
            _RemainingCooldown = _Cooldown;

            // Create bullet (slightly in front of marine)
            Vector3 bulletPos = _Player.Position;
            bulletPos.X += (float)Math.Sin(_Player.Direction.Z) * 25.0f;
            bulletPos.Y += -(float)Math.Cos(_Player.Direction.Z) * 25.0f;
            new Bullet(_Player.Parent, _Player, bulletPos, _Player.Direction);

            // Shake screen slightly
            _Player.Parent.ViewPort.Shake(1.5f, 0.8f, 0.95f);

            // Update ammo
            if (--_Ammo <= 0)
                Reload();

            // Play weapon sound
            if (_Sound != null)
                _Sound.Play(0.6f, 0.2f, 0.0f);
        }

        #endregion
    }
}
