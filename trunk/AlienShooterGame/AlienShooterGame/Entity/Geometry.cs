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
        public Vector2[] Vertices { get { return _Vertices; } set { _Vertices = value; CalculateGeometry(); } }
        protected Vector2[] _Vertices = null;

        public Vector2[] AbsoluteVertices { get { return _AbsoluteVertices; } }
        protected Vector2[] _AbsoluteVertices = null;

        public Vector2[] Polar { get { return _Polar; } }
        protected Vector2[] _Polar = null;

        public Vector2 Position { get { return _Position; } set { _Position = value; } }
        protected Vector2 _Position;

        public double Direction { get { return _Direction; } set { _Direction = value; } }
        protected double _Direction;

        public float Radius { get { return _Radius; } set { _Radius = value; } }
        protected float _Radius = 1.0f;

        public bool GeometryUpToDate { get { return _GeometryUpToDate; } set { _GeometryUpToDate = value; } }
        protected bool _GeometryUpToDate = false;

        public Entity Parent { get { return _Parent; } set { _Parent = value; } }
        protected Entity _Parent;

        public Vector2 UncompensatedTopLeft { get { return _UncompensatedTopLeft; } set { _UncompensatedTopLeft = value; } }
        protected Vector2 _UncompensatedTopLeft;

        public Vector2 UncompensatedSize { get { return _UncompensatedSize; } set { _UncompensatedSize = value; } }
        protected Vector2 _UncompensatedSize;

        public Geometry(Entity parent, Vector2[] vertices, Vector2 position, float direction)
        {
            _Parent = parent;
            _Vertices = vertices;
            _Position = position;
            _Direction = direction;
            CalculateGeometry();
        }

        public static Geometry CreateCircularGeometry(Entity parent, float radius, Vector2 position, float direction)
        {
            Geometry geom = new Geometry(parent, null, position, direction);
            geom._Radius = radius;
            geom.CalculateGeometry();
            return geom;
        }
        public static Geometry CreateCircularGeometry(Entity parent, float radius)
        {
            return CreateCircularGeometry(parent, radius, new Vector2(0, 0), 0.0f);
        }

        public static Geometry CreateRectangularGeometry(Entity parent, float height, float width, Vector2 position, float direction)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(-width, -height) / 2;
            points[1] = new Vector2(width, -height) / 2;
            points[2] = new Vector2(width, height) / 2;
            points[3] = new Vector2(-width, height) / 2;
            return new Geometry(parent, points, position, direction);
        }
        public static Geometry CreateRectangularGeometry(Entity parent, float height, float width)
        {
            return CreateRectangularGeometry(parent, height, width, new Vector2(0, 0), 0.0f);
        }

        protected virtual void CalculateGeometry()
        {
            if (_Vertices == null)
            {
                _UncompensatedTopLeft = new Vector2(-_Radius, -_Radius);
                _UncompensatedSize = new Vector2(2 * _Radius, 2 * _Radius);
                _GeometryUpToDate = true;
                return;
            }
            _UncompensatedTopLeft = new Vector2(MinX(), MinY());
            _UncompensatedSize = new Vector2(MaxX(), MaxY()) - _UncompensatedTopLeft;

            _Polar = new Vector2[_Vertices.Length];
            _AbsoluteVertices = new Vector2[_Vertices.Length];
            for (int i = 0; i < _Vertices.Length; i++)
                _Polar[i] = new Vector2((float)Math.Sqrt(_Vertices[i].X * _Vertices[i].X + _Vertices[i].Y * _Vertices[i].Y),
                    (float)Math.Atan2(_Vertices[i].Y, _Vertices[i].X));
            
            UpdateGeometry();
        }

        protected virtual void UpdateGeometry()
        {
            for (int i = 0; i < _Vertices.Length; i++)
            {
                _AbsoluteVertices[i] = new Vector2(_Polar[i].X * (float)Math.Sin(_Vertices[i].Y + (float)_Direction),
                    _Polar[i].X * (float)Math.Cos(_Vertices[i].Y + (float)_Direction));
                _AbsoluteVertices[i] += (Vector2)_Position;
            }
            _GeometryUpToDate = true;
        }

        public float MinX() 
        {
            if (_Vertices == null)
                return -_Radius;
            float min = 1000000f;
            foreach (Vector2 vec in _Vertices)
                if (vec.X < min)
                    min = vec.X;
            return min;
        }

        public float MaxX()
        {
            if (_Vertices == null)
                return _Radius;
            float max = -1000000f;
            foreach (Vector2 vec in _Vertices)
                if (vec.X > max)
                    max = vec.X;
            return max;
        }

        public float MinY()
        {
            if (_Vertices == null)
                return -_Radius;
            float min = 1000000f;
            foreach (Vector2 vec in _Vertices)
                if (vec.Y < min)
                    min = vec.Y;
            return min;
        }

        public float MaxY()
        {
            if (_Vertices == null)
                return _Radius;
            float max = -1000000f;
            foreach (Vector2 vec in _Vertices)
                if (vec.Y > max)
                    max = vec.Y;
            return max;
        }
    }
}
