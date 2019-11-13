using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
	public class Grid3D
	{
		public Node3D[,,] grid;
		private int[] Size;
		private float SamplingRate;
		public Grid3D(int N, int M, int L)
		{
			grid = new Node3D[N, M, L];
		}

		public void ConvertToXYZ_file(string filename,Vector3D Offset=null)
		{
			List<string> lines = new List<string>();
			if(Offset==null)
				Offset = new Vector3D(1.25f, 1.25f, 6.25f);
			foreach(var node in grid)
			{
				if (node.Black)
				{
					var imprimir = node.Position - Offset;
					string line = $"{imprimir.X} {imprimir.Y} {imprimir.Z}";
					lines.Add(line);
				}
			}
			File.WriteAllLines(filename+".xyz", lines.ToArray());

		}

        public void Scale(float scale)
        {

        }

		/// <summary>
		/// Creates a grid given a 3D matrix and a Sampling rate
		/// </summary>
		/// <param name="map"></param>
		/// <param name="SamplingRate">Sampling rate in mm/point</param>
		public Grid3D(int[,,] map,float SamplingRate=1)
		{
			this.SamplingRate = SamplingRate;
			Size = new int[3];
			Size[0] = map.GetLength(0);
			Size[1] = map.GetLength(1);
			Size[2] = map.GetLength(2);
			grid = new Node3D[Size[0], Size[1], Size[2]];
			for (int i = 0; i < Size[0]; i++)
			{
				for (int j = 0; j < Size[1]; j++)
				{
					for (int k = 0; k < Size[2]; k++)
					{
						grid[i, j, k] = new Node3D(new Vector3D(i*SamplingRate, j* SamplingRate, k* SamplingRate), map[i, j, k] == 1);
					}
				}
			}

			for (int i = 0; i < Size[0]; i++)
			{
				for (int j = 0; j < Size[1]; j++)
				{
					for (int k = 0; k < Size[2]; k++)
					{
						if (j != 0 && i != 0 && k != 0)//23
							grid[i, j, k].AddNeighbour26(grid?[i - 1, j - 1, k - 1], 0);
						if (j != 0 && k != 0)//24
							grid[i, j, k].AddNeighbour18(grid?[i, j - 1, k - 1], 1);
						if (j != 0 && i + 1 < Size[0] && k != 0)//25
							grid[i, j, k].AddNeighbour26(grid?[i + 1, j - 1, k - 1], 2);
						if (i != 0 && k != 0)//20
							grid[i, j, k].AddNeighbour18(grid?[i - 1, j, k - 1], 3);
						if (k != 0) //21
							grid[i, j, k].AddNeighbour6(grid?[i, j, k - 1], Direction.Up, 4);
						if (i + 1 < Size[0] && k != 0)//22
							grid[i, j, k].AddNeighbour18(grid?[i + 1, j, k - 1], 5);
						if (i != 0 && j + 1 < Size[1] && k != 0)//17
							grid[i, j, k].AddNeighbour26(grid?[i - 1, j + 1, k - 1], 6);
						if (j + 1 < Size[1] && k != 0)//18
							grid[i, j, k].AddNeighbour18(grid?[i, j + 1, k - 1], 7);
						if (i + 1 < Size[0] && j + 1 < Size[1] && k != 0)//19
							grid[i, j, k].AddNeighbour26(grid?[i + 1, j + 1, k - 1], 8);

						if (j != 0 && i != 0)//14
							grid[i, j, k].AddNeighbour18(grid?[i - 1, j - 1, k], 9);
						if (j != 0)//15
							grid[i, j, k].AddNeighbour6(grid?[i, j - 1, k], Direction.North, 10);
						if (j != 0 && i + 1 < Size[0])//16
							grid[i, j, k].AddNeighbour18(grid?[i + 1, j - 1, k], 11);
						if (i != 0)//12
							grid[i, j, k].AddNeighbour6(grid?[i - 1, j, k], Direction.West, 12);
						if (i + 1 < Size[0])//13
							grid[i, j, k].AddNeighbour6(grid?[i + 1, j, k], Direction.East, 13);
						if (i != 0 && j + 1 < Size[1])//9
							grid[i, j, k].AddNeighbour18(grid?[i - 1, j + 1, k], 14);
						if (j + 1 < Size[1])//10
							grid[i, j, k].AddNeighbour6(grid?[i, j + 1, k], Direction.South, 15);
						if (i + 1 < Size[0] && j + 1 < Size[1])//11
							grid[i, j, k].AddNeighbour18(grid?[i + 1, j + 1, k], 16);

						if (j != 0 && i != 0 && k + 1 < Size[2])//6
							grid[i, j, k].AddNeighbour26(grid?[i - 1, j - 1, k + 1], 17);
						if (j != 0 && k + 1 < Size[2])//7
							grid[i, j, k].AddNeighbour18(grid?[i, j - 1, k + 1], 18);
						if (j != 0 && i + 1 < Size[0] && k + 1 < Size[2])//0
							grid[i, j, k].AddNeighbour26(grid?[i + 1, j - 1, k + 1], 19);
						if (i != 0 && k + 1 < Size[2])//5
							grid[i, j, k].AddNeighbour18(grid?[i - 1, j, k + 1], 20);
						if (k + 1 < Size[2]) //4
							grid[i, j, k].AddNeighbour6(grid?[i, j, k + 1], Direction.Down, 21);
						if (i + 1 < Size[0] && k + 1 < Size[2])//3
							grid[i, j, k].AddNeighbour18(grid?[i + 1, j, k + 1], 22);
						if (i != 0 && j + 1 < Size[1] && k + 1 < Size[2])//8
							grid[i, j, k].AddNeighbour26(grid?[i - 1, j + 1, k + 1], 23);
						if (j + 1 < Size[1] && k + 1 < Size[2])//1
							grid[i, j, k].AddNeighbour18(grid?[i, j + 1, k + 1], 24);
						if (i + 1 < Size[0] && j + 1 < Size[1] && k + 1 < Size[2])//2
							grid[i, j, k].AddNeighbour26(grid?[i + 1, j + 1, k + 1], 25);
					}
				}
			}
		}

		public void Reset()
		{
			for (int i = 0; i < Size[0]; i++)
			{
				for (int j = 0; j < Size[1]; j++)
				{
					for (int k = 0; k < Size[2]; k++)
					{
						if (grid[i, j, k].Visited)
						{
							grid[i, j, k].Visited = false;
							//grid[i, j, k].Distance = 500000;
						}
					}
				}
			}
		}

		public void SetEndPoint(Vector3D v)
		{
			this[v].IsEndPoint = true;
		}

		public Node3D GetNodeAtPos(Vector3D v)
		{
			return grid[Convert.ToInt32(v.X/ SamplingRate), Convert.ToInt32(v.Y / SamplingRate), Convert.ToInt32(v.Z / SamplingRate)];
		}

		public void SetNodeAtPos(Vector3D v, Node3D n)
		{
			grid[Convert.ToInt32(v.X / SamplingRate), Convert.ToInt32(v.Y / SamplingRate), Convert.ToInt32(v.Z / SamplingRate)] = n;
		}

		public Node3D this[Vector3D key]
		{
			get => GetNodeAtPos(key);
			set => SetNodeAtPos(key, value);
		}

	}


	public class Grid
    {
        public Node[,] grid;
        private int[] Size;
        public Grid(int N, int M)
        {
            grid = new Node[N, M];
        }

        public Grid(int[,] map,bool EightNeigh=false)
        {
            Size = new int[2];
            Size[0] = map.GetLength(1);
            Size[1] = map.GetLength(0);
            grid = new Node[Size[0], Size[1]];
            for (int i = 0; i < Size[0]; i++)
            {
                for (int j = 0; j < Size[1]; j++)
                {
                    grid[i, j] = new Node(map[j, i]);
                    grid[i, j].Position = new Vector2D(i, j);
                }
            }

            for (int i = 0; i < Size[0]; i++)
            {
                for (int j = 0; j < Size[1]; j++)
                {

                    if (j + 1 < Size[1])
                        grid[i, j].AddNeighbour(grid?[i, j + 1]);
                    if (j != 0)
                        grid[i, j].AddNeighbour(grid?[i, j - 1]);
                    if (i + 1 < Size[0])
                        grid[i, j].AddNeighbour(grid?[i + 1, j]);
                    if (i != 0)
                        grid[i, j].AddNeighbour(grid?[i - 1, j]);

                    if (EightNeigh)
                    {
                        if (j != 0 && i + 1 < Size[0])
                            grid[i, j].AddNeighbour(grid?[i + 1, j - 1]);
                        if (j != 0 && i!=0)
                            grid[i, j].AddNeighbour(grid?[i - 1, j - 1]);
                        if (i + 1 < Size[0] && j + 1 < Size[1])
                            grid[i, j].AddNeighbour(grid?[i + 1, j + 1]);
                        if (i != 0 && j + 1 < Size[1])
                            grid[i, j].AddNeighbour(grid?[i - 1, j + 1]);
                    }
                }
            }
        }

        public void Reset()
        {
            for(int i = 0; i < Size[0]; i++)
            {
                for (int j = 0; j < Size[1]; j++)
                    if (grid[i, j].Visited)
                    {
                        grid[i, j].Visited = false;
                        grid[i, j].Distance = 500000;
                    }
                        
            }
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Size[0]; i++)
            {
                string line = "";
                for (int j = 0; j < Size[1]; j++)
                {
                    line += $"{grid[i, j].Weight} ";
                }
                line += "\n";
                res += line;
            }
            return res;
        }

        public Node GetNodeAtPos(Vector2D v)
        {
            return grid[v.x, v.y];
        }

        public void SetNodeAtPos(Vector2D v, Node n)
        {
            grid[v.x, v.y] = n;
        }

        public Node this[Vector2D key]
        {
            get => GetNodeAtPos(key);
            set => SetNodeAtPos(key, value);
        }

    }
}


