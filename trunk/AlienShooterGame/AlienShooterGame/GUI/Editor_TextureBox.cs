using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class Editor_TextureBox : Entity
    {

        public Editor_TextureBox(Screen Parent)
            : base(Parent)

        {
        }

        public override string Initialize()
        {
            _Geometry = new Geometry(this, new Vector2(250, 250), 258, 268, 0.0f, 50.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("texturelistbox", "Normal", 1, 1, 1.0f));

            _Depth = 0.2f;

            DynamicLighting = false;

            // Return the name for this class
            return "Editor_Gui";
        }
    }
}
