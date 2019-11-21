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
               
                Cv2.MorphologyEx(mat, destino, MorphTypes.Open, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(7, 7)));
                //Application.Run(new Form1(Umbral2D));
                for (int i = 0; i < 512; ++i)
                {
                    Umbral2D[i] = new double[512];
                    for (int j = 0; j < 512; ++j)
                    {
                        Umbral2D[i][j] = destino.At<double>(i, j);
                    }
                }
                listaPrematriz.Add(Umbral2D);
                it++;
            }


            //listaPrematriz.Reverse();
            matriz = listaPrematriz.ToArray();

            //TODO: Scale (Downsampling using Nearest Neighbour 3D!
            float scale = 0.5f; //Mitad de puntos
            var matrizDownSample = DownSample(matriz, scale);
            //Region growth given a point
            Vector3D StartPoint = new Vector3D(32 * scale, 256 * scale, 256 * scale);
            var MatrizRG = RegionGrowth(matrizDownSample, StartPoint);
            
            var thinCiclemap = PalagySolver.PalagyThinning(Convert3DArray(MatrizRG),null,0.1f);

            thinCiclemap.ConvertToXYZ_file("test3Thin");
            ConvertToXYZ_file(MatrizRG, "test3");
            var app = new OpenTKForm();
            //GLSettings.InitFromSettings_Palagy(false);
            app.LoadModelFromFile("test3.xyz");
            app.LoadModelFromFile("test3Thin.xyz");
            Application.Run(app);
            app.Dispose();
        }

        private double[][][] DownSample(double[][][] matriz, float scale)
        {
            //Adds zero padding
            int Xmax = matriz.Length;
            int Ymax = matriz[0].Length;
            int Zmax = matriz[0][0].Length;
            double[][][] Output = new double[(int)(Xmax*scale+2)][][];
            for (int i = 0; i < Xmax* scale+2; ++i)
            {
                Output[i] = new double[(int)(Ymax * scale+2)][];
                for (int j = 0; j < Ymax * scale+2; ++j)
                {
                    Output[i][j] = new double[(int)(Zmax * scale+2)];
                    for (int k = 0; k < Zmax * scale+2; ++k)
                    {
                        if (i == 0) { Output[i][j][k] = 0; continue; }
                        if (j == 0) { Output[i][j][k] = 0; continue; }
                        if (k == 0) { Output[i][j][k] = 0; continue; }
                        if (i == Xmax * scale+1) { Output[i][j][k] = 0; continue; }
                        if (j == Ymax * scale+1) { Output[i][j][k] = 0; continue; }
                        if (k == Zmax * scale+1) { Output[i][j][k] = 0; continue; }
                        Output[i][j][k] = matriz[(int)(i /scale)-1][(int)(j / scale)-1][(int)(k / scale)-1];
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
                int X = (int)NodoExaminar.X; int Y = (int)NodoExaminar.Y; int Z = (int)NodoExaminar.Z;
                
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


        public void ConvertToXYZ_file(double[][][] matriz, string filename, Vector3D Offset = null)
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
                            string line = $"{(i-32)/ (float)10} {(j-256)/ (float)10} {(k-256)/(float)10}";
                            lines.Add(line);
                        }
                    }
                }
            }

            File.WriteAllLines(filename + ".xyz", lines.ToArray());

        }

        private double[,] Convert2DArray(bool[] Input, int size)
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
        private double[,] Convert2DArray(double[] Input, int size)
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

        private double[][] ConvertArray(bool[] Input, int size)
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

        private double[][] ConvertArray(double[] Input, int size)
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

        private int[,,] Convert3DArray(double[][][] Input)
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
                        Output[i, j, Z - k - 1] = (int)Input[i][j][Z-k-1];
                    }
                }
            }
            return Output;
        }

    }
}
