using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AlienShooterGame
{
    class GUIEditor : Screen
    {
        Editor_Gui _Editor_GuiA;
        Editor_Gui _Editor_GuiB;
        Editor_TextureBox _Editor_TextureBox;

        protected Entity _PreviewEntityA;
        protected Entity _PreviewEntityB;
        protected Entity[] _TextureBoxEntities;
        protected Tile _TemporaryTile;
        protected int _TileIndex = 0;

        MouseState mState;

        public GUIEditor(ScreenManager manager)
            : base(manager, "GUIEditor")
        {
            //_ViewPort.Size = new Vector2(800, 440);
            _Editor_GuiA = new Editor_Gui(this);
            _Editor_GuiA.Geometry.Position = new Vector2(554, 397);
            _Editor_GuiB = new Editor_Gui(this);
            _Editor_GuiB.Geometry.Position = new Vector2(574, 417);
            _Editor_TextureBox = new Editor_TextureBox(this);
            _Editor_TextureBox.Hide = true;
            _TextureBoxEntities = new Entity[16];

            for (int i = 0; i < 16; i++)
            {
                _TemporaryTile = Tile.TileGen[(_TileIndex + i) % Tile.TileGen.Length](this, 1, 1, _TileIndex);
                _TextureBoxEntities[i] = new Entity(this);
                _TextureBoxEntities[i].Depth = 0.18f;
                _TextureBoxEntities[i].Geometry = new Geometry(_TextureBoxEntities[i],
                    new Vector2(138 + (i * 30) - (i /4 * 120) , 134 + (i /4 * 30)), 30, 30, 0);
                _TextureBoxEntities[i].Animations.AddAnimation(_TemporaryTile.Animations.Current);
                _TextureBoxEntities[i].Hide = true;
                _TemporaryTile.Dispose();
            }

            _TemporaryTile = Tile.TileGen[_TileIndex](this, 1, 1, _TileIndex);
            _PreviewEntityA = new Entity(this);
            _PreviewEntityA.Animations.AddAnimation(_TemporaryTile.Animations.Current);
            _PreviewEntityA.Geometry = new Geometry(_PreviewEntityA, new Vector2(554, 397), 30, 30, 0);
            _PreviewEntityA.Depth = 0.17f;
            _TemporaryTile.Dispose();

            _TemporaryTile = Tile.TileGen[(_TileIndex + 1) % Tile.TileGen.Length](this, 1, 1, _TileIndex);
            _PreviewEntityB = new Entity(this);
            _PreviewEntityB.Animations.AddAnimation(_TemporaryTile.Animations.Current);
            _PreviewEntityB.Geometry = new Geometry(_PreviewEntityB, new Vector2(574, 417), 30, 30, 0);
            _PreviewEntityB.Depth = 0.18f;
            _TemporaryTile.Dispose();

            Depth = 0.2f;
            _BackBehaviour = ActionOnBack.ExitApplication;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            this.BlocksInput = false;
            this.BlocksUpdates = false;
            this.BlocksVisibility = false;
            this.Lights.Clear();
        }
        public override void HandleInput(Bind bind)
        {
            //base.HandleInput(bind);
            Vector2 mWorldLoc = new Vector2(mState.X / _Manager.Resolution.X * _ViewPort.Size.X + _ViewPort.ActualLocation.X,
                mState.Y / _Manager.Resolution.Y * _ViewPort.Size.Y + _ViewPort.ActualLocation.Y);
            Vector2 diff = Vector2.Zero;

            if (bind.Name.CompareTo("PrimaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    diff = mWorldLoc - _Editor_GuiA.Geometry.Position;
                    if (diff.Length() < _Editor_GuiA.Geometry.CollisionRadius)
                    {
                        _Editor_TextureBox.Hide = false;
                        for (int i = 0; i < 16; i++)
                        {
                            _TextureBoxEntities[i].Hide = false;
                        }
                    }
                    else
                    {
                        diff = mWorldLoc - _Editor_GuiB.Geometry.Position;
                        if (diff.Length() < _Editor_GuiB.Geometry.CollisionRadius)
                        {
                            _Editor_TextureBox.Hide = false;
                            for (int i = 0; i < 16; i++)
                            {
                                _TextureBoxEntities[i].Hide = false;
                            }
                        }
                    }

                    if (_Editor_TextureBox.Hide == false)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            diff = mWorldLoc - _TextureBoxEntities[i].Geometry.Position;
                            if (diff.Length() < _TextureBoxEntities[i].Geometry.CollisionRadius)
                            {
                                Screen screen;
                                EditorScreen world;
                                _Manager.LookupScreen("Editor", out screen);
                                world = (EditorScreen)screen;
                                world._TileIndex = i % Tile.TileGen.Length;
                                _TemporaryTile = Tile.TileGen[world._TileIndex](this, 1, 1, world._TileIndex);
                                _PreviewEntityA.Dispose();
                                _PreviewEntityA = new Entity(this);
                                _PreviewEntityA.Animations.AddAnimation(_TemporaryTile.Animations.Current);
                                _PreviewEntityA.Geometry = new Geometry(_PreviewEntityA, new Vector2(554, 397), 30, 30, 0);
                                _PreviewEntityA.Depth = 0.17f;
                                _TemporaryTile.Dispose();
                            }
                        }
                    }
                }
            }

            if (bind.Name.CompareTo("SecondaryFire") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (_Editor_TextureBox.Hide == false)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            diff = mWorldLoc - _TextureBoxEntities[i].Geometry.Position;
                            if (diff.Length() < _TextureBoxEntities[i].Geometry.CollisionRadius)
                            {
                                Screen screen;
                                EditorScreen world;
                                _Manager.LookupScreen("Editor", out screen);
                                world = (EditorScreen)screen;
                                world._SecondaryIndex = i % Tile.TileGen.Length;
                                _TemporaryTile = Tile.TileGen[world._SecondaryIndex](this, 1, 1, world._SecondaryIndex);
                                _PreviewEntityB.Dispose();
                                _PreviewEntityB = new Entity(this);
                                _PreviewEntityB.Animations.AddAnimation(_TemporaryTile.Animations.Current);
                                _PreviewEntityB.Geometry = new Geometry(_PreviewEntityB, new Vector2(574, 417), 30, 30, 0);
                                _PreviewEntityB.Depth = 0.18f;
                                _TemporaryTile.Dispose();
                            }
                        }
                    }
                }
            }

            if (bind.Name.CompareTo("FlashLight") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    _Editor_TextureBox.Hide = true;
                    for (int i = 0; i < _TextureBoxEntities.GetLength(0); i++)
                    {
                        _TextureBoxEntities[i].Hide = true;
                    }
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            mState = Mouse.GetState();
        }
    }
}