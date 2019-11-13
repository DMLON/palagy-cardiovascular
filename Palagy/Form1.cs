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

namespace Palagy
{
	public partial class ImageBox2 : Form
	{
		Grid3D map1;
		Grid3D map2;
		int z;
		int N;
		int M;
		public ImageBox2(Grid3D map1,Grid3D map2,int n=5,int m=5)
		{
			this.map1 = map1;
			this.map2 = map2;
			z = 0;
			N = n;
			M = m;
			MatrixToImage conv = new MatrixToImage(min: 0, max: 1);
			//int N = 3;
			double[,] temp = new double[N, N];
			for (int i = 0; i < N; i++)
				for (int j = 0; j < N; j++)
				{
					temp[i, j] = map2.grid[i, j, z].Black ? 1 : 0;
				}
			this.MouseWheel += onMousewheel;
			this.Paint += ImageBox2_Paint;
			Bitmap image; conv.Convert(temp, out image);
			image = new ResizeNearestNeighbor(500, 500).Apply(image);
			this.InitializeComponent();
			pictureBox1.Image = image;
			pictureBox1.Size = new Size(500, 500);
			
		}

		private void ImageBox2_Paint(object sender, PaintEventArgs e)
		{
			//int N = 5;
			double[,] temp = new double[N, N];
			for (int i = 0; i < N; i++)
				for (int j = 0; j < N; j++)
				{
					temp[i, j] = map2.grid[i, j, z].Black ? 1 : 0;
				}
			MatrixToImage conv = new MatrixToImage(min: 0, max: 1);
			Bitmap image; conv.Convert(temp, out image);
			image = new ResizeNearestNeighbor(500, 500).Apply(image);
			pictureBox1.Image = image;
		}

		public void onMousewheel(object sender, MouseEventArgs e)
		{
			//int M = 5;
			z += e.Delta/120;
			z = z > M-1 ? M-1 : z < 0 ? 0 : z;
			this.Invalidate();
			zPos_label.Text = $"Z = {z}";
		}
	}
}
