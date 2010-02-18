using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Editor_Gui : Entity
    {
        protected Screen parent;
        protected Vector2 firstPos = new Vector2(250f, 250f);

        public Editor_Gui(Screen Parent)
            : base(Parent)

        {
            this.parent = Parent;
            DynamicLighting = false;
            Depth = 0.19f;
        }

        public override string Initialize()
        {
            Geometry = Geometry.CreateRectangularGeometry(this, 53, 53);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("editorbox", "Normal", 1, 1, 1.0f));

            // Set marine towards front of screen
            //_Depth = 0.2f;

            // Return the name for this class
            return "Editor_Gui";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }


    }
}
