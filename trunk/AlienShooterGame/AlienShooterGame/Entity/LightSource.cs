using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class LightSource
    {
        public float Range { get { return _Range; } set { _Range = value; } }
        protected float _Range = 700.0f;

        public float Radius { get { return _Radius; } set { _Radius = value; } }
        protected float _Radius = 1.9f;

        public Color Colour { get { return _Colour; } set { _Colour = value; } }
        protected Color _Colour;

        public Vector2 Position = new Vector2();

        public double Direction { get { return _Direction; } set { _Direction = value; } }
        protected double _Direction = 0.0f;

        public Screen Parent { get { return _Parent; } }
        protected Screen _Parent;

        public bool Active { get { return _Active; } set { _Active = value; } }
        protected bool _Active = true;


        public LightSource(Screen parent, Color colour, float range, float radius, double direction, Vector2 position)
        {
            _Radius = radius;
            _Range = range;
            _Colour = colour;
            _Direction = direction;
            Position = position;
            _Parent = parent;
            _Parent.Lights.Add(this);
        }
    }
}
