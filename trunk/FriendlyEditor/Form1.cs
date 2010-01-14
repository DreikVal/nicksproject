using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FriendlyEngine;

namespace FriendlyEditor
{
    using Image = System.Drawing.Image;
    using System.Xml;

    public partial class Form1 : Form
    {
        const int MaxFillCells = 1000;

        string[] imageExtensions = new string[]
            {
                ".jpg", ".png", ".tga",
            };

        int maxWidth = 0, maxHeight = 0;
        string directory;
        string documentName;
        Game game = new Game();

        SpriteBatch spriteBatch;
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan Ts = new TimeSpan();
        Texture2D tileTexture;
        Texture2D whiteTexture;

        Camera camera = new Camera();
        TileLayer currentLayer;
        CollisionLayer collLayer;
        int cellX, cellY;
        bool mouseLeftDown = false;
        bool mouseRightDown = false;
        bool renderMap = true;
       // bool previewBoxIsAnimating = false;
        TileMap tileMap = new TileMap();

        Dictionary<int, Texture2D> textureDict = new Dictionary<int, Texture2D>();
        Dictionary<int, string> textureNames = new Dictionary<int, string>();
        Dictionary<int, AnimatedSprite> animatedDict = new Dictionary<int, AnimatedSprite>();
        Dictionary<int, string> animatedNames = new Dictionary<int, string>();
        Dictionary<int, Texture2D> doodadDict = new Dictionary<int, Texture2D>();
        Dictionary<int, string> doodadNames = new Dictionary<int, string>();
        Dictionary<int, TileLayer> layerDict = new Dictionary<int, TileLayer>();
        Dictionary<int, string> layerNames = new Dictionary<int, string>();
        Dictionary<string, Image> previewDict = new Dictionary<string, Image>();
        int collisionIndex = 0;

        int fillCounter = MaxFillCells;

        public GraphicsDevice GraphicsDevice
        {
            get { return tileDisplay1.GraphicsDevice; }
        }

        public Form1()
        {
            InitializeComponent();
            
            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate(); };

