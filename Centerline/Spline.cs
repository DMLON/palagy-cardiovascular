using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Solver;
namespace Centerline
{
	//Centripetal Catmull–Rom spline
	public class Spline
	{
		public List<Vector3D> AnchorPoints { get; protected set; }
		/// <summary>
		/// Cada array tiene 4 elementos
		/// </summary>
		public List<Vector3D[]> Coeficientes { get; protected set; }
		/// <summary>
		/// Valor de spline [0 1]
		/// </summary>
		public float[] TG { get; protected set; }

		public Spline()
		{
			AnchorPoints = new List<Vector3D>();
			Coeficientes = new List<Vector3D[]>();
		}

		public Spline(List<Vector3D> points)
		{
			AnchorPoints = points;
			Coeficientes = new List<Vector3D[]>();
			Update();
		}

		public Vector3D EvaluateTangentAt(float tS)
		{
			//Returns the sampled_point resulting from sampling the spline in the sampling parameter value tS:
			int i;
			float t;
			Vector3D sampled_point = new Vector3D();

			//Quality control check: 0 <= tS <= 1
			if (tS > 1.0f)
				tS = 1.0f;
			if (tS < 0.0f)
				tS = 0.0f;

			//Search for the correct polynomial: 
			for (i = 1; i < AnchorPoints.Count; ++i)
			{
				if (tS <= TG[i])
				{
					//Get the proportion of the natural parameter for the spline polynomial (0 <= t <= 1) in that segment:
					t = (tS - TG[i - 1]) / (TG[i] - TG[i - 1]);

					//Choose the correct polynomial and evaluate it at t:
					var sampling_coefs = Coeficientes[i - 1];
					sampled_point = new Vector3D(
						sampling_coefs[1].X + sampling_coefs[2].X * 2 * t + sampling_coefs[3].X * 3 * t * t,
						sampling_coefs[1].Y + sampling_coefs[2].Y * 2 * t + sampling_coefs[3].Y * 3 * t * t,
						sampling_coefs[1].Z + sampling_coefs[2].Z * 2 * t + sampling_coefs[3].Z * 3 * t * t);
					break;
				}
			}

			return sampled_point;
		}

		/// <summary>
		/// Gets the point at given tS value
		/// </summary>
		/// <param name="tS">Value of spline [0 1]</param>
		/// <returns>Point at tS</returns>
		public Vector3D EvaluateAt(float tS)
		{
			//Returns the sampled_point resulting from sampling the spline in the sampling parameter value tS:
			int i;
			float t;
			Vector3D sampled_point = new Vector3D();

			//Quality control check: 0 <= tS <= 1
			if (tS > 1.0f)
				tS = 1.0f;
			if (tS < 0.0f)
				tS = 0.0f;

			//Search for the correct polynomial: 
			for (i = 1; i < AnchorPoints.Count; ++i)
			{
				if (tS <= TG[i])
				{
					//Get the proportion of the natural parameter for the spline polynomial (0 <= t <= 1) in that segment:
					t = (tS - TG[i - 1]) / (TG[i] - TG[i - 1]);

					//Choose the correct polynomial and evaluate it at t:
					var sampling_coefs = Coeficientes[i - 1];
					sampled_point = new Vector3D(
						sampling_coefs[0].X + sampling_coefs[1].X * t + sampling_coefs[2].X * t * t + sampling_coefs[3].X * t * t * t,
						sampling_coefs[0].Y + sampling_coefs[1].Y * t + sampling_coefs[2].Y * t * t + sampling_coefs[3].Y * t * t * t,
						sampling_coefs[0].Z + sampling_coefs[1].Z * t + sampling_coefs[2].Z * t * t + sampling_coefs[3].Z * t * t * t);
					break;
				}
			}

			return sampled_point;
		}
		//Hay un problema con el calculo de los coeficientes o de Evaluate At
		/// <summary>
		/// Adds an anchoring point to the spline
		/// </summary>
		/// <param name="punto"></param>
		public void AddPoint(Vector3D punto)
		{
			AnchorPoints.Add(punto);
			Update();
		}

		/// <summary>
		/// Computes coeficients for all points after adding a point
		/// </summary>
		public void Update()
		{
			ComputeAllCoeficients();
			ComputeInternalSplineParameter();
		}

		#region Coeficientes
		/// <summary>
		/// Computes Coeficients for all points
		/// </summary>
		private void ComputeAllCoeficients()
		{
			switch (AnchorPoints.Count)
			{
				case 1://Nada
					Coeficientes.Add(new Vector3D[4]);
					for (int i = 0; i < 4; i++)
						Coeficientes.Last()[i] = new Vector3D(0, 0, 0);
					break;
				case 2://Linea
					Coeficientes.Clear();
					Coeficientes.Add(new Vector3D[4]);
					for (int i = 0; i < 2; i++)
						Coeficientes.Last()[i] = new Vector3D(AnchorPoints[i].X, AnchorPoints[i].Y, AnchorPoints[i].Z);
					for (int i = 2; i < 4; i++)
						Coeficientes.Last()[i] = new Vector3D(0, 0, 0);
					break;
				default://Mayor a 2
						//Compute First Coef (0,1,2)
					Coeficientes.Clear();
					ComputeFirstCoeficients();
					//Compute rest of Coef (1,N-2)
					for (int i = 1; i < AnchorPoints.Count - 2; ++i)
					{
						ComputeCoeficientsForAnchors(AnchorPoints[i - 1], AnchorPoints[i], AnchorPoints[i + 1], AnchorPoints[i + 2]);
					}
					//Compute Last Coef (N-3,N-2,N-1)
					ComputeLastCoeficients();
					break;
			}
		}

