using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        int[] redArr = new int[256];
        int[] greenArr = new int[256];
        int[] blueArr = new int[256];
        private Bitmap[] RGB_FUNC(Bitmap source)
        {
            Bitmap[] result = new Bitmap[] { new Bitmap(source.Width, source.Height), new Bitmap(source.Width, source.Height), new Bitmap(source.Width, source.Height) };
            
            for (int i = 0; i < source.Width; i++)
            {
                for (int j = 0; j < source.Height; j++)
                {
                    Color color = source.GetPixel(i, j);
                    result[0].SetPixel(i, j, Color.FromArgb(color.A, color.R, 0, 0));
                    result[1].SetPixel(i, j, Color.FromArgb(color.A, 0, color.G, 0));
                    result[2].SetPixel(i, j, Color.FromArgb(color.A, 0, 0, color.B));
                    redArr[color.R]++;
                    greenArr[color.G]++;
                    blueArr[color.B]++;

                }
            }
            return result;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpeg;*.jpg;*.png";
                if (ofd.ShowDialog() == DialogResult.Cancel)
                    return;
                pictureBox1.Image = new Bitmap(ofd.FileName);

                Bitmap[] rgb = RGB_FUNC((Bitmap)pictureBox1.Image);
                redPictureBox.Image = rgb[0];
                greenPictureBox.Image = rgb[1];
                bluePictureBox.Image = rgb[2];                                                     
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 255; i++)
            {
                chart1.Series[0].Color = Color.Red;
                chart1.Series[0].Points.AddXY(i, redArr[i]);
                chart2.Series[0].Color = Color.Green;
                chart2.Series[0].Points.AddXY(i, greenArr[i]);
                chart3.Series[0].Color = Color.Blue;
                chart3.Series[0].Points.AddXY(i, blueArr[i]);
            }
        }
    }
}
