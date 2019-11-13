using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solver
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
			//int[,] map = {
			//    { 0, 1, 0, 1, 0, 0, 0 },
			//    { 0, 1, 0, 1, 1, 0, 0 },
			//    { 0, 1, 0, 0, 0, 1, 0 },
			//    { 0, 1, 0, 1, 0, 1, 0 },
			//    { 0, 0, 0, 1, 0, 0, 0 },
			//};

			int[,] map2 = {
				{  1, -1,  1,  50,  1,  1, 1 },
				{  1, -1,  1,  1, -1,  1, 1 },
				{  1, -1,  1,  1,  1, -1, 10 },
				{  1, -1,  1, -1,  1, -1, 1 },
				{  1,  1,  1, -1,  1,  1, 1 },
			};


			//Grid grid = new Grid(map);
			Grid grid2 = new Grid(map2);
            //Console.WriteLine(grid.ToString());

            //var Solution=DjikstraSolver.SolveGridUnweighted(grid, new Vector2D(0, 0), new Vector2D(0, 5));

            var SolutionW = DjikstraSolver.SolveGridWeightedDistance(grid2, new Vector2D(0, 0), new Vector2D(5, 0));

            var mazeForm = new Maze();
            Application.Run(mazeForm);
        }
    }
}
