using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SplineOpenTK
{
	public class PlaneRender : Plane, IRenderObject
	{
		private bool Rotate;
		private float thethaRotation = 0;
		private float RotationCoeficient = 0.5f;
		private float Height;
		private float Width;
		private List<Vector3D> Boundaries;

		public PlaneRender(Vector3D normal, Vector3D origen,float Width,float Height, bool rotate = false) : base(normal, origen)
		{
			Rotate = rotate;
		}

		public PlaneRender(Plane plane, float Width, float Height, bool rotate = false) :base(plane.Normal,plane.Origen)
		{
			this.Height = Height;
			this.Width = Width;
			Boundaries = new List<Vector3D>();
			SetBoundaries();
			Rotate = rotate;
		}

		private void SetBoundaries()
		{
			var Hipotenusa = Math.Sqrt((Width / 2) * (Width / 2) + (Height / 2) * (Height / 2));
			var DirectorLinea1 = (Director1 + Director2);
			DirectorLinea1.Normalize();
			var DirectorLinea2 = (Director1 - Director2);
			DirectorLinea2.Normalize();

			Boundaries.Add(DirectorLinea1 * Hipotenusa + Origen);
			Boundaries.Add(DirectorLinea2 * Hipotenusa + Origen);
			Boundaries.Add(DirectorLinea1 * (-Hipotenusa) + Origen);
			Boundaries.Add(DirectorLinea2 * (-Hipotenusa) + Origen);

		}

		public void SetRotation(float rotThetha)
		{
			this.thethaRotation = rotThetha;
		}

		public void Draw()
		{
			//GL.Disable(EnableCap.Lighting);
			thethaRotation += RotationCoeficient;
			//GL.LineWidth(1.5f);
			GL.PushMatrix();
			//GL.Color3(255, 0, 0);
			GL.LoadIdentity();
			if (Rotate)
				GL.Rotate(thethaRotation, 0, 1, 0);

			GL.Begin(BeginMode.Quads);
			foreach(var bound in Boundaries)
			{
				GL.Vertex3(bound.ToOpenTKVector());
			}
			GL.End();
			//GL.Enable(EnableCap.Lighting);
			GL.PopMatrix();
		}
	}
}
