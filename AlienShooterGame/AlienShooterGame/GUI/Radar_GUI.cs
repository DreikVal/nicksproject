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

        public Radar_Gui(Screen Parent)
            : base(Parent)

        {
            this.parent = Parent;
            DynamicLighting = false;
            Depth = 0.19f;
        }

        public override string Initialize()
        {
            Geometry = Geometry.CreateRectangularGeometry(this, 120, 120);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("radar", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Radar_Gui";
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            Screen screen;
            WorldScreen world;
            try
            {
                _Parent.Manager.LookupScreen("World", out screen);
                world = (WorldScreen)screen;
                world.Entities.ForEach(FindAliens, batch, null, null);
            }
            catch { }

            base.Draw(time, batch);
        }

        private object FindAliens(Entity ent, object batch, object p2, object p3)
        {
            SpriteBatch spriteBatch = (SpriteBatch)batch;

            float xRatio = 4;
            float yRatio = 4;

            if (ent as Alien == null)
                return null;
            
            else
            {
                spriteBatch.Draw(Application.AppReference.Content.Load<Texture2D>("blip"), 
                    new Vector2(ent.Geometry.Position.X/xRatio + Geometry.Position.X + 427, ent.Geometry.Position.Y/yRatio - 143),
                    null,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0.01f);
            }
            return null;
        }



    }
}
