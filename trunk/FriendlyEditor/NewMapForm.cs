using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FriendlyEditor
{
    public partial class NewMapForm : Form
    {
        public bool OKPressed = false;

        public NewMapForm()
        {
            InitializeComponent();
        }

        private void OKButton_Click_1(object sender, EventArgs e)
        {
            OKPressed = true;
            Close();
        }

        private void cancelButton_Click_1(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }
    }
}