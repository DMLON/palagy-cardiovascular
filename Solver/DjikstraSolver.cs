using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public static class DjikstraSolver
    {
        public static List<Vector2D> SolveGridUnweighted(Grid Grid, Vector2D Start, Vector2D Finish)
        {
            var solution = new List<Vector2D>();
            bool solvable = false;
            Queue<Node> Candidates = new Queue<Node>();
            Candidates.Enqueue(Grid[Start]);
            Grid[Start].Visited = true;
            while (Candidates.Count > 0)
            {
                var CheckNode = Candidates.Dequeue();
                if (CheckNode == Grid[Finish])
                {
                    //Found Solution
                    solvable = true;
                    break;
                }
                //Check neighbours if are visited and if they are visitable
                foreach (var neigh in CheckNode.Neighbours)
                {
                    if (!neigh.Visited && neigh.Weight != -1)
                    {
                        neigh.Visited = true;
                        //If they are visitable, add to candidates
                        neigh.Parent = CheckNode;
                        Candidates.Enqueue(neigh);
                    }
                }

            }
            if (!solvable)
                return null;

            Node currNode = Grid[Finish];
            solution.Add(currNode.Position);
            while (currNode != Grid[Start])
            {
                currNode = currNode.Parent;
                solution.Add(currNode.Position);
            }
            solution.Reverse();
            return solution;
        }

		/// <summary>
		/// Gives the solution to the given Grid using distance only to get the optimun path
		/// </summary>
		/// <param name="Grid">Map grid with -1 as impenetrable wall</param>
		/// <param name="Start">Start point</param>
		/// <param name="Finish">End point</param>
		/// <returns></returns>
		public static List<Vector2D> SolveGridUnweightedDistance(Grid Grid, Vector2D Start, Vector2D Finish)
        {
            var solution = new List<Vector2D>();
            bool solvable = false;
			PriorityQueue<Node> Candidates = new PriorityQueue<Node>();
			Candidates.Enqueue(Grid[Start]);
            Grid[Start].Visited = true;
            Grid[Start].Distance = 0;
            while (Candidates.Count() > 0)
            {
                var CheckNode = Candidates.Dequeue();
                if (CheckNode == Grid[Finish])
                {
                    //Found Solution
                    solvable = true;
                    break;
                }
                //Check neighbours if are visited and if they are visitable
                foreach (var neigh in CheckNode.Neighbours)
                {
                    if (!neigh.Visited && neigh.Weight != -1)
                    {
                        neigh.Visited = true;
                        //If they are visitable, add to candidates
                        neigh.Parent = CheckNode;
                        neigh.Distance = CheckNode.Distance + neigh.DistanceTo(CheckNode);
                        Candidates.Enqueue(neigh);
                    }
                }

            }
            if (!solvable)
                return solution;

            Node currNode = Grid[Finish];
            solution.Add(currNode.Position);
            while (currNode != Grid[Start])
            {
                currNode = currNode.Parent;
                solution.Add(currNode.Position);
            }
            solution.Reverse();
            return solution;
        }

		/// <summary>
		/// Gives the solution to the given Grid using distance only to get the optimun path LENTO
		/// </summary>
		/// <param name="Grid">Map grid with -1 as impenetrable wall</param>
		/// <param name="Start">Start point</param>
		/// <param name="Finish">End point</param>
		/// <returns></returns>
		public static List<Vector2D> SolveGridUnweightedDistance2(Grid Grid, Vector2D Start, Vector2D Finish)
		{
			var solution = new List<Vector2D>();
			bool solvable = false;
			//PriorityQueue<Node> Candidates = new PriorityQueue<Node>();
			List<Node> Candidates = new List<Node>();
			Candidates.Add(Grid[Start]);
			Grid[Start].Visited = true;
			Grid[Start].Distance = 0;
			while (Candidates.Count() > 0)
			{
				var minimo = Candidates.Min(y => y.DistanceWeight);
				var CheckNode = Candidates.Where(x => x.DistanceWeight == minimo).First();
				Candidates.Remove(CheckNode);
				//var CheckNode = Candidates.Dequeue();
				if (CheckNode == Grid[Finish])
				{
					//Found Solution
					solvable = true;
					break;
				}
				//Check neighbours if are visited and if they are visitable
				foreach (var neigh in CheckNode.Neighbours)
				{
					if (!neigh.Visited && neigh.Weight != -1)
					{
						neigh.Visited = true;
						//If they are visitable, add to candidates
						neigh.Parent = CheckNode;
						neigh.Distance = CheckNode.Distance + neigh.DistanceTo(CheckNode);
						Candidates.Add(neigh);
					}
				}

			}
			if (!solvable)
				return solution;

			Node currNode = Grid[Finish];
			solution.Add(currNode.Position);
			while (currNode != Grid[Start])
			{
				currNode = currNode.Parent;
				solution.Add(currNode.Position);
			}
			solution.Reverse();
			return solution;
		}

		/// <summary>
		/// Gives the solution to the given Grid using Weights and distance to get the optimun path
		/// </summary>
		/// <param name="Grid">Map grid with -1 as impenetrable wall</param>
		/// <param name="Start">Start point</param>
		/// <param name="Finish">End point</param>
		/// <returns></returns>
		public static List<Vector2D> SolveGridWeightedDistance(Grid Grid, Vector2D Start, Vector2D Finish)
		{
			var solution = new List<Vector2D>();
			bool solvable = false;
			PriorityQueue<Node> Candidates = new PriorityQueue<Node>();
			Candidates.Enqueue(Grid[Start]);
			Grid[Start].Visited = true;
			Grid[Start].Distance = 0;
			while (Candidates.Count() > 0)
			{
				var CheckNode = Candidates.Dequeue();
				if (CheckNode == Grid[Finish])
				{
					//Found Solution
					solvable = true;
					break;
				}
				//Check neighbours if are visited and if they are visitable
				foreach (var neigh in CheckNode.Neighbours)
				{
					if (!neigh.Visited && neigh.Weight != -1)
					{
						neigh.Visited = true;
						//If they are visitable, add to candidates
						neigh.Parent = CheckNode;
						neigh.Distance = CheckNode.Distance + neigh.DistanceTo(CheckNode);
						Candidates.Enqueue(neigh);
					}
				}

			}
			if (!solvable)
				return solution;

			Node currNode = Grid[Finish];
			solution.Add(currNode.Position);
			while (currNode != Grid[Start])
			{
				currNode = currNode.Parent;
				solution.Add(currNode.Position);
			}
			solution.Reverse();
			return solution;
		}

		public static bool SolveGridUnweighted(Grid3D Grid, Vector3D Start, Vector3D Finish)
		{
			bool solvable = false;
			Queue<Node3D> Candidates = new Queue<Node3D>();
			Candidates.Enqueue(Grid[Start]);
			Grid[Start].Visited = true;
			while (Candidates.Count > 0)
			{
				var CheckNode = Candidates.Dequeue();
				if (CheckNode == Grid[Finish])
				{
					//Found Solution
					solvable = true;
					break;
				}
				//Check neighbours if are visited and if they are visitable
				foreach (var neigh in CheckNode.Neigh26)
				{
					if (!neigh.Visited && neigh.Black)
					{
						neigh.Visited = true;
						//If they are visitable, add to candidates
						Candidates.Enqueue(neigh);
					}
				}

			}
			return solvable;
		}


		public static bool SolveGridUnweightedNeigh6(Grid3D Grid, Vector3D Start, Vector3D Finish)
		{
			bool solvable = false;
			Queue<Node3D> Candidates = new Queue<Node3D>();
			Candidates.Enqueue(Grid[Start]);
			Grid[Start].Visited = true;
			while (Candidates.Count > 0)
			{
				var CheckNode = Candidates.Dequeue();
				if (CheckNode == Grid[Finish])
				{
					//Found Solution
					solvable = true;
					break;
				}
				//Check neighbours if are visited and if they are visitable
				foreach (var neigh in CheckNode.Neigh6)
				{
					if (!neigh.Visited && neigh.Black)
					{
						neigh.Visited = true;
						//If they are visitable, add to candidates
						Candidates.Enqueue(neigh);
					}
				}

			}
			return solvable;
		}

		public static List<Vector3D> SolveGridWeightedDistance(Grid3D Grid, Vector3D Start, Vector3D Finish)
		{
			var solution = new List<Vector3D>();
			bool solvable = false;
			PriorityQueue<Node3D> Candidates = new PriorityQueue<Node3D>();
            Grid.Reset();
			Candidates.Enqueue(Grid[Start]);
			Grid[Start].Visited = true;
			Grid[Start].Distance = 0;
			while (Candidates.Count() > 0)
			{
				var CheckNode = Candidates.Dequeue();
				if (CheckNode == Grid[Finish])
				{
					//Found Solution
					solvable = true;
					break;
				}
				//Check neighbours if are visited and if they are visitable
				foreach (var neigh in CheckNode.Neigh26)
				{
					if (!neigh.Visited && neigh.Weight != -1)
					{
						neigh.Visited = true;
						//If they are visitable, add to candidates
						neigh.Parent = CheckNode;
						neigh.Distance = CheckNode.Distance + neigh.DistanceTo(CheckNode);
						Candidates.Enqueue(neigh);
					}
				}

			}
			if (!solvable)
				return solution;

			Node3D currNode = Grid[Finish];
			solution.Add(currNode.Position);
			while (currNode != Grid[Start])
			{
				currNode = currNode.Parent;
				solution.Add(currNode.Position);
			}
			solution.Reverse();
			return solution;
		}

	}
}
