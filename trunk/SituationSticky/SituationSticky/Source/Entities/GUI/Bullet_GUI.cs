using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SituationSticky
{
    public class Bullet_GUI : Entity
    {
        public Bullet_GUI(Screen Parent, Vector2 position)
            : base(Parent.Entities, position, 6f, 28f, 0f)
            
        { }

        public override string Initialize()
        {
            // Animation
            _Animations = new AnimationSet();
            _Animations.AddAnimation(new Animation("Textures/GUI/Bullet01_1x1", "Normal", 1, 1, 1.0f));

            // Settings
            _Depth = 0.18f;
            _DynamicLighting = false;

            // Return the name for this class
            return "Bullet_GUI";
        }
    }
}
