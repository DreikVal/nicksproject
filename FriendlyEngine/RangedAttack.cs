using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class RangedAttack
    {
        Vector2 position = Vector2.Zero;
        public Vector2 direction = Vector2.Zero;
        Rectangle size = new Rectangle(0,0,1,1);
        
        int damage = 100;
        float radius = 1f;
        float angle = 0;
        int range = 10;
        bool wait = false;
        int waitCounter = 0;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle Rect
        {
            get { return size; }
            set { size = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        public float Rotation
        {
            get { return angle; }
            set { angle = value; }
        }

        public float CollisionRadius
        {
            get { return radius; }
            set { radius = (float)Math.Max(value, 1f); }
        }

        public static bool IsVectorColliding(RangedAttack a, List<NPC> b)
        {
            if (a.wait == true)
                return false;

            a.wait = true;
            for (int i = 0; i < a.Range; i++)
            {
                a.direction.X = (float)Math.Cos(a.Rotation);
                a.direction.Y = (float)Math.Sin(a.Rotation);
                a.Position += Vector2.Normalize(a.direction);
                a.Rect = new Rectangle((int)a.Position.X, (int)a.Position.Y, 1, 1);

                foreach(NPC s in b)
                {
                    if(a.Rect.Intersects(s.Bounds))
                    {
                        s.Health -= 100;
                        return true;
                    }
                }
            }
            return false;

        }

        public RangedAttack()
        {

        }

        public void Update(GameTime gameTime)
        {
            if(wait == true && waitCounter <= 25)
                waitCounter++;
            else if (waitCounter >= 25)
            {
                wait = false;
                waitCounter = 0;
            }

        }
    }
}
