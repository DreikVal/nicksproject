using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    class TilePosition
    {
        public int Index { get { return index; } set { index = value; } }
        protected int index = 0;
        public Vector2 Position = Vector2.Zero;

        public Color ColourOverlay { get { return _ColourOverlay; } set { _ColourOverlay = value; } }
        public Color ActualColour { get { return _ActualColour; } set { _ActualColour = value; } }
        protected Color _ColourOverlay = Color.White;
        protected Color _ActualColour = Color.White;

        public TilePosition()
        {

        }

        public TilePosition(int Indexx)
        {
            index = Indexx;
        }

        public void Update(Microsoft.Xna.Framework.GameTime time, WorldScreen scr)
        {
            Vector4 _Lighting = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            foreach (LightSource light in scr.Lights)
            {
                if (!light.Active) continue;

                float x_diff = Position.X - light.Position.X;
                float y_diff = Position.Y - light.Position.Y;
                float dist = (float)Math.Sqrt((x_diff * x_diff) + (y_diff * y_diff));
                float val = 1.0f - (dist / light.Range);
                if (val < 0.0f) val = 0.0f;

                float angle = (float)Math.Atan2(y_diff, x_diff);
                float angle_diff = (float)Math.Abs(light.Direction - Math.PI / 2 - angle);
                if (angle_diff > Math.PI)
                    angle_diff = 2 * (float)Math.PI - angle_diff;
                float angle_val = 1.0f - (angle_diff / light.Radius);

                Vector4 pre = light.Colour.ToVector4();
                Vector4 dis = new Vector4(val, val, val, 1.0f);
                Vector4 ang = new Vector4(angle_val, angle_val, angle_val, 1.0f);
                Vector4 result = pre * dis * ang;
                if (result.X < 0.0f) result.X = 0.0f;
                if (result.Y < 0.0f) result.Y = 0.0f;
                if (result.Z < 0.0f) result.Z = 0.0f;
                if (result.W < 0.0f) result.W = 0.0f;
                _Lighting += result;
            }
            Vector4 _VectorOverlay = _ColourOverlay.ToVector4();

            _ActualColour = new Color(_VectorOverlay * _Lighting);
        }
    }
}
