using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/****************************************************************************/
/* Class Name  : HealthPack                                                 */
/* Author      : Chris                                                      */
/* Date Created: 3/1/2010 4:46:49 AM                                        */
/****************************************************************************/

namespace SituationSticky
{
    class HealthPack : Entity
    {
        #region Constants
        // Hardcoded class settings

        #endregion


        #region Members
        // Variable class members

        #endregion


        #region Init and Disposal

        public HealthPack(Screen parent, Vector3 position)
            : base(parent.Entities, position, new Vector3(25,25,25), Vector2.Zero)
        { }

        /// <summary>
        /// Initializes the entity with the correct settings.
        /// </summary>
        /// <returns>The string name of the entity.</returns>
        public override string Initialize()
        {
            base.Initialize();

            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/Health01_1x1", "Normal", 1, 1, 1f));

            // Settings
            _DynamicLighting = false;
            _Depth = 0.5f;
            _Temporary = false;
            _Spin = 0.01f;
            _CollisionType = CollisionType.Passive;
            new LightSource(_Parent, this, _Position, new Color(0.15f, 0f, 0f, 1f), 150f, 6.28f, Vector2.Zero);

            return "HealthPack";
        }

        public static HealthPack CreateHealthPack(Screen parent, Vector3 position) { return new HealthPack(parent, position); }

        #endregion


        #region Update

        /// <summary>
        /// Extends the update functionality for this entity.
        /// </summary>
        /// <param name="time">The XNA gametime parameter.</param>
        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        #endregion


        #region Draw

        /// <summary>
        /// Extends the draw functionality for this entity.
        /// </summary>
        /// <param name="time">The XNA gametime parameter.</param>
        /// <param name="batch">The spritebatch to render on.</param>
        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);
        }

        #endregion


        #region Utility

        protected override void HandleCollision(Entity ent)
        {
            if (ent as Marine != null)
            {
                ((Marine)ent).CurrentHP += 50;
                Dispose();
                new FloatingText(_Parent, _Position, 0.3f, 0.92f, "+50 HP", "Fonts/FloatingFont", new Color(0.2f, 0.6f, 0.3f, 0.7f), 1200);
            }
        }

        #endregion
    }
}
