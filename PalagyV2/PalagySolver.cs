using Solver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palagy
{
	public class Stopwatch2 : Stopwatch
	{
		new public void Stop()
		{
			TimeSpan ts = base.Elapsed;
			base.Stop();
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
			Console.WriteLine("RunTime " + elapsedTime);
		}
	}
    public static class PalagySolver
    {
        public static Grid3D PalagyThinning(int[,,] grid,Vector3D[] endPoints = null,float SamplingRate=0.05f)
        {

            Grid3D gridOut = new Grid3D(grid, SamplingRate);
            PalagyThinning2(ref gridOut, endPoints, SamplingRate);
            //PalagyThinning(ref gridOut, endPoints, SamplingRate);

            return gridOut;
        }

        public static void PalagyThinning(ref Grid3D grid, Vector3D[] endPoints = null, float SamplingRate = 0.05f)
        {

            Grid3D gridOut = grid;
            if (endPoints != null)
                foreach (var v in endPoints)
                    gridOut.SetEndPoint(v);
            int Mod = 0;
            Stopwatch2 stopWatch = new Stopwatch2();

            // Get the elapsed time as a TimeSpan value.

            int iteration = 0;
            do
            {
                Mod = 0;
                //Parallel.For(0, 6, index => {
                //	Mod += Subiteration(gridOut, directions[index]);
                //});
                //Task<int>[] taskList = new Task<int>[6];
                //for (int i = 0; i < 6; i++)
                //{
                //	taskList[i] = Task.Run(() => Subiteration(gridOut, directions[i]));
                //}

                //for (int i = 0; i < 6; i++)
                //{
                //	taskList[i].Wait();
                //	Mod += taskList[i].Result;
                //}
                Console.WriteLine($"Iteration: {iteration++}");
                stopWatch.Start();
                Mod += Subiteration(gridOut, Direction.Up);//17
                Mod += Subiteration(gridOut, Direction.Down);//17
                Mod += Subiteration(gridOut, Direction.North);//69 //1
                Mod += Subiteration(gridOut, Direction.South);//68
                Mod += Subiteration(gridOut, Direction.East);//23
                Mod += Subiteration(gridOut, Direction.West);//21
                stopWatch.Stop();
            } while (Mod > 0);
        }

        public static void PalagyThinning2(ref Grid3D grid, Vector3D[] endPoints = null, float SamplingRate = 0.05f)
        {

            Grid3D gridOut = grid;
            if (endPoints != null)
                foreach (var v in endPoints)
                    gridOut.SetEndPoint(v);
            int Mod = 0;
            Stopwatch2 stopWatch = new Stopwatch2();

            // Get the elapsed time as a TimeSpan value.

            int iteration = 0;
            do
            {
                Mod = 0;
                //Parallel.For(0, 6, index => {
                //	Mod += Subiteration(gridOut, directions[index]);
                //});
                //Task<int>[] taskList = new Task<int>[6];
                //for (int i = 0; i < 6; i++)
                //{
                //	taskList[i] = Task.Run(() => Subiteration(gridOut, directions[i]));
                //}

                //for (int i = 0; i < 6; i++)
                //{
                //	taskList[i].Wait();
                //	Mod += taskList[i].Result;
                //}
                Console.WriteLine($"Iteration: {iteration++}");
                stopWatch.Start();
                Mod += Subiteration2(gridOut, Direction.Up);//17
                Mod += Subiteration2(gridOut, Direction.Down);//17
                Mod += Subiteration2(gridOut, Direction.North);//69 //1
                Mod += Subiteration2(gridOut, Direction.South);//68
                Mod += Subiteration2(gridOut, Direction.East);//23
                Mod += Subiteration2(gridOut, Direction.West);//21
                stopWatch.Stop();
            } while (Mod > 0);
        }

        public static int Subiteration(Grid3D grid, Direction dir)
        {
            int mod = 0;

			Queue<Node3D> Candidates = new Queue<Node3D>();
			Queue<Node3D> ShellCandidate = new Queue<Node3D>();
			List<Node3D> Shell = new List<Node3D>();
			//Stopwatch2 stopWatch = new Stopwatch2();
			//stopWatch.Start();

			//--------Metodo facil--------------
			//foreach (Node n in grid.grid)
			//{
			//	//stopWatch.Reset();
			//	if (!n.Black)
			//		continue;
			//	if (n.IsBorde(dir))
			//		if (!n.IsEndPoint)
			//		{
			//			if (n.IsSimple)
			//				Candidates.Enqueue(n);
			//		}
			//}

			//stopWatch.Stop();
			foreach (Node3D n in grid.grid)
			{
				if (n.Black)
				{
					ShellCandidate.Enqueue(n);
					Shell.Add(n);
					n.AddedToCandidate = true;
					break;
				}
			}
			int count = 0;
			while (ShellCandidate.Count > 0)
			{
				Node3D n = ShellCandidate.Dequeue();
				var NoAgregados = n.Neigh26.Where(x => x.Black && !x.AddedToCandidate && !x.IsSurrounded).ToList();
				foreach (var noagregado in NoAgregados)
				{
					ShellCandidate.Enqueue(noagregado);
					Shell.Add(noagregado);
					noagregado.AddedToCandidate = true;
					count++;
				}

				if (n.IsBorde(dir))
					if (!n.IsEndPoint)
						if (n.IsSimple)
							Candidates.Enqueue(n);
			}
			while (Candidates.Count > 0)
            {
                var p = Candidates.Dequeue();
				p.AddedToCandidate = false;
                if (!p.IsEndPoint)
                {
                    if (p.IsSimple)
                    {
                        grid[p.Position].Black = false;
                        mod++;

					}
                }
            }
			Shell.ForEach(x => x.AddedToCandidate = false);
			//var im = new ImageBox2(null, grid,10,50);
			//Application.Run(im);
			//im.Dispose();
			return mod;
        }

        public static int Subiteration2(Grid3D grid, Direction dir)
        {
            int mod = 0;

            Queue<Node3D> Candidates = new Queue<Node3D>();
            Queue<Node3D> ShellCandidate = new Queue<Node3D>();
            List<Node3D> Shell = new List<Node3D>();
            foreach (Node3D n in grid.grid)
            {
                if (n.Black)
                {
                    ShellCandidate.Enqueue(n);
                    Shell.Add(n);
                    n.AddedToCandidate = true;
                    break;
                }
            }
            int count = 0;
            while (ShellCandidate.Count > 0)
            {
                Node3D n = ShellCandidate.Dequeue();
                var NoAgregados = n.Neigh26.Where(x => x.Black && !x.AddedToCandidate && !x.IsSurrounded).ToList();
                foreach (var noagregado in NoAgregados)
                {
                    ShellCandidate.Enqueue(noagregado);
                    Shell.Add(noagregado);
                    noagregado.AddedToCandidate = true;
                    count++;
                }

                if (n.IsBorde(dir))
                    if (!n.IsEndPoint)
                        if (n.IsSimple2)
                            Candidates.Enqueue(n);
            }
            while (Candidates.Count > 0)
            {
                var p = Candidates.Dequeue();
                p.AddedToCandidate = false;
                if (!p.IsEndPoint)
                {
                    if (p.IsSimple2)
                    {
                        grid[p.Position].Black = false;
                        mod++;

                    }
                }
            }
            Shell.ForEach(x => x.AddedToCandidate = false);
            //var im = new ImageBox2(null, grid,10,50);
            //Application.Run(im);
            //im.Dispose();
            return mod;
        }

    }
}
