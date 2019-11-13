using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SplineOpenTK
{
	public class ListRender : IRenderObject
	{
		private float thethaRotation=0;
		private List<Vector3D> points;
		private float RotationCoeficient = 0.5f;
		private bool Rotate;

		public ListRender(List<Vector3D> points, bool rotate = false)
		{
			this.points = points;
			Rotate = rotate;
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
			
			GL.Begin(BeginMode.LineStrip);
			//GL.Color4(255, 0, 0, 255);
			GL.Color3(1.0, 0, 0);
			foreach (var point in points)
			{
				GL.Vertex3(point.ToOpenTKVector());
			}
			GL.Color3(1.0, 1.0, 1.0);
			GL.End();
			//GL.Enable(EnableCap.Lighting);
			GL.PopMatrix();
		}
	}
}
