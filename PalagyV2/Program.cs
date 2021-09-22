using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging.Converters;
using Accord.Imaging;
using Accord.Controls;
using Accord.Imaging.Filters;
using System.Diagnostics;
using Solver;

namespace Palagy
{




	class Program
	{
		static void CubeAtPos(ref int[,,] map, Vector3D point, int size)
		{
			var x = point.X; var y = point.Y; var z = point.Z;
			for (int i = -size / 2; i <= size / 2; i++)
			{
				for (int j = -size / 2; j <= size / 2; j++)
				{
					for (int k = -size / 2; k <= size / 2; k++)
					{
						map[(int)x + i, (int)y + j, (int)z + k] = 1;
					}
				}
			}

		}

		static void Main(string[] args)
		{

			int[,,] map = new int[,,]
			{
				{{ 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0 } },
				{{ 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 },{ 0, 0, 0, 0, 0 } },
				{{ 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 1, 0, 0 }, { 0, 1, 1, 1, 0 },{ 0, 0, 0, 0, 0 } },
				{{ 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 },{ 0, 0, 0, 0, 0 } },
				{{ 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 0, 0, 0, 0, 0 } }
			};

			int[,,] map2 = new int[100, 100, 500];
			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					for (int k = 0; k < 500; k++)
					{
						map2[i, j, k] = 0;
						if ((i < 70 && i > 30) && (j < 70 && j > 30) && (k < 450 && k > 200))
							map2[i, j, k] = 1;
					}
				}
			}

			int[,,] map3 = new int[15, 15, 50];
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					for (int k = 0; k < 50; k++)
					{
						map3[i, j, k] = 0;
						if ((i < 8 && i > 4) && (j < 8 && j > 4) && (k < 45 && k > 20))
							map3[i, j, k] = 1;
					}
				}
			}

			CubeAtPos(ref map3, new Vector3D(4, 3, 25), 3);
			CubeAtPos(ref map3, new Vector3D(4, 9, 25), 3);
			CubeAtPos(ref map3, new Vector3D(7, 3, 25), 3);
			CubeAtPos(ref map3, new Vector3D(7, 9, 25), 3);
			CubeAtPos(ref map2, new Vector3D(20, 30, 250), 30);

			var gridmapTest = new Grid3D(map3);
			//ImageBox2 im2 = new ImageBox2(null, gridmapTest,10,50);
			//Application.Run(im2);
			//im2.Dispose();

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			// Get the elapsed time as a TimeSpan value.
			//var thin = PalagySolver.PalagyThinning(map3, new Vector3D[] { new Vector3D(7, 7, 21), new Vector3D(5, 5, 44) });

			var thin = PalagySolver.PalagyThinning(map2, new Vector3D[] { new Vector3D(50, 50, 201), new Vector3D(50, 50, 449) });
			TimeSpan ts = stopWatch.Elapsed;
			stopWatch.Stop();
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
			Console.WriteLine("RunTime " + elapsedTime);
			//var gridmap2 = new Grid3D(map2);
			//Console.WriteLine(Equals(gridmap2, thin));
			//Application.Run(new Form1(gridmap2,thin));


			// Create the converter to convert the matrix to a image


			// Declare an image and store the pixels on it
			ImageBox2 im2 = new ImageBox2(null, gridmapTest, 15, 50);
			Application.Run(im2);

			ImageBox2 im = new ImageBox2(null, thin, 100, 500);
			Application.Run(im);
			im.Dispose();
			var centerline = DjikstraSolver.SolveGridWeightedDistance(thin, new Vector3D(50, 50, 201), new Vector3D(50, 50, 449));


		}


		static bool Equals(Grid3D map1, Grid3D map2)
		{
			bool res = true;
			for (int i = 0; i < map1.grid.GetLength(0); i++)
			{
				for (int j = 0; j < map1.grid.GetLength(1); j++)
				{
					for (int k = 0; k < map1.grid.GetLength(2); k++)
					{
						if (map1.grid[i, j, k].Black != map2.grid[i, j, k].Black)
						{
							res = false;
							break;
						}
					}
				}
			}
			return res;
		}
	}

}
