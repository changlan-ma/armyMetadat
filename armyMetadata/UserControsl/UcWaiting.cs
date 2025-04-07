using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Configuration;

namespace armyMetadata.UserControsl
{
    public partial class UcWaiting : UserControl
    {
        public UcWaiting()
        {
            InitializeComponent();
        }
        string colorName = string.Empty;

        public void StartWaiting()
        {
            this.writTime.Start();
        }
        public void StopWaiting()
        {
            writTime.Stop();
        }

        private int count = -1;
        private ArrayList images = new ArrayList();
        public Bitmap[] bitmap = new Bitmap[8];
        private void writTime_Tick(object sender, EventArgs e)
        {
            count = (count + 1) % 8;
            pictureBox.Image = (Image)images[count];
        }
        public void set()
        {
            Draw();
            for (int i = 0; i < 8; i++)
            {
                Bitmap map = new Bitmap((bitmap[i]), new Size(pictureBox.Size.Width, pictureBox.Size.Height));
                images.Add(map);
            }
            pictureBox.Image = (Image)images[7];
        }
        public void Draw()
        {
            for (int j = 0; j < 8; j++)
            {
                bitmap[7 - j] = DrawCircle(j);
            }
        }
        int[] a = new int[8] { 25, 50, 75, 100, 125, 150, 175, 200 };

        private Color _circleColor = Color.DeepSkyBlue;
        public Bitmap DrawCircle(int j)
        {
            Bitmap map = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            Graphics g = Graphics.FromImage(map);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = 1; i <= 8; i++)
            {
                int alpha = a[(i + j - 1) % 8];
                Color drawColor = Color.FromArgb(alpha, _circleColor);
                using (SolidBrush brush = new SolidBrush(drawColor))
                {
                    float w = pictureBox.Size.Width / 8F;
                    float wbiff = w / 10f;
                    float h = pictureBox.Size.Height;
                    float hbiff = h / 10f;

                    float x = w * (i - 1) + wbiff;
                    float y = 0 + hbiff;
                    w = w - wbiff * 2F;
                    h = h - hbiff * 2F;
                    g.FillEllipse(brush, x, y, w, h);

                }
            }
            return map;
        }

        private void Writing_Load(object sender, EventArgs e)
        {
            colorName = ConfigurationManager.AppSettings["ColorName"];
            if (colorName == null || string.IsNullOrWhiteSpace(colorName))
            {
                _circleColor = Color.DeepSkyBlue;
            }
            else
            {
                try
                {
                    _circleColor = Color.FromName(colorName);
                }
                catch
                {
                    _circleColor = Color.DeepSkyBlue;
                }
            }
            set();
        }
    }
}
