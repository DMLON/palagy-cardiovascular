using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solver;
using Palagy;
using OpenTKTest;
using OpenTKLib;
using System.Windows.Forms;

namespace Centerline
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

		[STAThread]
		static void Main(string[] args)
		{
			int[,,] map2 = new int[50, 50, 250];
			for (int i = 0; i < 50; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					for (int k = 0; k < 250; k++)
					{
						map2[i, j, k] = 0;
						if ((i < 35 && i > 15) && (j < 35 && j > 15) && (k < 225 && k > 100))
							map2[i, j, k] = 1;
					}
				}
			}
			CubeAtPos(ref map2, new Vector3D(10, 15, 125), 10);

			Grid3D map2_grid = new Grid3D(map2,0.05f);

			var thin = PalagySolver.PalagyThinning(map2, new Vector3D[] { new Vector3D(30* 0.05f, 30* 0.05f, 101* 0.05f), new Vector3D(17* 0.05f, 20* 0.05f, 224* 0.05f) },0.05f);
			thin.ConvertToXYZ_file("test");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var app = new OpenTKForm();
			GLSettings.InitFromSettings_Palagy();
			app.LoadModelFromFile("test.xyz");

			map2_grid.ConvertToXYZ_file("test2");

			app.LoadModelFromFile("test2.xyz");
			Application.Run(app);

		}
	}
}
