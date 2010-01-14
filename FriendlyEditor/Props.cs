using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FriendlyEditor
{
    public partial class Props : Form
    {
        public bool OKPressed = false;

        public Props()
        {
            InitializeComponent();
        }

        public string NameText
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
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

        private void okButton_Click(object sender, EventArgs e)
        {
            OKPressed = true;
            Close();
        }
    }
}