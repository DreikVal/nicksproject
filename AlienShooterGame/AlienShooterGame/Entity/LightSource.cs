using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class LightSource : Entity
    {
        public float Range { get { return _Range; } set { _Range = value; } }
        protected float _Range = 700.0f;

        public float Radius { get { return _Radius; } set { _Radius = value; } }
        protected float _Radius = 1.9f;

        public bool Active { get { return _Active; } set { _Active = value; } }
        protected bool _Active = true;

        public LightSource(Screen parent, Color colour, float range, float radius, double direction, Vector2 position) : base(parent)
        {
            _Radius = radius;
            _Range = range;
            _ColourOverlay = colour;
            _Geometry = new Geometry(this, position, 0.0f, 0.0f, (float)direction);
            _Parent.Lights.Add(_ID, this);
        }

        public override void Update(GameTime time) { }
        public override void Draw(GameTime time, SpriteBatch batch) { }
        public override void Dispose()
        {
            base.Dispose();
            _Parent.Lights.Remove(_ID);
        }
    }
}
