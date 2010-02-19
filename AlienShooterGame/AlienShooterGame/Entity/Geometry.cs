using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AlienShooterGame
{
    public class Geometry
    {
        public Vector2 Position;

        public double Direction { get { return _Direction; } set { _Direction = value; } }
        protected double _Direction;

        public float CollisionRadius { get { return _CollisionRadius; } set { _CollisionRadius = value; } }
        protected float _CollisionRadius;

        public float Radius { get { return _Radius; } }
        protected float _Radius;

        public Entity Parent { get { return _Parent; } set { _Parent = value; } }
        protected Entity _Parent;

        public float Width { get { return _Width; } set { _Width = value; } }
        protected float _Width;

        public float Height { get { return _Height; } set { _Height = value; } }
        protected float _Height;

        public Vector2 Size { get { return new Vector2(_Width, _Height); } }

        public Geometry(Entity parent, Vector2 position, float width, float height, float direction, float collisionRadius)
        {
            _Parent = parent;
            _Width = width;
            _Height = height;
            Position = position;
            _Direction = direction;
            _CollisionRadius = collisionRadius;
            _Radius = (float)Math.Sqrt(height * height + width * width);
        }

        public Geometry(Entity parent, Vector2 position, float width, float height, float direction)
        {
            _Parent = parent;
            _Width = width;
            _Height = height;
            Position = position;
            _Direction = direction;
            _Radius = (float)Math.Sqrt(((height * height)/4) + ((width * width)/4));
            float correctionFactor = Math.Min(width, height) / Math.Max(width, height);
            _CollisionRadius = _Radius * correctionFactor;
        }   

        public bool Collision(Geometry otherGeom)
        {
            Vector2 diff = Position - otherGeom.Position;
            if (diff.Length() < _CollisionRadius + otherGeom.CollisionRadius)
                return true;

            return false;
        }
    }

}
