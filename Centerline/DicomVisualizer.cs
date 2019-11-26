using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DicomImageViewer;
using OpenTKLib;
using Solver;
using OpenCvSharp;
using Palagy;

namespace Centerline
{
    public class DicomVisualizer
    {
        DicomDecoder dicomDecoder;
        List<byte> pixels8 { get { return dicomDecoder.pixels8; } }
        List<ushort> pixels16 { get { return dicomDecoder.pixels16; } }
        List<byte> pixels24 { get { return dicomDecoder.pixels24; } }
        public static Vector3D PathFindingStartPoint;
        public static Vector3D PathFindingEndPoint;


        public DicomVisualizer(string fileName)
        {
            dicomDecoder = new DicomDecoder();
            ReadDicomFile(fileName);
        }

        private void ReadDicomFile(string fileName)
        {
            //X,Y,Z
            double[][][] matriz;
            List<double[][]> listaPrematriz = new List<double[][]>();
            int it = 2;
            while (true)
            {

                var file = fileName + $@"\I.{it.ToString("000")}.dcm";
                try
                {
                    dicomDecoder.DicomFileName = file;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    break; //Dejo de leer
                }
                //Umbralizo 100 - 220
                var Umbral = pixels16.Select(x => x > 80 ? x < 220 ? 1.0 : 0.0 : 0.0).ToArray();
                var Umbral2D = new double[512][];//ConvertArray(Umbral, dicomDecoder.width);
                Mat mat = new Mat(512, 512, MatType.CV_64F, Umbral);
                //Mat destinoDownSampling = new Mat();
                //Cv2.Resize(mat, destinoDownSampling, new OpenCvSharp.Size(512 / 2, 512 / 2));
                Mat destino = new Mat();
               
                //Aplico filtro morfologico (apertura, para eliminar algunos ruidos y sacar conexiones a riniones por ejemplo
                Cv2.MorphologyEx(mat, destino, MorphTypes.Open, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(7, 7))); // 7 7 ideal
                //Application.Run(new Form1(Umbral2D));
                for (int i = 0; i < 512; ++i)
                {
                    Umbral2D[511-i] = new double[512];
                    for (int j = 0; j < 512; ++j)
                    {
                        Umbral2D[511-i][511-j] = destino.At<double>(i, j);
                    }
                }
                listaPrematriz.Add(Umbral2D);
                it++;
            }
            //TODO: Visualizo cortes y me quedo con 2 puntos
            float scale = 0.5f; //Mitad de puntos
            DicomViewerForm dicomViewerForm = new DicomViewerForm(fileName, scale);
            Application.Run(dicomViewerForm);
            dicomViewerForm.Dispose();


            //+1 por el zero padding
            //var PathFindingStartPointTemp = new Vector3D((512 - 291 + 1) * scale, (512 - 13 + 1) * scale, (5 + 1) * scale);//146,7,3
            //PathFindingStartPoint = new Vector3D((int)PathFindingStartPointTemp.Z*0.1f, (int)PathFindingStartPointTemp.Y * 0.1f, (int)PathFindingStartPointTemp.X * 0.1f);


            //var PathFindingEndPointTemp = new Vector3D((512 - 319 + 1) * scale, (512 - 471 + 1) * scale, (51 + 1) * scale);//165,247,29
            //PathFindingEndPoint = new Vector3D((int)PathFindingEndPointTemp.Z*0.1f, (int)PathFindingEndPointTemp.Y * 0.1f,(int)PathFindingEndPointTemp.X * 0.1f);
            //listaPrematriz.Reverse();
            matriz = listaPrematriz.ToArray();


            var matrizDownSample = DownSample(matriz, scale);
            //Region growth given a point
            Vector3D SeedStartPoint = new Vector3D(32 * scale, 256 * scale, 256 * scale);
            var MatrizRG = RegionGrowth(matrizDownSample, SeedStartPoint);
            
            //palagy
            var thinCiclemap = PalagySolver.PalagyThinning(Convert3DArray(MatrizRG),null,0.1f); //new Vector3D[] {PathFindingStartPoint,PathFindingEndPoint }

