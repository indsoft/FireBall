using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeImage.Examples
{
    public partial class Threshold : Form
    {
        public Threshold()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            byte bt;

            if (!byte.TryParse(textBox1.Text, out bt))
            {
                errorProvider1.SetError(textBox1, "Invalid Value must be from 0 to 255");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(errorProvider1.GetError(this.textBox1)))
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public byte Range
        {
            get
            {
                return Convert.ToByte(this.textBox1.Text);
            }
        }
    }
}
