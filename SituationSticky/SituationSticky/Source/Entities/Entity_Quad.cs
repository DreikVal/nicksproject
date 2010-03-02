using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/****************************************************************************/
/* Class Name  : Entity_Quad                                             */
/* Author      : Chris                                                 */
/* Date Created: 3/3/2010 4:28:34 AM                                                     */
/****************************************************************************/

namespace SituationSticky
{
    public class Entity_Quad : Entity
    {
        #region Constants
        // Hardcoded class settings

        #endregion


        #region Members
        // Variable class members
        /// <summary>
        /// Gets a set of animations for this entity.
        /// </summary>
        public AnimationSet Animations { get { return _Animations; } }
        protected AnimationSet _Animations = new AnimationSet();

        protected Quad _Quad;
        protected BasicEffect _QuadEffect;
        protected VertexDeclaration _QuadVertexDecl;

        #endregion


        #region Init and Disposal

        public Entity_Quad(EntityList list, Vector3 position, Vector3 size, Vector3 direction)
            : base(list, position, size, direction)
        { }

        /// <summary>
        /// Initializes the entity with the correct settings.
        /// </summary>
        /// <returns>The string name of the entity.</returns>
        public override string Initialize()
        {
            base.Initialize();

            // Settings
            _DynamicLighting = false;
            _Depth = 0.5f;
            _Temporary = false;

            // Quad
            _Quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, _Size.X, _Size.Y);
            _QuadEffect = new BasicEffect(Application.AppReference.GraphicsManager.GraphicsDevice, null);
            _QuadEffect.EnableDefaultLighting();
            _QuadEffect.PreferPerPixelLighting = true;
            _QuadVertexDecl = new VertexDeclaration(Application.AppReference.GraphicsManager.GraphicsDevice,
               VertexPositionNormalTexture.VertexElements);

            return "Entity_Quad";
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

            if (_Animations.Current == null) _QuadEffect.TextureEnabled = false;
            else
            {
                _QuadEffect.TextureEnabled = true;
                _QuadEffect.Texture = _Animations.Current.Texture;
            }
            _QuadEffect.World = Matrix.CreateRotationZ(_Direction.Z) * Matrix.CreateTranslation(_Position);
            _QuadEffect.View = _Parent.ViewPort.ViewMatrix;
            _QuadEffect.Projection = _Parent.ViewPort.ProjectionMatrix;
            Application.AppReference.GraphicsDevice.VertexDeclaration = _QuadVertexDecl;
            _QuadEffect.Begin();
            foreach (EffectPass pass in _QuadEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Application.AppReference.GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    _Quad.Vertices, 0, 4,
                    _Quad.Indexes, 0, 2);

                pass.End();
            }
            _QuadEffect.End();
        }

        #endregion


        #region Utility

        #endregion
    }
}
