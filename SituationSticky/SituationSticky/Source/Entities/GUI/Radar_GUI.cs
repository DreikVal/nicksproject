using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class Radar_GUI : Entity
    {
        #region Members

        /// <summary>
        /// Texture used for the radar blips.
        /// </summary>
        protected Texture2D _BlipTex;

        #endregion

        #region Init and Disposal

        /// <summary>
        /// Creates a new Radar_GUI element.
        /// </summary>
        /// <param name="Parent">The screen for the radar.</param>
        /// <param name="position">The location of the radar.</param>
        public Radar_GUI(Screen Parent, Vector2 position)
            : base(Parent.Entities, position, 120f, 120f, 0f) {}

        public override string Initialize()
        {
            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/Radar01_1x1", "Normal", 1, 1, 1.0f));

            // Settings
            _Depth = 0.19f;
            _BlipTex = Application.AppReference.Content.Load<Texture2D>("Textures/GUI/Blip01_1x1");

            // Return the name for this class
            return "Radar_GUI";
        }

        #endregion

        #region Draw

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);

            WorldScreen world = _Parent.Manager.GetScreen("World") as WorldScreen;
            if (world == null) return;

            // Draw each blip individually
            world.Entities.Loaded.ForEach(FindAliens, batch, world.PlayerEntity, null);
        }

        private bool FindAliens(Entity ent, object batch, object player, object p3)
        {
            SpriteBatch spriteBatch = batch as SpriteBatch;
            Marine marine = player as Marine;
            float scalingFactor = 0.1f;
            float dFactor = 0.05f;

            if (ent as Drone == null)
                return true;

            Vector2 diff = ent.Position - marine.Position;
            Vector2 worldLoc = _Position + (dFactor * diff);
            Vector2 pixelLoc = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(worldLoc);
            Vector2 pixelSize = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(ent.Size * scalingFactor);

            spriteBatch.Draw(_BlipTex, new Rectangle((int)pixelLoc.X, (int)pixelLoc.Y, (int)pixelSize.X, (int)pixelSize.Y), Color.White);

            return true;
        }

        #endregion       
    }
}
