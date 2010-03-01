using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class MuzzleFlash : Entity
    {
        #region Constants

        public const int DefaultLifeTime = 120;
        public static Color MuzzleColour = new Color(1f, 0.9f, 0.7f, 0.25f);

        #endregion

        #region Members

        /// <summary>
        /// The owner of the entity, the position of the muzzle flash stays relative to this owner.
        /// </summary>
        protected Entity _Owner = null;

        /// <summary>
        /// The positional offset from the owner of this muzzle flash.
        /// </summary>
        protected Vector2 _Offset;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new muzzle flash object.
        /// </summary>
        /// <param name="parent">The screen to create the muzzle flash on.</param>
        /// <param name="owner">The owner (creator) of the muzzle flash.</param>
        /// <param name="offset">The positional offset from the owner.</param>
        public MuzzleFlash(Screen parent, Entity owner, Vector2 offset)
            : base(parent.Entities, owner.Position, new Vector3(15,20,0), owner.Direction)
        {
            _Owner = owner;
            _Offset = offset;
            //_Position.X = _Owner.Position.X + (float)Math.Sin(_Owner.Direction) * _Offset.X + (float)Math.Cos(_Owner.Direction) * _Offset.Y;
            //_Position.Y = _Owner.Position.Y + (float)Math.Sin(_Owner.Direction) * _Offset.Y - (float)Math.Cos(_Owner.Direction) * _Offset.X;
        }

        public override string Initialize()
        {         
            // Animations
            _Animations = new AnimationSet();
            Animation normal = new Animation("Textures/Effects/MuzzleFlash01_1x3", "Normal", 1, 3, 20.0f);
            normal.Loop = 1;
            _Animations.AddAnimation(normal);
            _Animations.PlayAnimation("Normal");

            // Settings
            _Depth = 0.18f;
            _ColourOverlay = MuzzleColour;
            _LifeTime = DefaultLifeTime;
            _DynamicLighting = false;
            _Temporary = true;

            return "MuzzleFlash";
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);

            // Lock location relative to the owner.
            //_Position.X = _Owner.Position.X + (float)Math.Sin(_Owner.Direction) * _Offset.X + (float)Math.Cos(_Owner.Direction) * _Offset.Y;
            //_Position.Y = _Owner.Position.Y + (float)Math.Sin(_Owner.Direction) * _Offset.Y - (float)Math.Cos(_Owner.Direction) * _Offset.X;
        }

        #endregion
    }
}
