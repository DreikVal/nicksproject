using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlienShooterGame
{
    public class Editor_Gui : Entity
    {
        protected Vector2 _Offset;

        public Editor_Gui(Screen Parent, Vector2 offset)
            : base(Parent)

        {
            _Offset = offset;
        }

        public override string Initialize()
        {
            _Geometry = new Geometry(this, new Vector2(), 53.0f, 53.0f, 0.0f, 50.0f);

            // Create an animation set for the marine
            _Animations = new AnimationSet();

            // Add the default animation
            _Animations.AddAnimation(new Animation("editorbox", "Normal", 1, 1, 1.0f));

            _Depth = 0.19f;

            DynamicLighting = false;

            // Return the name for this class
            return "Editor_Gui";
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            Geometry.Position = _Parent.ViewPort.ActualLocation + _Offset;
        }


    }
}
