using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FI = Fireball.Drawing.FreeImage;
using Fireball.Drawing;
using System.Reflection;


namespace FreeImage.Examples
{
    public partial class FormMain : Form
    {
        private FI freeImage = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void mnuOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = GetAllDialogFilters();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.freeImage = new Fireball.Drawing.FreeImage(ofd.FileName);
                this.SetPicture();
            }
        }

        private string GetAllDialogFilters()
        {
            var all = typeof(FreeImageFormatInfo).GetProperties(BindingFlags.Public | BindingFlags.Static);

            StringBuilder sb = new StringBuilder();

            for( int i =0; i < all.Length;i++)
            {
                var c = all[i];
                var prop = (FreeImageFormatInfo)c.GetValue(null, null);

                if (prop.Extensions.Length == 0) continue;

                sb.Append(prop.ToString() + "|");

                for (int y = 0; y < prop.Extensions.Length; y++)
                {
                    var cx = prop.Extensions[y];

                    sb.Append("*.");
                    sb.Append(cx);
                    if (y < prop.Extensions.Length - 1)
                        sb.Append(";");
                }

                if (i < all.Length - 1)
                    sb.Append("|");
            }

            return sb.ToString();
        }

        public void SetPicture()
        {
            if (freeImage != null)
            {
                pictureBox1.Location = Point.Empty;
                pictureBox1.Image = freeImage.GetBitmap();
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void mnuFlipVertically_Click(object sender, EventArgs e)
        { 
            this.freeImage.FlipVertical();
            this.SetPicture();
        }

        private void mnuFlipHorizontally_Click(object sender, EventArgs e)
        {
            this.freeImage.FlipHorizontal();
            this.SetPicture();
        }

        private void mnuInvert_Click(object sender, EventArgs e)
        {
            this.freeImage.Invert();
            this.SetPicture();
        }

        private void mnuSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = GetAllDialogFilters();

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.freeImage.Save(sfd.FileName);
            }
        }

        private void mnuThreshold_Click(object sender, EventArgs e)
        {
            Threshold frm = new Threshold();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.freeImage = this.freeImage.Threshold(frm.Range);
                this.SetPicture();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.freeImage = this.freeImage.Rotate(45);
            this.SetPicture();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.freeImage = this.freeImage.Rotate(-45);
            this.SetPicture();
        }
    }
}
