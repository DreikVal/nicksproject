using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooterGame
{
    public class FloatingText : Entity
    {
        protected float _Speed;
        protected float _Damping;
        protected String _Text;
        protected SpriteFont _Font;
        protected int _LifeTime;

        public FloatingText(Screen parent, Vector2 position, float speed, float damping, String text, String font, Color colour, int lifeTime) : base(parent) 
        { 
            _Speed = speed;
            _Text = text;
            _Font = Application.AppReference.Content.Load<SpriteFont>(font);
            _ColourOverlay = colour;
            _LifeTime = lifeTime;
            _Damping = damping;
            _Geometry.Position = position;
        }

        public override string Initialize()
        {
            // Create collision geometry for the marine
            _Geometry = new Geometry(this, new Vector2(), 50.0f, 28.0f, 0.0f);

            // Set crosshair to front of screen
            _Depth = 0.15f;

            // Return the name for this class
            return "FloatingText";
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            if (Disposed) return;

            // Draw the message string
            if (_Text != null && _Font != null)
            {
                Vector2 Length = _Font.MeasureString(_Text);
                Vector2 TopLeft_Pixels = _Parent.ViewPort.Transform_UnitPosition_To_PixelPosition(_Geometry.Position) - Length/2;
                Vector2 Size_Pixels = _Parent.ViewPort.Transform_UnitSize_To_PixelSize(Length);
                Vector2 Scale = Size_Pixels / Length;

                batch.DrawString(_Font, _Text, TopLeft_Pixels, _ColourOverlay, 0.0f, new Vector2(0, 0), Scale.X, SpriteEffects.None, Depth);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //base.Update(time);
            _LifeTime -= time.ElapsedGameTime.Milliseconds;
            if (_LifeTime <= 0) Dispose();
            _Speed *= _Damping;

            _Geometry.Position.X += (float)Math.Sin(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
            _Geometry.Position.Y += -(float)Math.Cos(_Geometry.Direction) * _Speed * time.ElapsedGameTime.Milliseconds;
        }
    }
}
