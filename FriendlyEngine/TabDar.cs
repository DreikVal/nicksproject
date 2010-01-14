using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class TabDar
    {
        public Texture2D Background;
        public Texture2D Field;
        public List<Texture2D> RadarElements = new List<Texture2D>();
        public List<Vector2> RadarElementPositions = new List<Vector2>();
        public List<Texture2D> RadarElementVariants = new List<Texture2D>();
        public List<Texture2D> RadarMissiles = new List<Texture2D>();
        public List<Vector2> RadarMissilePositions = new List<Vector2>();
        public Dictionary<string, int> PositionInRadarElements = new Dictionary<string,int>();

        public Vector2 Position = Vector2.Zero;
        public bool Visible = false;
        public bool toggled = false;
        public Color eightyWhite = new Color(255, 255, 255, 0f);

        public int Height;
        public int Width;

        public TabDar()
        {

        }
        public void ToggleVisible()
        {
            if(Visible == true)
                Visible = false;
            else if (Visible == false)
            {
                Visible = true;
                eightyWhite = new Color(255, 255, 255, 0.19f);
            }
            toggled = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            spriteBatch.Draw(Background, Position, eightyWhite);
            spriteBatch.Draw(Field, Position, eightyWhite);
            for(int i = 0; i < RadarElements.Count; i++)
            {
                spriteBatch.Draw(RadarElements[i], Position + RadarElementPositions[i], Color.White);
            }
            for (int i = 0; i < RadarMissiles.Count; i++)
            {
                spriteBatch.Draw(RadarMissiles[i], Position + RadarMissilePositions[i], Color.White);
            }
        }
    }
}
