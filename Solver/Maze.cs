using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solver
{
    public partial class Maze : Form
    {
        int[,] mazeImage;
        public Maze()
        {
            InitializeComponent();
            mazeImage = new int[pictureBox1.Width, pictureBox1.Height];
        }
        Vector2D _StartPos;
        Vector2D _EndPos;
        Vector2D StartPos { get { return _StartPos; } set { label_StartPos.Text = $"X:{value.x}, Y:{value.y}"; _StartPos = value; } }
        Vector2D EndPos { get { return _EndPos; } set { label_endPos.Text = $"X:{value.x}, Y:{value.y}"; _EndPos = value; } }
        Grid mazeGrid;
        //private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    firstClick = !firstClick;
        //    //label_MousePos.Text = $"X: {e.X}\nY: {e.Y}";
        //    if (firstClick) 
        //        StartPos = new Vector2D(e.X, e.Y);
        //    if (!firstClick)
        //        EndPos = new Vector2D(e.X, e.Y);
        //}


        public int[,] ConvertArray(int[] Input, int size)
        {
            int[,] Output = new int[(Input.Length / size), size];
            for (int i = 0; i < Input.Length; i += size)
            {
                for (int j = 0; j < size; j++)
                {
                    Output[i / size, j] = Input[i + j];
                }
            }
            return Output;
        }

        private void Maze_Load(object sender, EventArgs e)
        {
            LoadImage(pictureBox1);

        }

        void LoadImage(PictureBox pic)
        {
            Bitmap bmp = new Bitmap(pic.Image);
            //Bitmap bmp = new Bitmap("TestColor.png");
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            //Se lee raro, como BGR BGR BGR 0,0,0 (Esos 0,0,0 es como fin de linea)
            byte[] rgbValues = new byte[bytes];
            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            byte a = 255;
            List<int> lista = new List<int>();
            for (int i = 0; i < bytes; i += 4)
                lista.Add((rgbValues[i]>127?-1:1));

            var mazeImage = ConvertArray(lista.ToArray(), bmp.Width);
            mazeGrid = new Grid(mazeImage, true);
        }

        private void PrintSolution(List<Vector2D> solution)
        {
            //Bitmap bmp = new Bitmap(pictureBox1.Image);
            if (solution.Count == 0)
            {
				MessageBox.Show("Solution not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

			Bitmap blank = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
			Graphics g = Graphics.FromImage(blank);
			g.Clear(Color.White);
			g.DrawImage(pictureBox1.Image, 0, 0);
			Pen pen = new Pen(Color.Red);
			pen.Width = 3;

			for (int i = 0; i < solution.Count - 1; i++)
				g.DrawLine(pen, solution[i].x, solution[i].y, solution[i + 1].x, solution[i + 1].y);

			Bitmap tempImage = new Bitmap(blank);
			//blank.Dispose();

			tempImage.Save("Maze6Solved.png", ImageFormat.Png);
			//pen.Dispose();

			pictureBox1.Image = tempImage;

			//tempImage.Dispose();
			//using (var graphics = pictureBox1.CreateGraphics())
			//using(Pen pen = new Pen(Color.Red))
			//{
			//	pen.Width = 3;
			//	//graphics.DrawImage(pictureBox1.Image, 0, 0);


			//	Bitmap a = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height, graphics);
			//	a.Save("Maze6Solved.png", ImageFormat.Png);
			//}

		}

        private void button_solveMaze_Click(object sender, EventArgs e)
        {
            
            mazeGrid.Reset();

			var watch = System.Diagnostics.Stopwatch.StartNew();
			var l1 = DjikstraSolver.SolveGridUnweightedDistance(mazeGrid, StartPos, new Vector2D(1799, 1799));
			watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;

			//var watch2 = System.Diagnostics.Stopwatch.StartNew();
			//var l2 = DjikstraSolver.SolveGridUnweightedDistance2(mazeGrid, StartPos, new Vector2D(1799, 1799));
			//watch2.Stop();
			//var elapsedMs2 = watch2.ElapsedMilliseconds;

			Console.WriteLine($"PriorityQueue time = {elapsedMs}");
			//Console.WriteLine($"List LINQ time = {elapsedMs2}");
			PrintSolution(l1);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                StartPos = new Vector2D(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
                EndPos = new Vector2D(e.X, e.Y);

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                StartPos = new Vector2D(e.X, e.Y);
            }
                
            if (e.Button == MouseButtons.Right)
            {
                EndPos = new Vector2D(e.X, e.Y);
            }
                
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"C:\Users\Universidad Favaloro\Downloads\MazeSolver\MazeSolver\Solver\Maze6.png");
            LoadImage(pictureBox1);
        }
    }
}
