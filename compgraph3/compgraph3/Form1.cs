using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compgraph3
{
    public partial class Form1 : Form
    {
        public static void toHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color HSVtoRGB(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap bmp;
        double sh = 9, ss = 5, sv = 5;

         double gt0(double v, double sv) {
            v += sv * 0.1 - 0.5;
            if (v > 1) return 1;
            else if (v < 0) return 0;
            else return v;
        }
        double gt0_360(double v, double sv)
        {
            v += sv * 20;
            return v % 360;
        }

        void redrawPic() {

            Color rgb;
            double h, s, v;
            if (bmp != null)
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    rgb = bmp.GetPixel(x, y);
                    toHSV(rgb, out h, out s, out v);
                        h = gt0_360(h, sh);
                        s = gt0(s, ss);
                        v = gt0(v, sv);
                    bmp.SetPixel(x, y, HSVtoRGB(h, s, v));
                }
            pictureBox2.Image = bmp;
            bmp = new Bitmap(pictureBox1.Image);
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var op = openFileDialog1.ShowDialog();
            if (op.Equals(DialogResult.OK)) { 
                pictureBox1.Load(openFileDialog1.FileName);
                bmp = new Bitmap(pictureBox1.Image);
                trackBar1.Value = 10;
                trackBar2.Value = 10;
                trackBar3.Value = 10;
                //Graphics g = Graphics.FromImage(bmp);
                redrawPic();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            sv = trackBar1.Value;
            redrawPic();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var save = saveFileDialog1.ShowDialog();
            if (save.Equals(DialogResult.OK) && pictureBox2.Image != null)
                pictureBox2.Image.Save(saveFileDialog1.FileName + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ss = trackBar2.Value;
            redrawPic();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            sh = trackBar3.Value;
            redrawPic();
        }
    }
}