		private void ComputeFirstCoeficients()
		{
			var p1 = AnchorPoints[0]; //First point: p[0]
			var p2 = AnchorPoints[1];
			var p3 = AnchorPoints[2];
			// Virtual point is the mirror of p3 reflected on the plane whose normal is p2 - p1 and passes by the midpoint of p2 - p1:
			Vector3D normal = p2 - p1; // Normal of the plane: p2 - p1
			double k = 0.5 * Vector3D.DotProduct(normal, p2 + p1); // Orthogonal distance from the origin to the plane, such that: normal * p1_2 = k
																   // A line is defined between p3 and its reflection (the virtual point).
																   // This line is orthogonal to the plane and its equation is: X(t) = p3 + t * normal
																   // t0 is the value of the parameter t for which this line intersects the plane
			double t0 = (k - Vector3D.DotProduct(normal, p3)) / normal.LengthSquared;
			Vector3D M = p3 + normal * t0; // Midpoint between p3 and p0. It corresponds to X(t0) = p3 + t0 * normal
										   // Finally, the virtual point is calculated such that M is the average between p3 and the virtual point:
			var virtual_point = M * 2 - p3;
			ComputeCoeficientsForAnchors(virtual_point, p1, p2, p3);
		}

		private void ComputeLastCoeficients()
		{
			var p0 = AnchorPoints[AnchorPoints.Count - 3]; //First point: p[0]
			var p1 = AnchorPoints[AnchorPoints.Count - 2];
			var p2 = AnchorPoints[AnchorPoints.Count - 1];
			// Virtual point is the mirror of p3 reflected on the plane whose normal is p2 - p1 and passes by the midpoint of p2 - p1:
			Vector3D normal = p1 - p2; // Normal of the plane: p2 - p1
			double k = 0.5 * Vector3D.DotProduct(normal, p2 + p1); // Orthogonal distance from the origin to the plane, such that: normal * p1_2 = k
																   // A line is defined between p3 and its reflection (the virtual point).
																   // This line is orthogonal to the plane and its equation is: X(t) = p3 + t * normal
																   // t0 is the value of the parameter t for which this line intersects the plane
			double t0 = (k - Vector3D.DotProduct(normal, p0)) / normal.LengthSquared;
			Vector3D M = p0 + normal * t0; // Midpoint between p3 and p0. It corresponds to X(t0) = p3 + t0 * normal
										   // Finally, the virtual point is calculated such that M is the average between p3 and the virtual point:
			var virtual_point = M * 2 - p0;
			ComputeCoeficientsForAnchors(p0, p1, p2, virtual_point);
		}

		/// <summary>
		/// Computes TG
		/// </summary>
		private void ComputeInternalSplineParameter()
		{
			//Error check: is points empty? 
			if (AnchorPoints.Count == 0)
				return;

			TG = new float[AnchorPoints.Count];

			//Internal parameter always starts in 0:
			TG[0] = 0;

			//First, manage the exceptional case of having only one point:
			if (AnchorPoints.Count == 1)
				return;

			//Segment by segment chordal length:
			for (int i = 1; i < AnchorPoints.Count; ++i)
				TG[i] = Convert.ToSingle(AnchorPoints[i].DistanceTo(AnchorPoints[i - 1]));

			//Accumulate and normalize global parameter:
			for (int i = 1; i < AnchorPoints.Count; ++i)
				TG[i] += TG[i - 1];

			for (int i = 0; i < AnchorPoints.Count; ++i)
				TG[i] /= TG[AnchorPoints.Count - 1];
		}

		/// <summary>
		/// Computes coeficientes of interpolating spline for the given anchors
		/// </summary>
		/// <param name="p0">Point [i-1]</param>
		/// <param name="p1">Point [i]</param>
		/// <param name="p2">Point [i+1]</param>
		/// <param name="p3">Point [i+2]</param>
		private void ComputeCoeficientsForAnchors(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3)
		{
			float d01 = Convert.ToSingle(p0.DistanceTo(p1));
			float d12 = Convert.ToSingle(p1.DistanceTo(p2));
			float d23 = Convert.ToSingle(p2.DistanceTo(p3));
			Vector3D T1 = (p1 - p0) / d01 - (p2 - p0) / (d01 + d12) + (p2 - p1) / d12;
			T1 = T1 * d12;
			Vector3D T2 = (p2 - p1) / d12 - (p3 - p1) / (d12 + d23) + (p3 - p2) / d23;
			T2 = T2 * d12;
			Coeficientes.Add(new Vector3D[4]);
			Coeficientes.Last()[0] = new Vector3D(p1.X, p1.Y, p1.Z);
			Coeficientes.Last()[1] = T1;
			Coeficientes.Last()[2] = -3 * p1 + 3 * p2 - 2 * T1 - T2;
			Coeficientes.Last()[3] = 2 * p1 - 2 * p2 + T1 + T2;
		}
		#endregion



		public void CopyToClipboard()
		{
			string points = "";
			for (int i = 0; i < 1000; i++)
				points += EvaluateAt((float)i / 1000).ToString() + "\n";
			Clipboard.Clear();
			Clipboard.SetText(points, TextDataFormat.Text);
		}

		public void ConvertToXYZ_file(string filename,int Samples=100,Vector3D Offset=null)
		{

			List<string> lines = new List<string>();
			if (Offset == null)
				Offset = new Vector3D(0, 0, 0);
			for (int i = 0; i < Samples; i++)
			{
				var vec = EvaluateAt((float)i / Samples);
				var imprimir = vec - Offset;
				string line = $"{imprimir.X} {imprimir.Y} {imprimir.Z}";
				lines.Add(line);
			}
			
			File.WriteAllLines(filename + ".xyz", lines.ToArray());

		}

	}
}
