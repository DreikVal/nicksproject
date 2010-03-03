using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/****************************************************************************/
/* Class Name  : Entity_3D                                             */
/* Author      : Chris                                                 */
/* Date Created: 3/3/2010 4:25:25 AM                                                     */
/****************************************************************************/

namespace SituationSticky
{
    public class Entity_3D : Entity
    {
        #region Constants
        // Hardcoded class settings

        #endregion


        #region Members
        // Variable class members
        public Model Model { get { return _Model; } set { _Model = value; } }
        protected Model _Model;

        public float ModelScale { get { return _ModelScale; } set { _ModelScale = value; } }
        protected float _ModelScale = 1.0f;

        public Vector3 ModelRotation { get { return _ModelRotation; } set { _ModelRotation = value; } }
        protected Vector3 _ModelRotation = Vector3.Zero;

        #endregion


        #region Init and Disposal

        public Entity_3D(EntityList list, Vector3 position, Vector3 size, Vector3 direction)
            : base(list, position, size, direction)
        { }

        /// <summary>
        /// Initializes the entity with the correct settings.
        /// </summary>
        /// <returns>The string name of the entity.</returns>
        public override string Initialize()
        {
            base.Initialize();

            // Model
            

            // Settings
            _DynamicLighting = false;
            _Depth = 0.5f;
            _Temporary = false;

            return "DefaultEntityName";
        }

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

            if (_Model == null) return;

            foreach (ModelMesh mesh in _Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World =
                        Matrix.CreateScale(_Size * _ModelScale)
                        * Matrix.CreateFromYawPitchRoll(_ModelRotation.Y, _ModelRotation.X, _ModelRotation.Z)
                        * Matrix.CreateFromYawPitchRoll(_Direction.Y, _Direction.X, _Direction.Z)
                        * Matrix.CreateTranslation(_Position)
                        ;
                    effect.Projection = Parent.ViewPort.ProjectionMatrix;
                    effect.View = Parent.ViewPort.ViewMatrix;
                }
                mesh.Draw();
            }
        }

        #endregion


        #region Utility

        #endregion
    }
}
