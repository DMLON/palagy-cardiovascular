using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
	public enum Direction { Up, Down, North, South, East, West };

	public class Node : IComparable<Node>
	{
		public int Weight { get; private set; }
		public bool Visited { get; set; }
		public List<Node> Neighbours { get; private set; }
		public Node Parent;
		public Vector2D Position { get; set; }
		public double Distance;
		public double DistanceWeight { get { return Distance * Weight; } }
		public Node(int weight = 0)
		{
			Neighbours = new List<Node>(4);
			Distance = 500000;
			this.Weight = weight;
			Visited = false;
			
		}
		public void AddNeighbour(Node n)
		{
			if (!Neighbours.Contains(n))
			{
				Neighbours.Add(n);
				n.AddNeighbour(this);
			}
		}

		public int CompareTo(Node other)
		{
			return this.DistanceWeight.CompareTo(other.DistanceWeight);
		}

		public double DistanceTo(Node n)
		{
			return Position.DistanceTo(n.Position);
		}
	}


	[DebuggerDisplay("{ToString()}")]
	public class Vector3D
	{
		public float X { get; private set; }
		public float Y { get; private set; }
		public float Z { get; private set; }

		public Vector3D() { X = 0; Y = 0; Z = 0; }
		public Vector3D(Vector3D v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
		}
		public Vector3D(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public override string ToString()
		{
			return $"{X},{Y},{Z}";
		}

		public double DistanceTo(Vector3D v)
		{
			return Math.Sqrt((v.X - X) * (v.X - X) + (v.Y - Y) * (v.Y - Y) + (v.Z - Z) * (v.Z - Z));

		}

		public static Vector3D operator-(Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		public static Vector3D operator +(Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		public static Vector3D operator *(Vector3D v1, int i)
		{
			return new Vector3D(v1.X * i, v1.Y * i, v1.Z * i);
		}

		public static Vector3D operator *(Vector3D v1, double d)
		{
			return new Vector3D(Convert.ToSingle(v1.X * d), Convert.ToSingle(v1.Y * d), Convert.ToSingle(v1.Z * d));
		}

		public static Vector3D operator *(int i, Vector3D v1)
		{
			return new Vector3D(v1.X * i, v1.Y * i, v1.Z * i);
		}

		public static Vector3D operator *(double d,Vector3D v1)
		{
			return new Vector3D(Convert.ToSingle(v1.X * d), Convert.ToSingle(v1.Y * d), Convert.ToSingle(v1.Z * d));
		}

		public static Vector3D operator /(Vector3D v1, int i)
		{
			return new Vector3D(v1.X / i, v1.Y / i, v1.Z / i);
		}

		public static Vector3D operator /(Vector3D v1, double d)
		{
			return new Vector3D(Convert.ToSingle(v1.X / d), Convert.ToSingle(v1.Y / d), Convert.ToSingle(v1.Z / d));
		}

		public static double DotProduct(Vector3D v1, Vector3D v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}

		public double LengthSquared { get
			{
				return X * X + Y * Y + Z * Z;
			}
		}
	}

	[DebuggerDisplay("{Position.ToString()},Black = {Black}")]
	public class Node3D : IComparable<Node3D>
	{
		public bool Black { get; set; }
		public Node3D Parent;
		public Vector3D Position;
		public Node3D[] Neigh6 { get; private set; }
		public List<Node3D> Neigh26 { get; private set; }
		public int Weight { get { return Black ? 1 : -1; } }
		public double Distance;
		public double DistanceWeight { get { return Distance * Weight; } }
		public bool Visited;

		#region Palagy
		public bool AddedToCandidate;
		public bool IsSurrounded
		{
			get
			{
				return Neigh26.Count(x => x.Black) == 26;
			}
		}
		private bool? _IsEndPoint;
		public bool IsEndPoint
		{
			set
			{
				_IsEndPoint = value;
			}
			get
			{
				if (_IsEndPoint == null)
					//Si tiene un solo negro en N26
					return Neigh26.Count(x => x.Black) == 1;
				else return _IsEndPoint.Value;
			}
		}
		public bool IsBorde(Direction d)
		{
			int num = d.GetValue();
			if (Neigh6[num] != null)
				return !Neigh6[num].Black;
			else return false;
		}
		public bool IsSimple
		{
			get
			{
				//Task<bool> Cond2 = Task.Run(() => { return ConditionV2(); });
				//Task<bool> Cond4 = Task.Run(() => { return ConditionV4(); });
				//Cond2.Wait();
				//Cond4.Wait();
				bool res = false;
				if (ConditionV2())
					if (ConditionV4())
						res = true;
				return res;

			}
		}

        public bool IsSimple2
        {
            get
            {
                //Task<bool> Cond2 = Task.Run(() => { return ConditionV2(); });
                //Task<bool> Cond4 = Task.Run(() => { return ConditionV4(); });
                //Cond2.Wait();
                //Cond4.Wait();
                bool res = false;
                if (ConditionV2())
                    if (ConditionV4_version2())
                        res = true;
                return res;

            }
        }

        //static bool IS_COND2_SATISFIED(int[] NP, int[,,] M)
        //{
        //	//NP: Vecindad de un punto p;
        //	bool es = false;

        //	//M: recibe (para no pedir en cada momemento) la Matriz de vecindad de p:
        //	//5x5x5 en lugar de 3x3x3 para evitar problemas de contorno:

        //	for (int i = 0; i < 5; i++)
        //		for (int j = 0; j < 5; j++)
        //			for (int kk = 0; kk < 5; kk++)
        //				M[i, j, kk] = 0;

        //	//Llenado de la Matriz Vecindad de p, sin p
        //	int k = 0;

        //	int[] c = new int[3]; //Puntos semillas para ver conectividad:

        //	for (int z = 1; z < 4; z++)
        //		for (int x = 1; x < 4; x++)
        //			for (int y = 1; y < 4; y++)
        //			{
        //				if (!(x == 2 && y == 2 && z == 2)) //Si no estoy en el medio, o sea, sobre el punto p
        //				{
        //					if (NP[k] == 1)
        //					{
        //						M[x, y, z] = 1;
        //						c[0] = x; c[1] = y; c[2] = z; //Búsqueda del punto semilla "c"
        //					}
        //					k++;
        //				}
        //			}

        //	List<int[]> CP = new List<int[]>(); //connected points
        //	CP.Add(c);

        //	List<int[]> AC = new List<int[]>();

        //	M[c[0], c[1], c[2]] = 0; //Eliminacion en M del punto semilla "c"

        //	int[] np;
        //	int[] ap;

        //	List<int[]> NewP = new List<int[]>(); //array de coordenadas de los puntos vecinos

        //	int[] NP2 = new int[26]; //almacena la vecindad dentro de los puntos NP

        //	while (CP.Count != 0)
        //	{
        //		c = CP[0];

        //		//collect26neigh(c, M, NP2);

        //		//COLLECT_26NEIGH_COORDS(NP2, c, NewP);

        //		for (int j = 0; j < NewP.Count; j++)
        //		{
        //			ap = NewP[j];
        //			np = new int[3];
        //			np[0] = ap[0]; np[1] = ap[1]; np[2] = ap[2];
        //			CP.Add(np);
        //			M[ap[0], ap[1], ap[2]] = 0;
        //		}

        //		//np = new Punto;
        //		//np->x= c->x; np->y = c->y; np->z = c->z;
        //		//AC->Add(np);
        //		AC.Add(c);

        //		CP.RemoveAt(0);

        //		NewP.Clear();

        //	}//end while...

        //	if (AC.Count == NP.Sum())
        //		es = true;

        //	//Borrar elementos auxiliares:
        //	NewP.Clear();
        //	CP.Clear();
        //	AC.Clear();

        //	return es;
        //}

        //static bool IS_COND4_SATISFIED(int[] Np, int[,,] M)
        //{
        //	//Input: Np: Vecindad de un punto p;
        //	bool es = false;

        //	//M: recibe (para no pedir en cada momemento) la Matriz de vecindad de p:
        //	//5x5x5 en lugar de 3x3x3 para evitar problemas de contorno:
        //	for (int i = 0; i < 5; i++)
        //		for (int j = 0; j < 5; j++)
        //			for (int k = 0; k < 5; k++)
        //				M[i, j, k] = 0;

        //	//Creacion de la Matriz Vecindad de p

        //	//INVIERTO LA VECINDAD PARA TRABAJAR CON LOS MISMOS ALGORITMOS QUE BUSCAN
        //	//CONECTIVIDAD ENTRE VECINOS NEGROS
        //	for (int i = 0; i < 26; i++)
        //	{
        //		if (Np[i] == 1)
        //		{
        //			Np[i] = 0;
        //			continue;
        //		}

        //		if (Np[i] == 0)
        //		{
        //			Np[i] = 1;
        //			continue;
        //		}

        //	}
        //	//IGUALO A 0 TODOS LOS CONRNER POINTS, CONVIRTIENDO LA 26-VECINDAD EN 18-VECINDAD
        //	M[1, 1, 1] = 0;
        //	M[1, 2, 1] = Np[1];
        //	M[1, 3, 1] = 0;
        //	M[2, 1, 1] = Np[3];
        //	M[2, 2, 1] = Np[4]; //U
        //	M[2, 3, 1] = Np[5];
        //	M[3, 1, 1] = 0;
        //	M[3, 2, 1] = Np[7];
        //	M[3, 3, 1] = 0;

        //	M[1, 1, 2] = Np[9];
        //	M[1, 2, 2] = Np[10];//%N
        //	M[1, 3, 2] = Np[11];
        //	M[2, 1, 2] = Np[12];//%W
        //	M[2, 2, 2] = 0;
        //	M[2, 3, 2] = Np[13];//%E
        //	M[3, 1, 2] = Np[14];
        //	M[3, 2, 2] = Np[15];//%S
        //	M[3, 3, 2] = Np[16];

        //	M[1, 1, 3] = 0;
        //	M[1, 2, 3] = Np[18];
        //	M[1, 3, 3] = 0;
        //	M[2, 1, 3] = Np[20];
        //	M[2, 2, 3] = Np[21];//%D
        //	M[2, 3, 3] = Np[22];
        //	M[3, 1, 3] = 0;
        //	M[3, 2, 3] = Np[24];
        //	M[3, 3, 3] = 0;

        //	int UpFlag = 0, NorthFlag = 0, WestFlag = 0, EastFlag = 0, SouthFlag = 0, DownFlag = 0;

        //	int[] c = new int[3];
        //	c = null;

        //	//Encuentro cualquiera de los puntos U,D,N,S,E,W para usar como semilla de un
        //	//crecimiento 6-connected:

        //	if (M[2, 2, 1] == 1)
        //	{
        //		UpFlag = 1;
        //		c = new int[3];
        //		c[0] = 2; c[1] = 2; c[2] = 1;
        //	}

        //	if (M[1, 2, 2] == 1)
        //	{
        //		NorthFlag = 1;
        //		c = new int[3];
        //		c[0] = 1; c[1] = 2; c[2] = 2;
        //	}

        //	if (M[2, 1, 2] == 1)
        //	{
        //		WestFlag = 1;
        //		c = new int[3];
        //		c[0] = 2; c[1] = 1; c[2] = 2;
        //	}

        //	if (M[2, 3, 2] == 1)
        //	{
        //		EastFlag = 1;
        //		c = new int[3];
        //		c[0] = 2; c[1] = 3; c[2] = 2;
        //	}

        //	if (M[3, 2, 2] == 1)
        //	{
        //		SouthFlag = 1;
        //		c = new int[3];
        //		c[0] = 3; c[1] = 2; c[2] = 2;
        //	}

        //	if (M[2, 2, 3] == 1)
        //	{
        //		DownFlag = 1;
        //		c = new int[3];
        //		c[0] = 2; c[1] = 2; c[2] = 3;
        //	}

        //	int cant_borders = UpFlag + EastFlag + WestFlag + NorthFlag + SouthFlag + DownFlag;

        //	if (c == null)
        //		return es;

        //	List<int[]> CP = new List<int[]>();
        //	CP.Add(c);

        //	List<int[]> AC = new List<int[]>();

        //	List<int[]> NewP = new List<int[]>();

        //	M[c[0], c[1], c[2]] = 0;

        //	int[] NP = new int[26];

        //	int[] ap;
        //	int[] np;

        //	while (CP.Count != 0)
        //	{
        //		ap = CP[0];
        //		//COLLECT_6_NEIGH2 y
        //		//COLLECT_6_COORDS para asegurarnos que estamos tomando vecinos 6
        //		//conectados.

        //		//collect26neigh(ap, M, NP);
        //		//COLLECT_6NEIGH_COORDS(NP, ap, NewP);

        //		for (int j = 0; j < NewP.Count; j++)
        //		{
        //			ap = NewP[j];
        //			M[ap[0], ap[1], ap[2]] = 0;
        //		}

        //		for (int j = 0; j < NewP.Count; j++)
        //		{
        //			ap = NewP[j];
        //			np = new int[3];
        //			np[0] = ap[0]; np[1] = ap[1]; np[2] = ap[2];
        //			CP.Add(np);
        //		}
        //		ap = CP[0];
        //		//np = new Punto;
        //		//np->x=ap->x;np->y=ap->y; np->z=ap->z;
        //		//AC->Add(np);
        //		AC.Add(ap);
        //		CP.RemoveAt(0);

        //		NewP.Clear();
        //	}

        //	//Limpieza de la matriz auxiliar
        //	for (int i = 0; i < 5; i++)
        //		for (int j = 0; j < 5; j++)
        //			for (int k = 0; k < 5; k++)
        //				M[i, j, k] = 0;


        //	for (int i = 0; i < AC.Count; i++)
        //	{
        //		ap = AC[i];
        //		M[ap[0], ap[1], ap[2]] = 1;
        //	}

        //	int suma_border = M[2, 2, 1] + M[1, 2, 2] + M[2, 1, 2] + M[2, 3, 2] + M[3, 2, 2] + M[2, 2, 3];

        //	if (suma_border == cant_borders)
        //		es = true;

        //	CP.Clear();
        //	AC.Clear();
        //	NewP.Clear();

        //	return es;
        //}

        /// <summary>
        /// Revisa si al sacar el punto p separa estructuras negras en N26 (Slow)
        /// </summary>
        /// <returns></returns>
        public bool Condition2()
		{

			int[,,] map = new int[3, 3, 3];
			Vector3D StartSeed = null;
			int l = 0;
			for (int k = 0; k < 3; k++)
				for (int j = 0; j < 3; j++)
					for (int i = 0; i < 3; i++)
					{
						map[i, j, k] = 0;
						if (!(i == 1 && j == 1 && k == 1))
						{
							map[i, j, k] = Neigh26[l].Black ? 1 : 0;
							if (map[i, j, k] == 1)
							{
								if (StartSeed == null)
									StartSeed = new Vector3D(i, j, k);
							}
							l++;
						}
					}
			Grid3D grid = new Grid3D(map);

			int x = 0;
			int y = 0;
			int z = 0;
			bool solvable = true;
			for (int i = 0; i < 28; i++)
			{
				if (map[x % 3, y % 3, z % 3] == 1)
					solvable = DjikstraSolver.SolveGridUnweighted(grid, StartSeed, new Vector3D(x % 3, y % 3, z % 3));
				if (!solvable)
					break;

				//y=i%3
				if (x == 2 && y == 2)
				{
					z++;
					x = 0;
					y = x;
				}
				if (x == 2)
				{
					y++;
				}
				x = i % 3;
				grid.Reset();
			}

			return solvable;
		}

		/// <summary>
		/// Revisa si al sacar el punto p separa estructuras negras en N26 (Faster)
		/// </summary>
		/// <returns></returns>
		public bool ConditionV2()
		{

			int[,,] map = new int[3, 3, 3];
			Vector3D StartSeed = null;
			int l = 0;
			for (int k = 0; k < 3; k++)
				for (int j = 0; j < 3; j++)
					for (int i = 0; i < 3; i++)
					{
						map[i, j, k] = 0;
						if (!(i == 1 && j == 1 && k == 1))
						{
							map[i, j, k] = Neigh26[l].Black ? 1 : 0;
							if (map[i, j, k] == 1)
							{
								if (StartSeed == null)
									StartSeed = new Vector3D(i, j, k);
							}
							l++;
						}
					}
			Grid3D grid = new Grid3D(map);
			Queue<Node3D> Candidates = new Queue<Node3D>();
			Candidates.Enqueue(grid[StartSeed]);

			var nodes = grid.grid.Cast<Node3D>().Where(x => x.Black).ToList();
			List<Node3D> VisitedNodes = new List<Node3D>();
			VisitedNodes.Add(grid[StartSeed]);

			//Region growth
			while (Candidates.Count > 0)
			{
				if (nodes.Count == VisitedNodes.Count)
					break;
				var node = Candidates.Dequeue();
				node.Visited = true;
				var nodesNotNull = node.Neigh6.Where(x => x != null).ToList();
				foreach (var n in nodesNotNull.Where(x => x.Black && !x.Visited))
				{
					n.Visited = true;
					VisitedNodes.Add(n);
					Candidates.Enqueue(n);
				}
			}
			//Si las cantidades no son iguales significa que hubo alguno sin visitar
			return nodes.Count == VisitedNodes.Count;
		}

		/// <summary>
		/// Revisa si al sacar el punto p une cavidades blancas en N6 (Faster)
		/// </summary>
		/// <returns></returns>
		public bool ConditionV4()
		{

			int[,,] map = new int[3, 3, 3];
			Vector3D StartSeed = null;
            
			int l = 0;
			for (int k = 0; k < 3; k++)
				for (int j = 0; j < 3; j++)
					for (int i = 0; i < 3; i++)
					{
                        bool corner = false;
                        if (i == 0 && j == 0 && k == 0)
                            corner = true;
                        if (i == 0 && j == 0 && k == 2)
                            corner = true;
                        if (i == 0 && j == 2 && k == 0)
                            corner = true;
                        if (i == 0 && j == 2 && k == 2)
                            corner = true;
                        if (i == 2 && j == 0 && k == 0)
                            corner = true;
                        if (i == 2 && j == 0 && k == 2)
                            corner = true;
                        if (i == 2 && j == 2 && k == 0)
                            corner = true;
                        if (i == 2 && j == 2 && k == 2)
                            corner = true;
                        map[i, j, k] = 0;
						if (!(i == 1 && j == 1 && k == 1))
                        {
                            if (!corner)
                            {
                            map[i, j, k] = Neigh26[l].Black ? 0 : 1;
                            if (map[i, j, k] == 1 && StartSeed == null)
                            {
                                //la semilla no puede ser una esquina (Solo acepta vecinos 18)
                                if (i == 0 && j == 0 && k == 0)
                                    corner = true;
                                if (i == 0 && j == 0 && k == 2)
                                    corner = true;
                                if (i == 0 && j == 2 && k == 0)
                                    corner = true;
                                if (i == 0 && j == 2 && k == 2)
                                    corner = true;
                                if (i == 2 && j == 0 && k == 0)
                                    corner = true;
                                if (i == 2 && j == 0 && k == 2)
                                    corner = true;
                                if (i == 2 && j == 2 && k == 0)
                                    corner = true;
                                if (i == 2 && j == 2 && k == 2)
                                    corner = true;
                                if (!corner)
                                    StartSeed = new Vector3D(i, j, k);
                            }

                            }
                            l++;
                        }
                    }

			Grid3D grid = new Grid3D(map);
			Queue<Node3D> Candidates = new Queue<Node3D>();
			Candidates.Enqueue(grid[StartSeed]);
            //Nodos negros
			var nodes = grid.grid.Cast<Node3D>().Where(x => x.Black).ToList();
			List<Node3D> VisitedNodes = new List<Node3D>();
			VisitedNodes.Add(grid[StartSeed]);
			//Region Growth
			while (Candidates.Count > 0)
			{
				if (nodes.Count == VisitedNodes.Count)
					break;
				var node = Candidates.Dequeue();
				node.Visited = true;
                //Busco nodos que no sean null (caso extremo que sea una pared)
				var nodesNotNull = node.Neigh6.Where(x => x != null).ToList();
                //Por  cada nodo que no sea null, sea negro y no este visitado, meterlo en visitados
				foreach (var n in nodesNotNull.Where(x => x.Black && !x.Visited))
				{
					n.Visited = true;
					VisitedNodes.Add(n);
					Candidates.Enqueue(n);
				}
			}
			//Si las cantidades no son iguales significa que hubo alguno sin visitar
			return nodes.Count == VisitedNodes.Count;
		}

        /// <summary>
		/// Revisa si al sacar el punto p une cavidades blancas en N6 (Faster)
		/// </summary>
		/// <returns></returns>
		public bool ConditionV4_version2()
        {

            int[,,] map = new int[3, 3, 3];
            Vector3D StartSeed = null;

            int l = 0;
            for (int k = 0; k < 3; k++)
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < 3; i++)
                    {
                        bool corner = false;
                        map[i, j, k] = 0;
                        if (!(i == 1 && j == 1 && k == 1))
                        {
                                map[i, j, k] = Neigh26[l].Black ? 0 : 1;
                                if (map[i, j, k] == 1 && StartSeed == null)
                                {
                                    //la semilla no puede ser una esquina (Solo acepta vecinos 18)
                                    if (i == 0 && j == 0 && k == 0)
                                        corner = true;
                                    if (i == 0 && j == 0 && k == 2)
                                        corner = true;
                                    if (i == 0 && j == 2 && k == 0)
                                        corner = true;
                                    if (i == 0 && j == 2 && k == 2)
                                        corner = true;
                                    if (i == 2 && j == 0 && k == 0)
                                        corner = true;
                                    if (i == 2 && j == 0 && k == 2)
                                        corner = true;
                                    if (i == 2 && j == 2 && k == 0)
                                        corner = true;
                                    if (i == 2 && j == 2 && k == 2)
                                        corner = true;
                                    if (!corner)
                                        StartSeed = new Vector3D(i, j, k);
                                }
                            l++;
                        }
                    }

            Grid3D grid = new Grid3D(map);
            Queue<Node3D> Candidates = new Queue<Node3D>();
            Candidates.Enqueue(grid[StartSeed]);
            //Nodos negros
            var nodes = grid.grid.Cast<Node3D>().Where(x => x.Black).ToList();
            List<Node3D> VisitedNodes = new List<Node3D>();
            VisitedNodes.Add(grid[StartSeed]);
            //Region Growth
            while (Candidates.Count > 0)
            {
                if (nodes.Count == VisitedNodes.Count)
                    break;
                var node = Candidates.Dequeue();
                node.Visited = true;
                //Busco nodos que no sean null (caso extremo que sea una pared)
                var nodesNotNull = node.Neigh6.Where(x => x != null).ToList();
                //Por  cada nodo que no sea null, sea negro y no este visitado, meterlo en visitados
                foreach (var n in nodesNotNull.Where(x => x.Black && !x.Visited))
                {
                    n.Visited = true;
                    VisitedNodes.Add(n);
                    Candidates.Enqueue(n);
                }
            }
            //Si las cantidades no son iguales significa que hubo alguno sin visitar
            return nodes.Count == VisitedNodes.Count;
        }

        /// <summary>
        /// Revisa si al sacar el punto p une cavidades blancas en N6 (Slow)
        /// </summary>
        /// <returns></returns>
        private bool Condition4()
		{
			int[,,] map = new int[3, 3, 3];
			Vector3D StartSeed = null;
			int l = 0;

			for (int k = 0; k < 3; k++)
				for (int j = 0; j < 3; j++)
					for (int i = 0; i < 3; i++)
					{
						map[i, j, k] = 0;
						if (!(i == 1 && j == 1 && k == 1))
						{
							map[i, j, k] = Neigh26[l].Black ? 0 : 1;
							if (map[i, j, k] == 1 && StartSeed == null)
								StartSeed = new Vector3D(i, j, k);
							l++;
						}
					}

			Grid3D grid = new Grid3D(map);

			int x = 0;
			int y = 0;
			int z = 0;
			bool solvable = true;
			for (int i = 0; i < 28; i++)
			{
				if (map[x, y, z] == 1)
					solvable = DjikstraSolver.SolveGridUnweightedNeigh6(grid, StartSeed, new Vector3D(x, y, z));
				if (!solvable)
					break;
				if (x == 2 && y == 2)
				{
					z++;
					x = 0;
					y = x;
				}
				if (x == 2)
				{
					y++;
				}
				x = i % 3;


				grid.Reset();
			}
			return solvable;
		}


		#endregion

		public Node3D(Vector3D pos, bool black = false, bool? endPoint = null)
		{
			Parent = null;
			Distance = 50000;
			this.Black = black;
			this.Position = pos;
			Neigh6 = new Node3D[6];
			//Neigh6 = new Dictionary<Direction, Node>(6);
			//Neigh18 = new List<Node>(18);
			Neigh26 = new List<Node3D>(26);
			Visited = false;
			_IsEndPoint = endPoint;
			AddedToCandidate = false;
		}

		/// <summary>
		/// Adds a neighbour to the node N6
		/// </summary>
		/// <param name="n">Node</param>
		public void AddNeighbour6(Node3D n, Direction dir, int index)
		{
			if (Neigh6.Count(x => x != null) < 6)
			{
				this.Neigh6[dir.GetValue()] = n;
			}
			if (Neigh26.Count < 26)
				this.Neigh26.Add(n);
			// if (Neigh18.Count < 18)
			//     this.Neigh18.Add(n);
		}

		/// <summary>
		/// Adds a neighbour to the node N18
		/// </summary>
		/// <param name="n">Node</param>
		public void AddNeighbour18(Node3D n, int index)
		{
			if (Neigh26.Count < 26)
				this.Neigh26.Add(n);
			//if (Neigh18.Count < 18)
			//    this.Neigh18.Add(n);
		}

		/// <summary>
		/// Adds a neighbour to the node N26
		/// </summary>
		/// <param name="n">Node</param>
		public void AddNeighbour26(Node3D n, int index)
		{
			if (Neigh26.Count < 26)
				this.Neigh26.Add(n);
		}
		public int CompareTo(Node3D other)
		{
			return this.DistanceWeight.CompareTo(other.DistanceWeight);
		}

		public double DistanceTo(Node3D n)
		{
			return Position.DistanceTo(n.Position);
		}


	}
}
