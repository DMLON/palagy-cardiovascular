using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging.Converters;
using Accord.Imaging;
using Accord.Controls;
using Accord.Imaging.Filters;
using Solver;

namespace Centerline
{
    public partial class Form1 : Form
    {
        double[,] map1;
        int N;
        public Form1(double[,] map)
        {
            this.map1 = map;
            MatrixToImage conv = new MatrixToImage(min: 0, max: 1);
            //int N = 3;
            Bitmap image; conv.Convert(map1, out image);
            image = new ResizeNearestNeighbor(512, 512).Apply(image);
            this.InitializeComponent();
            pictureBox1.Image = image;
            pictureBox1.Size = new Size(512, 512);

        }
    }
}
