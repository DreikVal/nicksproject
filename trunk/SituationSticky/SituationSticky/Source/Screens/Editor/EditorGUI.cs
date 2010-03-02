using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SituationSticky
{
    class EditorGUI : Screen
    {
        public static Vector3 PreviewPositionA = new Vector3(700, 415, 0);
        public static Vector3 PreviewPositionB = new Vector3(715, 430, 0);

        TextureBox_GUI _TextureBox_GUI;
        public bool TextureListShown { get { return !_TextureBox_GUI.Hide; } }
        protected Entity_Quad _PreviewEntityA;
        protected Entity_Quad _PreviewEntityB;
        protected Entity_Quad[] _TextureBoxEntities;
        protected Tile _TemporaryTile;
        protected int _TileIndex = 0;

        public EditorGUI(ScreenManager manager)
            : base(manager, "EditorGUI")
        {
            _TextureBox_GUI = new TextureBox_GUI(this);
            _TextureBox_GUI.Hide = true;
            _TextureBoxEntities = new Entity_Quad[16];
            new Crosshair(this);

            for (int i = 0; i < 16; i++)
            {
                _TemporaryTile = Tile.TileGen[(_TileIndex + i) % Tile.TileGen.Length](this, new Vector3());
                _TextureBoxEntities[i] = new Entity_Quad(_Entities, new Vector3(138 + (i * 30) - (i / 4 * 120), 134 + (i / 4 * 30), 0), new Vector3(30, 30, 0), Vector3.Zero);
                _TextureBoxEntities[i].Depth = 0.18f;
                _TextureBoxEntities[i].Animations.AddAnimation(_TemporaryTile.Animations.Current);
                _TextureBoxEntities[i].Hide = true;
                _TemporaryTile.Dispose();
            }

            _TemporaryTile = Tile.TileGen[_TileIndex](this, new Vector3());
            _PreviewEntityA = new Entity_Quad(_Entities, PreviewPositionA, new Vector3(30, 30, 0), Vector3.Zero);
            _PreviewEntityA.Animations.AddAnimation(_TemporaryTile.Animations.Current);
            _PreviewEntityA.Depth = 0.17f;
            _TemporaryTile.Dispose();

            _TemporaryTile = Tile.TileGen[(_TileIndex + 1) % Tile.TileGen.Length](this, new Vector3());
            _PreviewEntityB = new Entity_Quad(_Entities, PreviewPositionB, new Vector3(30, 30, 0), Vector3.Zero);
            _PreviewEntityB.Animations.AddAnimation(_TemporaryTile.Animations.Current);
            _PreviewEntityB.Depth = 0.18f;
            _TemporaryTile.Dispose();

            // Screen settings
            _Depth = 0.2f;
            _FadeInTime = 0.0f;
            _FadeOutTime = 0.0f;
            _BlocksInput = false;
            _BlocksUpdates = false;
            _BlocksVisibility = false;
            _ViewPort.Size = new Vector2(800, 440);
        }
        public override void HandleInput(Bind bind)
        {
            //base.HandleInput(bind);
            MouseState mState = Mouse.GetState();
            Vector3 mWorldLoc = new Vector3(mState.X / _Manager.Resolution.X * _ViewPort.Size.X + _ViewPort.ActualLocation.X,
                mState.Y / _Manager.Resolution.Y * _ViewPort.Size.Y + _ViewPort.ActualLocation.Y, 0);
            Vector3 diff = Vector3.Zero;

            if (bind.Name.CompareTo("PRI") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (_TextureBox_GUI.Hide == false)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            diff = mWorldLoc - _TextureBoxEntities[i].Position;
                            if (diff.Length() < _TextureBoxEntities[i].Radius)
                            {
                                EditorScreen editor = _Manager.GetScreen("Editor") as EditorScreen;
                                editor.TileIndex = i % Tile.TileGen.Length;
                                _TemporaryTile = Tile.TileGen[editor.TileIndex](this, new Vector3());
                                _PreviewEntityA.Dispose();
                                _PreviewEntityA = new Entity_Quad(_Entities, PreviewPositionA, new Vector3(30, 30, 0), Vector3.Zero);
                                _PreviewEntityA.Animations.AddAnimation(_TemporaryTile.Animations.Current);
                                _PreviewEntityA.Depth = 0.17f;
                                _TemporaryTile.Dispose();
                            }
                        }
                    }
                }
            }

            if (bind.Name.CompareTo("SEC") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (_TextureBox_GUI.Hide == false)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            diff = mWorldLoc - _TextureBoxEntities[i].Position;
                            if (diff.Length() < _TextureBoxEntities[i].Radius)
                            {
                                EditorScreen editor = _Manager.GetScreen("Editor") as EditorScreen;
                                editor.SecondaryIndex = i % Tile.TileGen.Length;
                                _TemporaryTile = Tile.TileGen[editor.SecondaryIndex](this, new Vector3());
                                _PreviewEntityB.Dispose();
                                _PreviewEntityB = new Entity_Quad(_Entities, PreviewPositionB, new Vector3(30, 30, 0), Vector3.Zero);
                                _PreviewEntityB.Animations.AddAnimation(_TemporaryTile.Animations.Current);
                                _PreviewEntityB.Depth = 0.17f;
                                _TemporaryTile.Dispose();
                            }
                        }
                    }
                }
            }

            if (bind.Name.CompareTo("FLA") == 0)
            {
                if (bind.State == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    if (_TextureBox_GUI.Hide)
                        ShowTextureList();
                    else
                        HideTextureList();                   
                }
            }
        }

        protected virtual void ShowTextureList()
        {
            _TextureBox_GUI.Hide = false;
            for (int i = 0; i < 16; i++)
                _TextureBoxEntities[i].Hide = false;
        }

        protected virtual void HideTextureList()
        {
            _TextureBox_GUI.Hide = true;
            for (int i = 0; i < 16; i++)
                _TextureBoxEntities[i].Hide = true;
        }
    }
}