/*public class Grid
    {
        public Node[,] grid;
        private int[] Size;
        public Grid(int N, int M)
        {
            grid = new Node[N, M];
        }

        public Grid(int[,] map)
        {
            Size = new int[2];
            Size[0] = map.GetLength(0);
            Size[1] = map.GetLength(1);
            grid = new Node[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    grid[i, j] = new Node(map[i, j]);
                    grid[i, j].Position = new Vector2D(i, j);
                }
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (j + 1 < map.GetLength(1))
                        grid[i, j].AddNeighbour(grid?[i, j + 1]);
                    if (j != 0)
                        grid[i, j].AddNeighbour(grid?[i, j - 1]);
                    if (i + 1 < map.GetLength(0))
                        grid[i, j].AddNeighbour(grid?[i + 1, j]);
                    if (i != 0)
                        grid[i, j].AddNeighbour(grid?[i - 1, j]);
                }
            }
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Size[0]; i++)
            {
                string line = "";
                for (int j = 0; j < Size[1]; j++)
                {
                    line += $"{grid[i, j].Weight} ";
                }
                line += "\n";
                res += line;
            }
            return res;
        }

        public Node GetNodeAtPos(Vector2D v)
        {
            return grid[v.x, v.y];
        }

        public void SetNodeAtPos(Vector2D v, Node n)
        {
            grid[v.x, v.y] = n;
        }

        public Node this[Vector2D key]
        {
            get => GetNodeAtPos(key);
            set => SetNodeAtPos(key, value);
        }

    }
}
*/