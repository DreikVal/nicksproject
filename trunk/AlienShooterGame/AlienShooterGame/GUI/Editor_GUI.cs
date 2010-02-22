using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Editor_Gui : Entity
    {

        public Editor_Gui(Screen Parent)
            : base(Parent)

        {
        }

        public override string Initialize()
        {
            _Geometry = new Geometry(this, new Vector2(564, 407), 53.0f, 53.0f, 0.0f, 50.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("editorbox", "Normal", 1, 1, 1.0f));

            _Depth = 0.2f;

            DynamicLighting = false;

            // Return the name for this class
            return "Editor_Gui";
        }
    }
}
