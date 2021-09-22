using System;
using System.Linq;
using Solver;
using Palagy;
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
            // Data from https://simvascular.github.io/clinicalCase1.html
            DicomVisualizer dv = new DicomVisualizer(@"C:\Users\damia\Downloads\AortofemoralNormal1\OSMSC0110-aorta\image_data\volume");




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
            CubeAtPos(ref map2, new Vector3D(20, 35, 200), 15);
            CubeAtPos(ref map2, new Vector3D(25, 15, 170), 13);

            int[,,] circleMap = new int[50, 50, 250];
            double center = 25;
            int radius = 10;
            int radius2 = 15;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    //center = 25;
                    for (int k = 0; k < 250; k++)
                    {
                        center = 25 + 3 * Math.Sin(k / 250f * 2 * Math.PI);
                        circleMap[i, j, k] = 0;
                        if (k > 20 && k < 230)
                        {
                            Vector2D test = new Vector2D((int)(Math.Round(i - center)), (int)(Math.Round(j - center)));
                            if (test.Length < radius)
                                circleMap[i, j, k] = 1;
                        }
                        if (k > 120 && k < 150)
                        {
                            Vector2D test = new Vector2D((int)(Math.Round(i - center)), (int)(Math.Round(j - center)));
                            if (test.Length < radius2)
                                circleMap[i, j, k] = 1;
                        }
                    }
                }
            }
            //CubeAtPos(ref map2, new Vector3D(35, 35, 150), 10);
            //for (int i = -25; i < 4; i++)
            //{
            //    for (int j = -3; j < 4; j++)
            //    {
            //        for (int k = 0; k < 250; k++)
            //        {
            //            map2[i + 25, j + 25, k] = 0;
            //        }
            //    }
            //}
            //Grid3D map2_grid = new Grid3D(map2,0.05f);
            Grid3D CicleMapGrid = new Grid3D(circleMap, 0.05f);
            var StartPoint = new Vector3D(25 * 0.05f, 25 * 0.05f, 101 * 0.05f);
            var EndPoint = new Vector3D(25 * 0.05f, 25 * 0.05f, 224 * 0.05f);
            //var thin = PalagySolver.PalagyThinning(map2, new Vector3D[] { StartPoint, EndPoint },0.05f);

            //var thin = PalagySolver.PalagyThinning(map2, new Vector3D[] { StartPoint, EndPoint });

            var thinCiclemap = PalagySolver.PalagyThinning(circleMap, new Vector3D[] { StartPoint, EndPoint });

            //PalagySolver.PalagyThinning(ref thin);
            //Djistra
            //var Solution = DjikstraSolver.SolveGridWeightedDistance(thin, StartPoint, EndPoint);

            //Spline
            //Spline SolutionSpline = new Spline(Solution);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var app = new OpenTKForm();
            GLSettings.InitFromSettings_Palagy(false);

            //map2_grid.ConvertToXYZ_file("Map2");
            //CicleMapGrid.ConvertToXYZ_file("Circle");
            //thin.ConvertToXYZ_file("Thin");
            thinCiclemap.ConvertToXYZ_file("Thin");
            //SolutionSpline.ConvertToXYZ_file("Centerline");

            app.LoadModelFromFile("Thin.xyz");
            //app.LoadModelFromFile("Map2.xyz");
            //app.LoadModelFromFile("Circle.xyz");
            //app.LoadModelFromFile("Centerline.xyz");

            Application.Run(app);

        }
	}
}
