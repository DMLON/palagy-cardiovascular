using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SplineOpenTK
{
	public static class ExtensionMethods
	{
		public static float DistanceFrom(this Vector3D vector, Vector3D vectorDestino)
		{
			return Convert.ToSingle((vector - vectorDestino).Length);
		}

		public static Vector3 ToOpenTKVector(this Vector3D vector)
		{
			return new Vector3(Convert.ToSingle(vector.X), Convert.ToSingle(vector.Y), Convert.ToSingle(vector.Z));
		}

		public static List<Vector3D> ToWindowsMediaVector(this List<Solver.Vector3D> vector)
		{
			List<Vector3D> o = new List<Vector3D>();
			foreach (var vec in vector)
				o.Add(new Vector3D(vec.X, vec.Y, vec.Z));
			return o;
		}
	}
}