            Mouse.WindowHandle = tileDisplay1.Handle;
            
        }

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            whiteTexture = Texture2D.FromFile(GraphicsDevice, "Content/whitetex.png");
            tileTexture = Texture2D.FromFile(GraphicsDevice, "Content/tile.png");
        }


        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            Logic();
            Render();
            
        }

        public void FillCell(int x, int y, int desiredIndex)
        {
            int oldIndex = currentLayer.GetCellIndex(x, y);
            currentLayer.SetCellIndex(x, y, desiredIndex);

            if (desiredIndex == oldIndex || fillCounter == 0)
                return;

            fillCounter--;

            if(x > 0 && currentLayer.GetCellIndex(x - 1, y) == oldIndex)
                FillCell(x -1, y, desiredIndex);
            if (x < currentLayer.Width - 1 && currentLayer.GetCellIndex(x + 1, y) == oldIndex)
                FillCell(x + 1, y, desiredIndex);
            if (y > 0 && currentLayer.GetCellIndex(x, y -1) == oldIndex)
                FillCell(x, y - 1, desiredIndex);
            if (y < currentLayer.Height - 1 && currentLayer.GetCellIndex(x, y + 1) == oldIndex)
                FillCell(x, y + 1, desiredIndex);
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (currentLayer != null)
                currentLayer.Alpha = (float)alphaSlider.Value / 100f;
        }

        private void tileDisplay1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseLeftDown = true;
            else if (e.Button == MouseButtons.Right)
                mouseRightDown = true;
        }

        private void tileDisplay1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseLeftDown = false;
            if (e.Button == MouseButtons.Right)
                mouseRightDown = false;
        }

        private void Logic()
        {
            stopWatch.Stop();
            Ts += stopWatch.Elapsed;
            tileDisplay1.Update();
            stopWatch.Reset();
            stopWatch.Start();
            InputHelper.Update();

            if (InputHelper.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Up)&& vScrollBar1.Value > 0)
            {
                vScrollBar1.Focus();
                vScrollBar1.Value -= vScrollBar1.SmallChange;
            }
            if (InputHelper.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Down) && vScrollBar1.Value < vScrollBar1.Maximum)
            {
                vScrollBar1.Focus();
                vScrollBar1.Value += vScrollBar1.SmallChange;
            }
            if (InputHelper.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Left) && hScrollBar1.Value > 0)
            {
                hScrollBar1.Focus();
                hScrollBar1.Value -= hScrollBar1.SmallChange;
            }
            if (InputHelper.IsNewPress(Microsoft.Xna.Framework.Input.Keys.Right) && hScrollBar1.Value < hScrollBar1.Maximum)
            {
                hScrollBar1.Focus();
                hScrollBar1.Value += hScrollBar1.SmallChange;
            }


            camera.Position.X = hScrollBar1.Value * Engine.TileWidth;
            camera.Position.Y = vScrollBar1.Value * Engine.TileHeight;

            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (mx >= 0 && mx < tileDisplay1.Width &&
                my >= 0 && my < tileDisplay1.Height)
            {
                cellX = mx / Engine.TileWidth;
                cellY = my / Engine.TileHeight;

                cellX += hScrollBar1.Value;
                cellY += vScrollBar1.Value;


                if (collisionStripButton.Checked && collLayer != null)
                {
                    cellX = (int)MathHelper.Clamp(cellX, 0, collLayer.Width - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, collLayer.Height - 1);

                    if (mouseLeftDown)
                    {
                        if (collisionListBox.SelectedItem != null)
                        {
                            collLayer.SetCellIndex(cellX, cellY, collisionIndex);
                        }
                    }
                    if (mouseRightDown)
                    {
                        if (collisionListBox.SelectedItem != null)
                        {
                            collLayer.SetCellIndex(cellX, cellY, 0);
                        }
                    }
                }

                if (collisionStripButton.Checked == false && collLayer != null)
                {
                    cellX = (int)MathHelper.Clamp(cellX, 0, collLayer.Width - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, collLayer.Height - 1);

                    if (mouseLeftDown)
                    {
                        if (doodadListBox.SelectedItem != null)
                        {
                            Texture2D texture = doodadDict[doodadListBox.SelectedIndex];

                            int index = tileMap.DoodadLayer.IsUsingTexture(texture);

                            if (index == -1)
                            {
                                tileMap.DoodadLayer.AddTexture(texture);
                                index = tileMap.DoodadLayer.IsUsingTexture(texture);
                            }
                            else
                                tileMap.DoodadLayer.SetCellIndex(cellX, cellY, index);
                        }
                    }
                    if (mouseRightDown)
                    {
                        if (doodadListBox.SelectedItem != null)
                        {
                            tileMap.DoodadLayer.SetCellIndex(cellX, cellY, -1);
                        }
                    }
                    
                }

                if (currentLayer != null && collisionStripButton.Checked == false)
                {
                    cellX = (int)MathHelper.Clamp(cellX, 0, currentLayer.Width - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, currentLayer.Height - 1);

                    if (mouseLeftDown)
                    {
                        if (textureListBox.SelectedItem != null)
                        {
                            Texture2D texture = textureDict[textureListBox.SelectedIndex];

                            int index = tileMap.ReturnTextureIndex(texture);

                            if (index == -1)
                            {
                            }

                            if (fillStripButton.Checked)
                            {
                                fillCounter = MaxFillCells;
                                FillCell(cellX, cellY, index);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, index);
                        }
                    }
                    if (mouseRightDown)
                    {
                        if (textureListBox.SelectedItem != null)
                        {
                            if (fillStripButton.Checked)
                            {
                                fillCounter = MaxFillCells;
                                FillCell(cellX, cellY, -1);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, -1);
                        }
                    }
                }
                else
                {
                    cellX = cellY = -1;
                }
            }
            tileMap.Update(camera, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Ts);
        }

        private void Render()
        {
            GraphicsDevice.Clear(Color.Black);
            if (renderMap == true)
            {

                foreach (TileLayer layer in tileMap.Layers)
                {
                    layer.Draw(spriteBatch, camera, tileMap.tileTextures);

                    if (!showGridToolStripMenuItem.Checked || showAllLayersToolStripMenuItem.Checked)
                    {
                    }
                    else
                    {
                        spriteBatch.Begin();
                        for (int y = 0; y < layer.Height; y++)
                        {
                            for (int x = 0; x < layer.Width; x++)
                            {
                                if (layer.GetCellIndex(x, y) == -1)
                                {
                                    spriteBatch.Draw(
                                      tileTexture,
                                      new Rectangle(
                                          x * Engine.TileWidth - (int)camera.Position.X,
                                          y * Engine.TileHeight - (int)camera.Position.Y,
                                          Engine.TileWidth,
                                          Engine.TileHeight),
                                      Color.White);
                                }

                            }
                        }

                        spriteBatch.End();
                    }

                    if (!showAllLayersToolStripMenuItem.Checked && layer == currentLayer)
                        break;
                }

                if (doodadListBox.SelectedItem != null || showAllLayersToolStripMenuItem.Checked)
                {
                    tileMap.DoodadLayer.Draw(spriteBatch, camera);
                }

                if (collisionStripButton.Checked && whiteTexture != null)
                {
                    collLayer.Draw(spriteBatch, camera, whiteTexture);
                }

                if (currentLayer != null || collisionStripButton.Checked || doodadListBox.SelectedItem != null)
                {

                    if (cellX != -1 && cellY != -1)
                    {
                        spriteBatch.Begin();

                        spriteBatch.Draw(
                             tileTexture,
                             new Rectangle(
                                   cellX * Engine.TileWidth - (int)camera.Position.X,
                                   cellY * Engine.TileHeight - (int)camera.Position.Y,
                                   Engine.TileWidth,
                                   Engine.TileHeight),
                             Color.Red);

                        spriteBatch.End();
                    }
                }
                //spriteBatch.Begin();
                //foreach (AnimatedSprite a in tileMap.animatedTextures)
                //    a.Draw(spriteBatch);
                //spriteBatch.End();
            }
        }

        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMapForm form = new NewMapForm();

            form.ShowDialog();

            if (form.OKPressed)
            {

                NukeMap();

                TileMap tileMAP = new TileMap();
                int width = int.Parse(form.widthTextBox.Text);
                int height = int.Parse(form.heightTextBox.Text);

                for (int i = 0; i < form.layerUpDown.Value; i++)
                {
                    tileMAP.Layers.Add(new TileLayer(
                        width,
                        height));

                    layerDict.Add(i, tileMAP.Layers[i]);
                    layerNames.Add(i, ("Layer" + i));
                    layerListBox.Items.Add(("Layer" + i));
                }

                tileMAP.CollisionLayer = new CollisionLayer(width, height);
                collLayer = tileMAP.CollisionLayer;
                tileMAP.DoodadLayer = new DoodadLayer(width, height);
                //doodadLayer = tileMAP.DoodadLayer;
                tileMap = tileMAP;

                AdjustScrollBars();
            }
        }

        private void NukeMap()
        {
            tileMap = null;
            collLayer = null;


            layerListBox.Items.Clear();
            textureListBox.Items.Clear();
            doodadListBox.Items.Clear();
            collisionListBox.Items.Clear();
            collisionStripButton.Visible = false;
            if(tileMap != null)
            tileMap.ClearTextureList();
            layerDict.Clear();
            layerNames.Clear();
            textureDict.Clear();
            textureNames.Clear();
            doodadDict.Clear();
            doodadNames.Clear();
            previewDict.Clear();
            currentLayer = null;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Level File|*.level";
            openFileDialog1.InitialDirectory = contentPathTextBox.Text;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                directory = Path.GetDirectoryName(openFileDialog1.FileName);
                documentName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                NukeMap();
                XmlDocument jesus = new XmlDocument();
                jesus.Load(openFileDialog1.FileName);
                tileMap = TileMap.ProcessFile(jesus, textureNames, doodadNames, layerNames);

                if (tileMap.CollisionLayer != null)
                {
                    collLayer = tileMap.CollisionLayer;
                    foreach (KeyValuePair<string, int> a in collLayer.Types)
                    {
                        collisionListBox.Items.Add(a.Key);
                    }
                    collisionStripButton.Visible = true;
                }

                for (int i = 0; i < textureNames.Count; i++)
                {
                    string filename = textureNames[i];
                    filename = filename.Insert(0, contentPathTextBox.Text + "\\");

                    Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);
                    Image image = Image.FromFile(filename);

                    filename = filename.Replace(contentPathTextBox.Text + "\\", "");
                    previewDict.Add(filename, image);
                    filename = filename.Remove(filename.LastIndexOf("."));

                    textureListBox.Items.Add(filename);
                    tileMap.AddTexture(texture);
                    textureDict.Add(i, texture);
                }

                for (int i = 0; i < doodadNames.Count; i++)
                {
                    string filename = doodadNames[i];
                    filename = filename.Insert(0, contentPathTextBox.Text + "\\");

                    Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);
                    Image image = Image.FromFile(filename);

                    filename = filename.Replace(contentPathTextBox.Text + "\\", "");
                    previewDict.Add(filename, image);
                    filename = filename.Remove(filename.LastIndexOf("."));

                    doodadListBox.Items.Add(filename);
                    tileMap.DoodadLayer.AddTexture(texture);
                    doodadDict.Add(i, texture);
                }
                for (int i = 0; i < layerNames.Count; i++)
                {
                    string filename = layerNames[i];

                    layerListBox.Items.Add(filename);
                    layerDict.Add(i, tileMap.Layers[i]);
                }
                AdjustScrollBars();
            }
        }

        private void AdjustScrollBars()
        {
            if (tileMap.GetWidthInPixels() > tileDisplay1.Width)
            {
                maxWidth = (int)Math.Max(tileMap.GetWidth(), maxWidth);
                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = maxWidth;
            }
            else
            {
                maxWidth = 0;
                hScrollBar1.Visible = false;
            }
            if (tileMap.GetHeightInPixels() > tileDisplay1.Height)
            {
                maxHeight = (int)Math.Max(tileMap.GetHeight(), maxHeight);
                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = maxHeight;
            }
            else
            {
                maxHeight = 0;
                vScrollBar1.Visible = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Level File|*.level";
            if (directory != null)
                saveFileDialog1.InitialDirectory = directory;
            else
                saveFileDialog1.InitialDirectory = contentPathTextBox.Text;
            if (documentName != null)
                saveFileDialog1.FileName = documentName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                tileMap.Save(filename, textureNames, doodadNames, tileMap);
            }
            renderMap = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                doodadListBox.SelectedItem = null;
                collisionListBox.SelectedItem = null;
                texturePreviewBox.Image = previewDict[textureNames[textureListBox.SelectedIndex]];
                //animationListBox.SelectedItem = null;
            }
        }

        private void layerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                currentLayer = layerDict[layerListBox.SelectedIndex];
                alphaSlider.Value = (int)(currentLayer.Alpha * 100);
                doodadListBox.SelectedItem = null;
            }
        }

        private void collisionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (collisionListBox.SelectedItem != null)
            {
                int i = 0;
                collLayer.Types.TryGetValue(collisionListBox.SelectedItem as string, out i);
                collisionIndex = i;
                doodadListBox.SelectedItem = null;
                textureListBox.SelectedItem = null;
            }
        }

        //private void addLayerButton_Click(object sender, EventArgs e)
        //{
        //    NewLayerForm form = new NewLayerForm();

        //    form.ShowDialog();

        //    if (form.OKPressed)
        //    {
        //        TileLayer tileLayer = new TileLayer(
        //            int.Parse(form.widthTextBox.Text),
        //            int.Parse(form.heightTextBox.Text));

        //        layerDict.Add(form.nameTextBox.Text, tileLayer);
        //        tileMap.Layers.Add(tileLayer);
        //        layerListBox.Items.Add(form.nameTextBox.Text);

        //        AdjustScrollBars();
        //    }
        //}

        //private void removeLayerButton_Click(object sender, EventArgs e)
        //{
        //    if (currentLayer != null)
        //    {
        //        string filename = layerListBox.SelectedItem.ToString();

        //        tileMap.Layers.Remove(currentLayer);
        //        layerDict.Remove(filename);
        //        layerListBox.Items.Remove(layerListBox.SelectedItem);

        //        currentLayer = null;

        //        AdjustScrollBars();
        //    }
        //}

        private void addTextureButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPEG File|*.jpg|PING File|*.png|TGA File|*.tga";
            openFileDialog1.InitialDirectory = contentPathTextBox.Text;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fullFilename in openFileDialog1.FileNames)
                {
                    string filename = fullFilename;

                    Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);

                    Image image = Image.FromFile(filename);

                    filename = filename.Replace(contentPathTextBox.Text + "\\", "");

                    int index = textureDict.Count;
                    textureNames.Add(index, filename);
                    tileMap.AddTexture(texture);
                    textureDict.Add(index, texture);
                    previewDict.Add(filename, image);

                    filename = filename.Remove(filename.LastIndexOf("."));

                    index = previewDict.Count;

                    textureListBox.Items.Add(filename);
                }
            }
        }

        private void removeTextureButton_Click(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                int texIndex = textureListBox.SelectedIndex;

                foreach (TileLayer layer in tileMap.Layers)
                    if (layer.HasIndex(tileMap.ReturnTextureIndex(textureDict[texIndex])) != -1)
                        layer.ReplaceIndex(layer.HasIndex(tileMap.ReturnTextureIndex(textureDict[texIndex])), -1);

                tileMap.RemoveTexture(textureDict[texIndex], tileMap);
                previewDict.Remove(textureNames[texIndex]);
                textureDict.Remove(texIndex);
                textureNames.Remove(texIndex);
                textureListBox.Items.Remove(textureListBox.SelectedItem);
                texturePreviewBox.Image = null;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "C:\\Users\\Ath3rl3y\\Documents\\Visual Studio 2008\\Projects\\FriendlyGame\\MusicShmup\\Content";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                contentPathTextBox.Text = folderBrowserDialog1.SelectedPath;
            else
                Close();
        }

        private void layerPropButton_Click(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                Props propsform = new Props();

                propsform.NameText = layerListBox.SelectedIndex.ToString();
                propsform.WidthText = layerDict[int.Parse(propsform.NameText)].Width.ToString();
                propsform.HeightText = layerDict[int.Parse(propsform.NameText)].Height.ToString();

                propsform.ShowDialog();
            }
        }

        private void layerUpButton_Click(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null && layerListBox.Items.IndexOf(layerListBox.SelectedItem) != 0)
            {
                int selectedItem = 0;
                object movingItem = null;
                object removedItem = null;
                TileLayer movingLayer = null;
                TileLayer removedLayer = null;

                selectedItem = layerListBox.Items.IndexOf(layerListBox.SelectedItem);
                movingItem = layerListBox.Items[selectedItem];
                removedItem = layerListBox.Items[selectedItem - 1];
                layerListBox.Items.RemoveAt(selectedItem);
                layerListBox.Items.RemoveAt(selectedItem - 1);
                layerListBox.Items.Insert(selectedItem - 1, movingItem);
                layerListBox.Items.Insert(selectedItem, removedItem);

                movingLayer = tileMap.Layers[selectedItem];
                removedLayer = tileMap.Layers[selectedItem - 1];
                tileMap.Layers.RemoveAt(selectedItem);
                tileMap.Layers.RemoveAt(selectedItem - 1);
                tileMap.Layers.Insert(selectedItem - 1, movingLayer);
                tileMap.Layers.Insert(selectedItem, removedLayer);
            }
        }

        private void layerDownButton_Click(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null && layerListBox.Items.IndexOf(layerListBox.SelectedItem) != layerListBox.Items.Count - 1)
            {
                int selectedItem = 0;
                object movingItem = null;
                object removedItem = null;
                TileLayer movingLayer = null;
                TileLayer removedLayer = null;

                selectedItem = layerListBox.Items.IndexOf(layerListBox.SelectedItem);
                movingItem = layerListBox.Items[selectedItem];
                removedItem = layerListBox.Items[selectedItem + 1];
                layerListBox.Items.RemoveAt(selectedItem + 1);
                layerListBox.Items.RemoveAt(selectedItem);
                layerListBox.Items.Insert(selectedItem, removedItem);
                layerListBox.Items.Insert(selectedItem + 1, movingItem);

                movingLayer = tileMap.Layers[selectedItem];
                removedLayer = tileMap.Layers[selectedItem + 1];
                tileMap.Layers.RemoveAt(selectedItem + 1);
                tileMap.Layers.RemoveAt(selectedItem);
                tileMap.Layers.Insert(selectedItem, removedLayer);
                tileMap.Layers.Insert(selectedItem + 1, movingLayer);
            }
        }

        private void addCollLayer_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Visible == false)
            {
                NewLayerForm form = new NewLayerForm();

                form.ShowDialog();

                if (form.OKPressed)
                {
                    collLayer = new CollisionLayer(
                        int.Parse(form.widthTextBox.Text),
                        int.Parse(form.heightTextBox.Text));

                    tileMap.CollisionLayer = collLayer;
                    collLayer.Name = form.nameTextBox.Text;

                    foreach (string t in collLayer.Types.Keys)
                    {
                        collisionListBox.Items.Add(t);
                    }

                    collisionStripButton.Visible = true;

                    AdjustScrollBars();
                }
            }
        }

        private void removeCollLayer_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Visible == true && collisionListBox.SelectedItem.ToString() == collLayer.Name)
            {
                collisionStripButton.Visible = false;
                collisionStripButton.Checked = false;
                collLayer = null;
                collisionListBox.Items.Clear();
            }
        }

        private void collisionProperties_Click(object sender, EventArgs e)
        {
            if (collisionListBox.SelectedItem == collisionListBox.Items[0] && collisionListBox.SelectedItem != null)
            {
                Props propsform = new Props();

                propsform.NameText = collisionListBox.SelectedItem.ToString();
                propsform.WidthText = collLayer.Width.ToString();
                propsform.HeightText = collLayer.Height.ToString();

                propsform.ShowDialog();
            }
        }

        private void coll0Transparency_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Checked)
            {
                Vector4 color;
                for (int i = 0; i < collLayer.GetColorCount; i++)
                {
                    color = collLayer.GetColors(i).ToVector4();
                    color.W = 1f;
                    collLayer.SetColors(color, i);
                }
            }
        }

        private void col25Transparency_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Checked)
            {
                Vector4 color;
                for (int i = 0; i < collLayer.GetColorCount; i++)
                {
                    color = collLayer.GetColors(i).ToVector4();
                    color.W = .75f;
                    collLayer.SetColors(color, i);
                }
            }
        }

        private void coll50Transparency_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Checked)
            {
                Vector4 color;
                for (int i = 0; i < collLayer.GetColorCount; i++)
                {
                    color = collLayer.GetColors(i).ToVector4();
                    color.W = .50f;
                    collLayer.SetColors(color, i);
                }
            }
        }

        private void coll75Transparency_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Checked)
            {
                Vector4 color;
                for (int i = 0; i < collLayer.GetColorCount; i++)
                {
                    color = collLayer.GetColors(i).ToVector4();
                    color.W = .25f;
                    collLayer.SetColors(color, i);
                }
            }
        }

        private void coll100Transparency_Click(object sender, EventArgs e)
        {
            if (collisionStripButton.Checked)
            {
                Vector4 color;
                for (int i = 0; i < collLayer.GetColorCount; i++)
                {
                    color = collLayer.GetColors(i).ToVector4();
                    color.W = 0f;
                    collLayer.SetColors(color, i);
                }
            }
        }

        private void fillStripButton_Click(object sender, EventArgs e)
        {
            collisionStripButton.Checked = false;
        }

        private void collisionStripButton_Click(object sender, EventArgs e)
        {
            fillStripButton.Checked = false;
        }

        private void doodadListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doodadListBox.SelectedItem != null)
            {
                textureListBox.SelectedItem = null;
                collisionListBox.SelectedItem = null;
                texturePreviewBox.Image = previewDict[doodadNames[doodadListBox.SelectedIndex]];
            }
        }

        private void layerListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void collisionListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void doodadListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void textureListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void addDoodadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PING File|*.png|JPEG File|*.jpg|TGA File|*.tga";
            openFileDialog1.InitialDirectory = contentPathTextBox.Text;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fullFilename in openFileDialog1.FileNames)
                {
                    string filename = fullFilename;

                    Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);
                    Image image = Image.FromFile(filename);

                    filename = filename.Replace(contentPathTextBox.Text + "\\", "");


                    int index = doodadDict.Count;
                    doodadDict.Add(index, texture);
                    doodadNames.Add(index, filename);
                    previewDict.Add(filename, image);

                    filename = filename.Remove(filename.LastIndexOf("."));

                    doodadListBox.Items.Add(filename);
                    tileMap.DoodadLayer.AddTexture(texture);
                }
            }
        }

        private void removeDoodadButton_Click(object sender, EventArgs e)
        {
            if (doodadListBox.SelectedItem != null)
            {
                int doodex = doodadListBox.SelectedIndex;

                if (tileMap.DoodadLayer.IsUsingTexture(textureDict[doodex]) != -1)
                    tileMap.DoodadLayer.RemoveTexture(textureDict[doodex]);

                doodadDict.Remove(doodex);
                previewDict.Remove(doodadNames[doodex]);
                doodadNames.Remove(doodex);
                doodadListBox.Items.Remove(textureListBox.SelectedItem);

                texturePreviewBox.Image = null;

            }
        }

        private void showPreviewBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showPreviewBoxToolStripMenuItem.Checked)
                texturePreviewBox.Visible = true;
            else if (showPreviewBoxToolStripMenuItem.Checked == false)
                texturePreviewBox.Visible = false;
        }

        //private void animationAdd_Click(object sender, EventArgs e)
        //{
        //    openFileDialog1.Filter = "JPEG File|*.jpg|PING File|*.png|TGA File|*.tga";
        //    openFileDialog1.InitialDirectory = contentPathTextBox.Text;
        //    openFileDialog1.Multiselect = true;

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        foreach (string fullFilename in openFileDialog1.FileNames)
        //        {
        //            string filename = fullFilename;

        //            Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);
        //            Image image = Image.FromFile(filename);
        //            AnimatedSprite As = new AnimatedSprite(texture);

        //            filename = filename.Replace(contentPathTextBox.Text + "\\", "");
        //            filename = filename.Remove(filename.LastIndexOf("."));

        //            AnimatedSpriteForm asform = new AnimatedSpriteForm();

        //            asform.ShowDialog();

        //            if (asform.OKPressed)
        //            {
        //                FrameAnimation newFrame = new FrameAnimation(int.Parse(asform.FrameText), int.Parse(asform.WidthText), 
        //                    int.Parse(asform.HeightText), int.Parse(asform.xOffsetText), int.Parse(asform.yOffsetText));
        //                newFrame.FramesPerSecond = int.Parse(asform.FramesPerSecondText);
        //                As.Animations.Add(asform.NameText, newFrame);
        //                As.CurrentAnimationName = asform.NameText;

        //                animationListBox.Items.Add(filename);
        //                tileMap.AddAnimatedSprite(As);
        //                animatedDict.Add(filename, As);
        //                previewDict.Add(filename, image);
        //            }
        //        }
        //    }
        //}

        private void animationProps_Click(object sender, EventArgs e)
        {
            if (animationListBox.SelectedItem != null)
            {
                if (tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation != null)
                {
                    AnimatedSpriteForm propsform = new AnimatedSpriteForm();

                    propsform.NameText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimationName.ToString();
                    propsform.FrameText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.NumberOfFrames.ToString();
                    propsform.WidthText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentRect.Width.ToString();
                    propsform.HeightText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentRect.Height.ToString();
                    propsform.xOffsetText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentFrame.ToString();
                    propsform.yOffsetText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentRect.Height.ToString();
                    propsform.FramesPerSecondText = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.FramesPerSecond.ToString();

                    propsform.ShowDialog();
                }
            }
        }

        //private void animationRemove_Click(object sender, EventArgs e)
        //{
        //    if (animationListBox.SelectedItem != null)
        //    {
        //        string textureName = animationListBox.SelectedItem.ToString();

        //        foreach (TileLayer layer in tileMap.Layers)
        //            if (layer.HasIndex(tileMap.ReturnTextureIndex(textureDict[textureName])) != -1)
        //                layer.ReplaceIndex(layer.HasIndex(tileMap.ReturnTextureIndex(textureDict[textureName])), -1);

        //        tileMap.RemoveTexture(textureDict[textureName], tileMap);
        //        animatedDict.Remove(textureName);
        //        previewDict.Remove(textureName);
        //        animationListBox.Items.Remove(animationListBox.SelectedItem);

        //        texturePreviewBox.Image = null;

        //    }
        //}

        private void animationTrackBar_Scroll(object sender, EventArgs e)
        {
            if (animationListBox.SelectedItem != null)
            {
                tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentFrame = animationTrackBar.Value;
            }
        }

        private void animationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (animationListBox.SelectedItem != null)
            {
                animationTrackBar.Maximum = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.NumberOfFrames;
                animationTrackBar.Value = tileMap.animatedTextures[animationListBox.Items.IndexOf(animationListBox.SelectedItem)].CurrentAnimation.CurrentFrame;
            }
            texturePreviewBox.Image = previewDict[animationListBox.SelectedItem.ToString()];
            textureListBox.SelectedItem = null;
            doodadListBox.SelectedItem = null;
        }

        private void animationListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void animatePreviewBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(animatePreviewBoxToolStripMenuItem.Checked == true)
            //    previewBoxIsAnimating = true;
            //else if (animatePreviewBoxToolStripMenuItem.Checked == false)
            //    previewBoxIsAnimating = false;
        }

        private void doodadProperties_Click(object sender, EventArgs e)
        {
            if (doodadListBox.SelectedItem != null)
            {
                Props propsform = new Props();

                propsform.NameText = doodadNames[doodadListBox.SelectedIndex];
                propsform.WidthText = doodadDict[doodadListBox.SelectedIndex].Width.ToString();
                propsform.HeightText = doodadDict[doodadListBox.SelectedIndex].Height.ToString();

                propsform.ShowDialog();
            }
        }

        private void textureProperties_Click(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                Props propsform = new Props();

                propsform.NameText = textureNames[textureListBox.SelectedIndex];
                propsform.WidthText = textureDict[textureListBox.SelectedIndex].Width.ToString();
                propsform.HeightText = textureDict[textureListBox.SelectedIndex].Height.ToString();

                propsform.ShowDialog();
            }
        }
    }
}