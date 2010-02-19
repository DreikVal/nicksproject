using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Radar_Gui : Entity
    {
        protected Screen parent;
        protected Ammo_Gui_Bullet[] bullets;
        protected Vector2 firstPos = new Vector2(794f, 422f);
        protected Vector2 increment = new Vector2(-7f, 0f);

        protected List<Vector2> radarBlip = new List<Vector2>();
        protected Texture2D _BlipTex;

        public Radar_Gui(Screen Parent)
            : base(Parent)

        {}

        public override string Initialize()
        {
            Geometry = new Geometry(this, new Vector2(), 120.0f, 120.0f, 0.0f);


            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("radar", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            _Depth = 0.19f;

            _BlipTex = Application.AppReference.Content.Load<Texture2D>("blip");

            // Return the name for this class
            return "Radar_Gui";
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);

            Screen screen;
            WorldScreen world;
            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                world.Entities.ForEach(FindAliens, batch, world.Player, null);
            }
                catch { }

            
        }

        private object FindAliens(Entity ent, object batch, object player, object p3)
        {
            SpriteBatch spriteBatch = (SpriteBatch)batch;
            Marine marine = (Marine)player;
            float scalingFactor = 0.1f;

            if (ent as Alien == null)
                return null;

            Vector2 diff = ent.Geometry.Position - marine.Geometry.Position;
            Vector2 worldLoc = Geometry.Position + (scalingFactor*diff);
            Vector2 pixelLoc = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(worldLoc);
            Vector2 pixelSize = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(ent.Geometry.Size * scalingFactor);

            spriteBatch.Draw(_BlipTex, new Rectangle((int)pixelLoc.X, (int)pixelLoc.Y, (int)pixelSize.X, (int)pixelSize.Y), Color.White);         

            return null;
        }



    }
}
