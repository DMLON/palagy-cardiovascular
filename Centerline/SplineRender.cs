using OpenTK.Graphics.OpenGL;
using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centerline
{
	public class SplineRender : Spline, IRenderObject
	{
		private float thethaRotation = 0;
		private float RotationCoeficient = 0.5f;
		private bool Rotate;

		public SplineRender(Spline s, bool rotate = false)
		{
			this.TG = s.TG;
			this.Coeficientes = s.Coeficientes;
			this.AnchorPoints = s.AnchorPoints;
			this.Rotate = rotate;
		}
		public SplineRender(List<Vector3D> points, bool rotate = false) : base(points)
		{
			this.Rotate = rotate;
		}

		public SplineRender(bool rotate = false) :base()
		{
			this.Rotate = rotate;
		}

		public void IncreaseSpeed()
		{
			RotationCoeficient += 0.1f;
		}
		public void DecreaseSpeed()
		{
			RotationCoeficient -= 0.1f;
		}

		public void Draw()
		{
			//GL.Disable(EnableCap.Lighting);
			thethaRotation += RotationCoeficient;
			GL.LineWidth(1.5f);
			GL.PushMatrix();
			//GL.Color3(255, 0, 0);
			GL.LoadIdentity();
			if(Rotate)
				GL.Rotate(thethaRotation, 0, 1, 0);

			GL.Begin(BeginMode.LineStrip);
			for (int i = 0; i < 100; i++)
			{
				GL.Vertex3(EvaluateAt((float)i / 100).ToOpenTKVector());
			}
			GL.End();
			//GL.Enable(EnableCap.Lighting);
			GL.PopMatrix();
		}
	}
}