            //Path finding con los puntos dados
            var solucion = DjikstraSolver.SolveGridWeightedDistance(thinCiclemap, thinCiclemap.GetClosestPointTo(PathFindingStartPoint), thinCiclemap.GetClosestPointTo(PathFindingEndPoint));

            //scale up
            for (int i = 0; i < solucion.Count; ++i)
                solucion[i] = solucion[i] / scale;//scaleo los puntos de la solucion

            //Spline con solucion
            Spline solucionSpline = new Spline(solucion);
            solucionSpline.ConvertToXYZ_file("AortaCenterline", 1000, new Vector3D(32 * (-2) * scale / 10f, 256 * scale / 10f, 256 * scale / 10f));


            //Obtengo todas las centerlines del arbol arterial---------
            //List<Node3D> Endpoints = new List<Node3D>();
            //foreach (var n in thinCiclemap.grid)
            //{
            //    if (n.Black)
            //        if (n.IsEndPoint)
            //            Endpoints.Add(n);
            //}

            //for (int i = 0; i < Endpoints.Count; ++i)
            //{
            //    solucion = DjikstraSolver.SolveGridWeightedDistance(thinCiclemap, thinCiclemap.GetClosestPointTo(PathFindingStartPoint), Endpoints[i].Position);
            //    if (solucion.Count > 0)
            //    {
            //        for (int j = 0; j < solucion.Count; ++j)
            //            solucion[j] = solucion[j] / scale;//scaleo los puntos de la solucion
            //        solucionSpline = new Spline(solucion);
            //        solucionSpline.ConvertToXYZ_file($"AortaCenterline{i}", 1000, new Vector3D(32 * (-2) * scale / 10f, 256 * scale / 10f, 256 * scale / 10f));
            //    }
            //}
            //--------------


            //Visualizo
            thinCiclemap.ConvertToXYZ_file("AortaSkele",new Vector3D(32*(-2)* scale / 10f,256* scale / 10f,256 *scale / 10f));
            ConvertToXYZ_file(MatrizRG, "Aorta", new Vector3D(32*2 * scale / 10f, 256 * scale / 10f, 256 * scale  / 10f));
            var app = new OpenTKForm();
            GLSettings.InitFromSettings_Palagy(false);
            app.LoadModelFromFile("Aorta.xyz");
            app.LoadModelFromFile("AortaSkele.xyz");
            app.LoadModelFromFile("AortaCenterline.xyz");

            //for (int i = 0; i < Endpoints.Count; ++i)
            //{
            //    app.LoadModelFromFile($"AortaCenterline{i}.xyz");
            //}

            Application.Run(app);
            app.Dispose();
        }

        private double[][][] DownSample(double[][][] matriz, float scale)
        {
            //Adds zero padding
            int Xmax = matriz.Length;
            int Ymax = matriz[0].Length;
            int Zmax = matriz[0][0].Length;
            double[][][] Output = new double[Convert.ToInt32((Xmax*scale+2))][][];
            for (int i = 0; i < Xmax* scale+2; ++i)
            {
                Output[i] = new double[Convert.ToInt32((Ymax * scale+2))][];
                for (int j = 0; j < Ymax * scale+2; ++j)
                {
                    Output[i][j] = new double[Convert.ToInt32((Zmax * scale+2))];
                    for (int k = 0; k < Zmax * scale+2; ++k)
                    {
                        if (i == 0) { Output[i][j][k] = 0; continue; }
                        if (j == 0) { Output[i][j][k] = 0; continue; }
                        if (k == 0) { Output[i][j][k] = 0; continue; }
                        if (i == Xmax * scale+1) { Output[i][j][k] = 0; continue; }
                        if (j == Ymax * scale+1) { Output[i][j][k] = 0; continue; }
                        if (k == Zmax * scale+1) { Output[i][j][k] = 0; continue; }
                        Output[i][j][k] = matriz[Convert.ToInt32((i /scale))-1][Convert.ToInt32((j / scale))-1][Convert.ToInt32((k / scale))-1];
                    }
                }
            }
            return Output;

        }

