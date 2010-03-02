using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SituationSticky
{
    public class TextureBox_GUI : Entity_Quad
    {

        public TextureBox_GUI(Screen Parent)
            : base(Parent.Entities, new Vector3(250f, 250f, 0), new Vector3(258, 268, 0), Vector3.Zero) { }

        public override string Initialize()
        {
            base.Initialize();

            // Animations
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/TextureListBox01_1x1", "Normal", 1, 1, 1.0f));

            // Settings
            _Depth = 0.2f;
            _DynamicLighting = false;

            return "TextureBox_GUI";
        }
    }
}
