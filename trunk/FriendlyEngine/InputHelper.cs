using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public static class InputHelper
    {
        static KeyboardState newState;
        static KeyboardState oldState;
        static MouseState mState;
        static Vector2 newMouse = Vector2.Zero;
        static Vector2 oldMouse = Vector2.Zero;

        public static void Update()
        {
            mState = Mouse.GetState();
            newMouse = new Vector2(mState.X, mState.Y);

            oldState = newState;
            newState = Keyboard.GetState();

            if (newMouse != oldMouse)
            {
                newMouse -= oldMouse;
            }

            oldMouse = new Vector2(mState.X, mState.Y);
        }

        public static bool IsNewPress(Keys key)
        {
            return (newState.IsKeyDown(key) && oldState.IsKeyUp(key));
        }

        public static bool IsKeyDown(Keys key)
        {
            return newState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return newState.IsKeyUp(key);
        }

        public static bool IsLeftButton()
        {
            if (mState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }
        public static bool IsLeftButtonReleased()
        {
            if (mState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        public static void SetMouseXY(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }
        public static void SetMouseXY(Vector2 xy)
        {
            Mouse.SetPosition((int)xy.X, (int)xy.Y);
        }
        public static Vector2 MouseXY()
        {
            if (mState != null)
                return new Vector2(mState.X, mState.Y);
            else
                return new Vector2(0, 0);
        }

        public static int MouseX()
        {
            if (mState != null)
                return mState.X;
            else
                return 0;
        }
        public static int MouseY()
        {
            if (mState != null)
                return mState.Y;
            else
                return 0;
        }

        public static Vector2 MouseXYTracked()
        {
            if (mState != null)
                return new Vector2(mState.X + newMouse.X, mState.Y + newMouse.Y);
            else
                return new Vector2(0, 0);
        }
    }
}