        private double[][][] RegionGrowth(double[][][] matriz, Vector3D SeedPoint)
        {
            int Xmax = matriz.Length;
            int Ymax = matriz[0].Length;
            int Zmax = matriz[0][0].Length;
            bool[][][] visited = new bool[Xmax][][];

            for (int i = 0; i < Xmax; ++i)
            {
                visited[i] = new bool[Ymax][];
                for (int j = 0; j < Ymax; ++j)
                {
                    visited[i][j] = new bool[Zmax];
                    for (int k = 0; k < Zmax; ++k)
                    {
                        visited[i][j][k] = false;
                    }
                }
            }
            Queue<Vector3D> Candidates = new Queue<Vector3D>();
            Candidates.Enqueue(SeedPoint);
            while (Candidates.Count > 0)
            {
                var NodoExaminar = Candidates.Dequeue();
                int X = Convert.ToInt32(NodoExaminar.X); int Y = Convert.ToInt32(NodoExaminar.Y); int Z = Convert.ToInt32(NodoExaminar.Z);
                
                for (int i = X-1; i < X+2; ++i)
                {
                    for (int j = Y-1; j < Y+2; ++j)
                    {
                        for (int k = Z-1; k < Z+2; ++k)
                        {
                            if (i < 0 || j < 0 || k < 0) continue;
                            if (i >= Xmax || j >= Ymax || k >= Zmax) continue;
                            if (i == X && j == Y && k == Z) continue;
                            if (matriz[i][j][k] > 0 && !visited[i][j][k])
                            {
                                Candidates.Enqueue(new Vector3D(i, j, k));
                                visited[i][j][k] = true;
                            }
                        }
                    }
                }

            }
            return visited.Select(x => x.Select(y => y.Select(z => z ? 1.0 : 0.0).ToArray()).ToArray()).ToArray();

        }


        public void ConvertToXYZ_file(double[][][] matriz, string filename, Vector3D Offset = null,float scale = 0.1f)
        {
            List<string> lines = new List<string>();
            int X = matriz.Length;
            int Y = matriz[0].Length;
            int Z = matriz[0][0].Length;
            for (int i = 0; i < X; ++i)
            {
                for (int j = 0; j < Y; ++j)
                {
                    for (int k = 0; k < Z; ++k)
                    {
                        if (matriz[i][j][k] == 1)
                        {
                            string line;
                            if(Offset==null)
                                line = $"{(i)*scale} {(j)*scale} {(k)*scale}";
                            else
                                line = $"{(i) * scale - Offset.X} {(j) * scale - Offset.Y} {(k) * scale - Offset.Z}";
                            lines.Add(line);
                        }
                    }
                }
            }

            File.WriteAllLines(filename + ".xyz", lines.ToArray());

        }

        static public double[,] Convert2DArray(bool[] Input, int size)
        {
            double[,] Output = new double[(Input.Length / size), size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size, j] = Input[i + j]?1:0;
                }
            }
            return Output;
        }
        static public double[,] Convert2DArray(double[] Input, int size)
        {
            double[,] Output = new double[(Input.Length / size), size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size, j] = Input[i + j];
                }
            }
            return Output;
        }

        static public double[,] Convert2DArray(ushort[] Input, int size)
        {
            double[,] Output = new double[(Input.Length / size), size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size, j] = Input[i + j]/256.0;
                }
            }
            return Output;
        }

        static public double[][] ConvertArray(bool[] Input, int size)
        {
            double[][] Output = new double[(Input.Length / size)][];
            for(int i=0;i< (Input.Length / size);++i)
                Output[i] = new double[size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size][j] = Input[i + j] ? 1 : 0;
                }
            }
            return Output;
        }

        static public double[][] ConvertArray(double[] Input, int size)
        {
            double[][] Output = new double[(Input.Length / size)][];
            for (int i = 0; i < (Input.Length / size); ++i)
                Output[i] = new double[size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size][j] = Input[i + j];
                }
            }
            return Output;
        }

        static public int[,,] Convert3DArray(double[][][] Input)
        {
            int X = Input.Length;
            int Y = Input[0].Length;
            int Z = Input[0][0].Length;
            int[,,] Output = new int[X,Y,Z];
            for(int i = 0; i < X; ++i)
            {
                for (int j = 0; j < Y; ++j)
                {
                    for (int k = 0; k < Z; ++k)
                    {
                        Output[i, j, Z - k - 1] = Convert.ToInt32(Input[i][j][Z-k-1]);
                    }
                }
            }
            return Output;
        }

    }
}
