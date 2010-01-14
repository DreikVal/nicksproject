namespace FriendlyEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTileMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collisionTransparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coll0Transparency = new System.Windows.Forms.ToolStripMenuItem();
            this.col25Transparency = new System.Windows.Forms.ToolStripMenuItem();
            this.coll50Transparency = new System.Windows.Forms.ToolStripMenuItem();
            this.coll75Transparency = new System.Windows.Forms.ToolStripMenuItem();
            this.coll100Transparency = new System.Windows.Forms.ToolStripMenuItem();
            this.showPreviewBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animatePreviewBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contentPathTextBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.layerListBox = new System.Windows.Forms.ListBox();
            this.addLayerButton = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.textureListBox = new System.Windows.Forms.ListBox();
            this.addTextureButton = new System.Windows.Forms.Button();
            this.removeTextureButton = new System.Windows.Forms.Button();
            this.texturePreviewBox = new System.Windows.Forms.PictureBox();
            this.alphaSlider = new System.Windows.Forms.TrackBar();
            this.collisionListBox = new System.Windows.Forms.ListBox();
            this.layerPropButton = new System.Windows.Forms.Button();
            this.layerUpButton = new System.Windows.Forms.Button();
            this.layerDownButton = new System.Windows.Forms.Button();
            this.addCollLayer = new System.Windows.Forms.Button();
            this.removeCollLayer = new System.Windows.Forms.Button();
            this.collisionProperties = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fillStripButton = new System.Windows.Forms.ToolStripButton();
            this.collisionStripButton = new System.Windows.Forms.ToolStripButton();
            this.doodadListBox = new System.Windows.Forms.ListBox();
            this.addDoodadButton = new System.Windows.Forms.Button();
            this.removeDoodadButton = new System.Windows.Forms.Button();
            this.animationTrackBar = new System.Windows.Forms.TrackBar();
            this.animationListBox = new System.Windows.Forms.ListBox();
            this.animationAdd = new System.Windows.Forms.Button();
            this.animationRemove = new System.Windows.Forms.Button();
            this.animationProps = new System.Windows.Forms.Button();
            this.doodadProperties = new System.Windows.Forms.Button();
            this.textureProperties = new System.Windows.Forms.Button();
            this.tileDisplay1 = new FriendlyEditor.TileDisplay();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaSlider)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animationTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(19, 729);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(868, 16);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.vScrollBar1.Location = new System.Drawing.Point(0, 27);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 699);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1104, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTileMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newTileMapToolStripMenuItem
            // 
            this.newTileMapToolStripMenuItem.Name = "newTileMapToolStripMenuItem";
            this.newTileMapToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.newTileMapToolStripMenuItem.Text = "New Tile Map";
            this.newTileMapToolStripMenuItem.Click += new System.EventHandler(this.newTileMapToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllLayersToolStripMenuItem,
            this.showGridToolStripMenuItem,
            this.collisionTransparencyToolStripMenuItem,
            this.showPreviewBoxToolStripMenuItem,
            this.animatePreviewBoxToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // showAllLayersToolStripMenuItem
            // 
            this.showAllLayersToolStripMenuItem.CheckOnClick = true;
            this.showAllLayersToolStripMenuItem.Name = "showAllLayersToolStripMenuItem";
            this.showAllLayersToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showAllLayersToolStripMenuItem.Text = "Show All Layers";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Checked = true;
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            // 
            // collisionTransparencyToolStripMenuItem
            // 
            this.collisionTransparencyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coll0Transparency,
            this.col25Transparency,
            this.coll50Transparency,
            this.coll75Transparency,
            this.coll100Transparency});
            this.collisionTransparencyToolStripMenuItem.Name = "collisionTransparencyToolStripMenuItem";
            this.collisionTransparencyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.collisionTransparencyToolStripMenuItem.Text = "Collision Transparency";
            // 
            // coll0Transparency
            // 
            this.coll0Transparency.Name = "coll0Transparency";
            this.coll0Transparency.Size = new System.Drawing.Size(103, 22);
            this.coll0Transparency.Text = "0%";
            this.coll0Transparency.Click += new System.EventHandler(this.coll0Transparency_Click);
            // 
            // col25Transparency
            // 
            this.col25Transparency.Name = "col25Transparency";
            this.col25Transparency.Size = new System.Drawing.Size(103, 22);
            this.col25Transparency.Text = "25%";
            this.col25Transparency.Click += new System.EventHandler(this.col25Transparency_Click);
            // 
            // coll50Transparency
            // 
            this.coll50Transparency.Name = "coll50Transparency";
            this.coll50Transparency.Size = new System.Drawing.Size(103, 22);
            this.coll50Transparency.Text = "50%";
            this.coll50Transparency.Click += new System.EventHandler(this.coll50Transparency_Click);
            // 
            // coll75Transparency
            // 
            this.coll75Transparency.Name = "coll75Transparency";
            this.coll75Transparency.Size = new System.Drawing.Size(103, 22);
            this.coll75Transparency.Text = "75%";
            this.coll75Transparency.Click += new System.EventHandler(this.coll75Transparency_Click);
            // 
            // coll100Transparency
            // 
            this.coll100Transparency.Name = "coll100Transparency";
            this.coll100Transparency.Size = new System.Drawing.Size(103, 22);
            this.coll100Transparency.Text = "100%";
            this.coll100Transparency.Click += new System.EventHandler(this.coll100Transparency_Click);
            // 
            // showPreviewBoxToolStripMenuItem
            // 
            this.showPreviewBoxToolStripMenuItem.Checked = true;
            this.showPreviewBoxToolStripMenuItem.CheckOnClick = true;
            this.showPreviewBoxToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPreviewBoxToolStripMenuItem.Name = "showPreviewBoxToolStripMenuItem";
            this.showPreviewBoxToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showPreviewBoxToolStripMenuItem.Text = "Show Preview Box";
            this.showPreviewBoxToolStripMenuItem.Click += new System.EventHandler(this.showPreviewBoxToolStripMenuItem_Click);
            // 
            // animatePreviewBoxToolStripMenuItem
            // 
            this.animatePreviewBoxToolStripMenuItem.CheckOnClick = true;
            this.animatePreviewBoxToolStripMenuItem.Name = "animatePreviewBoxToolStripMenuItem";
            this.animatePreviewBoxToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.animatePreviewBoxToolStripMenuItem.Text = "Animate Preview Box";
            this.animatePreviewBoxToolStripMenuItem.Click += new System.EventHandler(this.animatePreviewBoxToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contentPathTextBox
            // 
            this.contentPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.contentPathTextBox.Location = new System.Drawing.Point(893, 32);
            this.contentPathTextBox.Name = "contentPathTextBox";
            this.contentPathTextBox.ReadOnly = true;
            this.contentPathTextBox.Size = new System.Drawing.Size(202, 20);
            this.contentPathTextBox.TabIndex = 4;
            // 
            // layerListBox
            // 
            this.layerListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layerListBox.FormattingEnabled = true;
            this.layerListBox.Location = new System.Drawing.Point(893, 58);
            this.layerListBox.Name = "layerListBox";
            this.layerListBox.Size = new System.Drawing.Size(158, 69);
            this.layerListBox.TabIndex = 7;
            this.layerListBox.SelectedIndexChanged += new System.EventHandler(this.layerListBox_SelectedIndexChanged);
            this.layerListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.layerListBox_KeyDown);
            // 
            // addLayerButton
            // 
            this.addLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addLayerButton.Location = new System.Drawing.Point(1082, 56);
            this.addLayerButton.Name = "addLayerButton";
            this.addLayerButton.Size = new System.Drawing.Size(21, 23);
            this.addLayerButton.TabIndex = 8;
            this.addLayerButton.Text = "+";
            this.addLayerButton.UseVisualStyleBackColor = true;
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeLayerButton.Location = new System.Drawing.Point(1082, 108);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(21, 23);
            this.removeLayerButton.TabIndex = 8;
            this.removeLayerButton.Text = "-";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            // 
            // textureListBox
            // 
            this.textureListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textureListBox.FormattingEnabled = true;
            this.textureListBox.Location = new System.Drawing.Point(893, 325);
            this.textureListBox.Name = "textureListBox";
            this.textureListBox.Size = new System.Drawing.Size(185, 95);
            this.textureListBox.TabIndex = 7;
            this.textureListBox.SelectedIndexChanged += new System.EventHandler(this.textureListBox_SelectedIndexChanged);
            this.textureListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textureListBox_KeyDown);
            // 
            // addTextureButton
            // 
            this.addTextureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addTextureButton.Location = new System.Drawing.Point(1079, 327);
            this.addTextureButton.Name = "addTextureButton";
            this.addTextureButton.Size = new System.Drawing.Size(24, 23);
            this.addTextureButton.TabIndex = 8;
            this.addTextureButton.Text = "+";
            this.addTextureButton.UseVisualStyleBackColor = true;
            this.addTextureButton.Click += new System.EventHandler(this.addTextureButton_Click);
            // 
            // removeTextureButton
            // 
            this.removeTextureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeTextureButton.Location = new System.Drawing.Point(1079, 398);
            this.removeTextureButton.Name = "removeTextureButton";
            this.removeTextureButton.Size = new System.Drawing.Size(24, 23);
            this.removeTextureButton.TabIndex = 8;
            this.removeTextureButton.Text = "-";
            this.removeTextureButton.UseVisualStyleBackColor = true;
            this.removeTextureButton.Click += new System.EventHandler(this.removeTextureButton_Click);
            // 
            // texturePreviewBox
            // 
            this.texturePreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.texturePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.texturePreviewBox.Location = new System.Drawing.Point(893, 539);
            this.texturePreviewBox.Name = "texturePreviewBox";
            this.texturePreviewBox.Size = new System.Drawing.Size(185, 185);
            this.texturePreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.texturePreviewBox.TabIndex = 9;
            this.texturePreviewBox.TabStop = false;
            // 
            // alphaSlider
            // 
            this.alphaSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alphaSlider.Location = new System.Drawing.Point(901, 134);
            this.alphaSlider.Maximum = 100;
            this.alphaSlider.Name = "alphaSlider";
            this.alphaSlider.Size = new System.Drawing.Size(193, 42);
            this.alphaSlider.TabIndex = 11;
            this.alphaSlider.TickFrequency = 5;
            this.alphaSlider.Value = 100;
            this.alphaSlider.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // collisionListBox
            // 
            this.collisionListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.collisionListBox.FormattingEnabled = true;
            this.collisionListBox.Location = new System.Drawing.Point(893, 177);
            this.collisionListBox.Name = "collisionListBox";
            this.collisionListBox.Size = new System.Drawing.Size(185, 69);
            this.collisionListBox.TabIndex = 7;
            this.collisionListBox.SelectedIndexChanged += new System.EventHandler(this.collisionListBox_SelectedIndexChanged);
            this.collisionListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.collisionListBox_KeyDown);
            // 
            // layerPropButton
            // 
            this.layerPropButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layerPropButton.Location = new System.Drawing.Point(1082, 80);
            this.layerPropButton.Name = "layerPropButton";
            this.layerPropButton.Size = new System.Drawing.Size(21, 27);
            this.layerPropButton.TabIndex = 13;
            this.layerPropButton.Text = "P";
            this.layerPropButton.UseVisualStyleBackColor = true;
            this.layerPropButton.Click += new System.EventHandler(this.layerPropButton_Click);
            // 
            // layerUpButton
            // 
            this.layerUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layerUpButton.Location = new System.Drawing.Point(1059, 56);
            this.layerUpButton.Name = "layerUpButton";
            this.layerUpButton.Size = new System.Drawing.Size(19, 34);
            this.layerUpButton.TabIndex = 14;
            this.layerUpButton.Text = "˄";
            this.layerUpButton.UseVisualStyleBackColor = true;
            this.layerUpButton.Click += new System.EventHandler(this.layerUpButton_Click);
            // 
            // layerDownButton
            // 
            this.layerDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layerDownButton.Location = new System.Drawing.Point(1059, 91);
            this.layerDownButton.Name = "layerDownButton";
            this.layerDownButton.Size = new System.Drawing.Size(19, 36);
            this.layerDownButton.TabIndex = 14;
            this.layerDownButton.Text = "˅";
            this.layerDownButton.UseVisualStyleBackColor = true;
            this.layerDownButton.Click += new System.EventHandler(this.layerDownButton_Click);
            // 
            // addCollLayer
            // 
            this.addCollLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addCollLayer.Location = new System.Drawing.Point(1080, 176);
            this.addCollLayer.Name = "addCollLayer";
            this.addCollLayer.Size = new System.Drawing.Size(23, 20);
            this.addCollLayer.TabIndex = 15;
            this.addCollLayer.Text = "+";
            this.addCollLayer.UseVisualStyleBackColor = true;
            this.addCollLayer.Click += new System.EventHandler(this.addCollLayer_Click);
            // 
            // removeCollLayer
            // 
            this.removeCollLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeCollLayer.Location = new System.Drawing.Point(1079, 224);
            this.removeCollLayer.Name = "removeCollLayer";
            this.removeCollLayer.Size = new System.Drawing.Size(24, 22);
            this.removeCollLayer.TabIndex = 16;
            this.removeCollLayer.Text = "-";
            this.removeCollLayer.UseVisualStyleBackColor = true;
            this.removeCollLayer.Click += new System.EventHandler(this.removeCollLayer_Click);
            // 
            // collisionProperties
            // 
            this.collisionProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.collisionProperties.Location = new System.Drawing.Point(1079, 198);
            this.collisionProperties.Name = "collisionProperties";
            this.collisionProperties.Size = new System.Drawing.Size(24, 23);
            this.collisionProperties.TabIndex = 17;
            this.collisionProperties.Text = "P";
            this.collisionProperties.UseVisualStyleBackColor = true;
            this.collisionProperties.Click += new System.EventHandler(this.collisionProperties_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillStripButton,
            this.collisionStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(861, 27);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(26, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fillStripButton
            // 
            this.fillStripButton.CheckOnClick = true;
            this.fillStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fillStripButton.Image = ((System.Drawing.Image)(resources.GetObject("fillStripButton.Image")));
            this.fillStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fillStripButton.Name = "fillStripButton";
            this.fillStripButton.Size = new System.Drawing.Size(23, 22);
            this.fillStripButton.Text = "Fill";
            this.fillStripButton.Click += new System.EventHandler(this.fillStripButton_Click);
            // 
            // collisionStripButton
            // 
            this.collisionStripButton.CheckOnClick = true;
            this.collisionStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collisionStripButton.Image = ((System.Drawing.Image)(resources.GetObject("collisionStripButton.Image")));
            this.collisionStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collisionStripButton.Name = "collisionStripButton";
            this.collisionStripButton.Size = new System.Drawing.Size(23, 22);
            this.collisionStripButton.Text = "Collision";
            this.collisionStripButton.Visible = false;
            this.collisionStripButton.Click += new System.EventHandler(this.collisionStripButton_Click);
            // 
            // doodadListBox
            // 
            this.doodadListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doodadListBox.FormattingEnabled = true;
            this.doodadListBox.Location = new System.Drawing.Point(893, 250);
            this.doodadListBox.Name = "doodadListBox";
            this.doodadListBox.Size = new System.Drawing.Size(185, 69);
            this.doodadListBox.TabIndex = 19;
            this.doodadListBox.SelectedIndexChanged += new System.EventHandler(this.doodadListBox_SelectedIndexChanged);
            this.doodadListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.doodadListBox_KeyDown);
            // 
            // addDoodadButton
            // 
            this.addDoodadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addDoodadButton.Location = new System.Drawing.Point(1079, 251);
            this.addDoodadButton.Name = "addDoodadButton";
            this.addDoodadButton.Size = new System.Drawing.Size(24, 23);
            this.addDoodadButton.TabIndex = 20;
            this.addDoodadButton.Text = "+";
            this.addDoodadButton.UseVisualStyleBackColor = true;
            this.addDoodadButton.Click += new System.EventHandler(this.addDoodadButton_Click);
            // 
            // removeDoodadButton
            // 
            this.removeDoodadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeDoodadButton.Location = new System.Drawing.Point(1079, 302);
            this.removeDoodadButton.Name = "removeDoodadButton";
            this.removeDoodadButton.Size = new System.Drawing.Size(24, 23);
            this.removeDoodadButton.TabIndex = 20;
            this.removeDoodadButton.Text = "-";
            this.removeDoodadButton.UseVisualStyleBackColor = true;
            this.removeDoodadButton.Click += new System.EventHandler(this.removeDoodadButton_Click);
            // 
            // animationTrackBar
            // 
            this.animationTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.animationTrackBar.Location = new System.Drawing.Point(901, 426);
            this.animationTrackBar.Name = "animationTrackBar";
            this.animationTrackBar.Size = new System.Drawing.Size(193, 42);
            this.animationTrackBar.TabIndex = 21;
            this.animationTrackBar.Scroll += new System.EventHandler(this.animationTrackBar_Scroll);
            // 
            // animationListBox
            // 
            this.animationListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.animationListBox.FormattingEnabled = true;
            this.animationListBox.Location = new System.Drawing.Point(893, 465);
            this.animationListBox.Name = "animationListBox";
            this.animationListBox.Size = new System.Drawing.Size(185, 69);
            this.animationListBox.TabIndex = 22;
            this.animationListBox.SelectedIndexChanged += new System.EventHandler(this.animationListBox_SelectedIndexChanged);
            this.animationListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.animationListBox_KeyDown);
            // 
            // animationAdd
            // 
            this.animationAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.animationAdd.Location = new System.Drawing.Point(1079, 463);
            this.animationAdd.Name = "animationAdd";
            this.animationAdd.Size = new System.Drawing.Size(24, 23);
            this.animationAdd.TabIndex = 23;
            this.animationAdd.Text = "+";
            this.animationAdd.UseVisualStyleBackColor = true;
            // 
            // animationRemove
            // 
            this.animationRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.animationRemove.Location = new System.Drawing.Point(1079, 512);
            this.animationRemove.Name = "animationRemove";
            this.animationRemove.Size = new System.Drawing.Size(24, 23);
            this.animationRemove.TabIndex = 23;
            this.animationRemove.Text = "-";
            this.animationRemove.UseVisualStyleBackColor = true;
            // 
            // animationProps
            // 
            this.animationProps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.animationProps.Location = new System.Drawing.Point(1079, 487);
            this.animationProps.Name = "animationProps";
            this.animationProps.Size = new System.Drawing.Size(24, 23);
            this.animationProps.TabIndex = 23;
            this.animationProps.Text = "P";
            this.animationProps.UseVisualStyleBackColor = true;
            this.animationProps.Click += new System.EventHandler(this.animationProps_Click);
            // 
            // doodadProperties
            // 
            this.doodadProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doodadProperties.Location = new System.Drawing.Point(1079, 276);
            this.doodadProperties.Name = "doodadProperties";
            this.doodadProperties.Size = new System.Drawing.Size(24, 23);
            this.doodadProperties.TabIndex = 24;
            this.doodadProperties.Text = "P";
            this.doodadProperties.UseVisualStyleBackColor = true;
            this.doodadProperties.Click += new System.EventHandler(this.doodadProperties_Click);
            // 
            // textureProperties
            // 
            this.textureProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textureProperties.Location = new System.Drawing.Point(1079, 363);
            this.textureProperties.Name = "textureProperties";
            this.textureProperties.Size = new System.Drawing.Size(24, 23);
            this.textureProperties.TabIndex = 25;
            this.textureProperties.Text = "P";
            this.textureProperties.UseVisualStyleBackColor = true;
            this.textureProperties.Click += new System.EventHandler(this.textureProperties_Click);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tileDisplay1.Location = new System.Drawing.Point(19, 27);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(868, 699);
            this.tileDisplay1.TabIndex = 0;
            this.tileDisplay1.Text = "tileDisplay1";
            this.tileDisplay1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseDown);
            this.tileDisplay1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 745);
            this.Controls.Add(this.textureProperties);
            this.Controls.Add(this.doodadProperties);
            this.Controls.Add(this.animationProps);
            this.Controls.Add(this.animationRemove);
            this.Controls.Add(this.animationAdd);
            this.Controls.Add(this.animationListBox);
            this.Controls.Add(this.animationTrackBar);
            this.Controls.Add(this.removeDoodadButton);
            this.Controls.Add(this.addDoodadButton);
            this.Controls.Add(this.doodadListBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.collisionProperties);
            this.Controls.Add(this.removeCollLayer);
            this.Controls.Add(this.addCollLayer);
            this.Controls.Add(this.layerDownButton);
            this.Controls.Add(this.layerUpButton);
            this.Controls.Add(this.layerPropButton);
            this.Controls.Add(this.alphaSlider);
            this.Controls.Add(this.texturePreviewBox);
            this.Controls.Add(this.removeTextureButton);
            this.Controls.Add(this.removeLayerButton);
            this.Controls.Add(this.addTextureButton);
            this.Controls.Add(this.addLayerButton);
            this.Controls.Add(this.textureListBox);
            this.Controls.Add(this.collisionListBox);
            this.Controls.Add(this.layerListBox);
            this.Controls.Add(this.contentPathTextBox);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Friendly Editor 9000";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaSlider)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animationTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TileDisplay tileDisplay1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTileMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox contentPathTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListBox layerListBox;
        private System.Windows.Forms.Button addLayerButton;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.ListBox textureListBox;
        private System.Windows.Forms.Button addTextureButton;
        private System.Windows.Forms.Button removeTextureButton;
        private System.Windows.Forms.PictureBox texturePreviewBox;
        private System.Windows.Forms.TrackBar alphaSlider;
        private System.Windows.Forms.ListBox collisionListBox;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllLayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.Button layerPropButton;
        private System.Windows.Forms.Button layerUpButton;
        private System.Windows.Forms.Button layerDownButton;
        private System.Windows.Forms.Button addCollLayer;
        private System.Windows.Forms.Button removeCollLayer;
        private System.Windows.Forms.Button collisionProperties;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton fillStripButton;
        private System.Windows.Forms.ToolStripButton collisionStripButton;
        private System.Windows.Forms.ToolStripMenuItem collisionTransparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coll0Transparency;
        private System.Windows.Forms.ToolStripMenuItem col25Transparency;
        private System.Windows.Forms.ToolStripMenuItem coll50Transparency;
        private System.Windows.Forms.ToolStripMenuItem coll75Transparency;
        private System.Windows.Forms.ToolStripMenuItem coll100Transparency;
        private System.Windows.Forms.ListBox doodadListBox;
        private System.Windows.Forms.Button addDoodadButton;
        private System.Windows.Forms.Button removeDoodadButton;
        private System.Windows.Forms.ToolStripMenuItem showPreviewBoxToolStripMenuItem;
        private System.Windows.Forms.TrackBar animationTrackBar;
        private System.Windows.Forms.ListBox animationListBox;
        private System.Windows.Forms.Button animationAdd;
        private System.Windows.Forms.Button animationRemove;
        private System.Windows.Forms.Button animationProps;
        private System.Windows.Forms.ToolStripMenuItem animatePreviewBoxToolStripMenuItem;
        private System.Windows.Forms.Button doodadProperties;
        private System.Windows.Forms.Button textureProperties;

    }
}

