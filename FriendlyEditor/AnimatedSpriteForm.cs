using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FriendlyEditor
{
    public partial class AnimatedSpriteForm : Form
    {
        public bool OKPressed = false;

        public AnimatedSpriteForm()
        {
            InitializeComponent();
        }

        public string NameText
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public string FrameText
        {
            get { return numberFramesTextBox.Text; }
            set { numberFramesTextBox.Text = value; }
        }

        public string WidthText
        {
            get { return widthTextBox.Text; }
            set { widthTextBox.Text = value; }
        }

        public string HeightText
        {
            get { return heightTextBox.Text; }
            set { heightTextBox.Text = value; }
        }

        public string xOffsetText
        {
            get { return xOffsetTextBox.Text; }
            set { xOffsetTextBox.Text = value; }
        }

        public string yOffsetText
        {
            get { return yOffsetTextBox.Text; }
            set { yOffsetTextBox.Text = value; }
        }

        public string FramesPerSecondText
        {
            get { return FPSTextBox.Text; }
            set { FPSTextBox.Text = value; }
        }

        private void OKButton_Click_1(object sender, EventArgs e)
        {
            OKPressed = true;
            Close();
        }
    }
}