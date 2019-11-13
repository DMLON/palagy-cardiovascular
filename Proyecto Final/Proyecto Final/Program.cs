using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Proyecto_Final
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			SplineRender splineTest = new SplineRender(true);
			splineTest.AddPoint(new Vector3D(0,-4, 0));
			splineTest.AddPoint(new Vector3D(1,-2,0));
			//splineTest.AddPoint(new Vector3D(2,2,2));2 r  
			splineTest.AddPoint(new Vector3D(0,0,-1));
			splineTest.AddPoint(new Vector3D(-1, 2, 0));
			splineTest.AddPoint(new Vector3D(0, 4, 1));


			//splineTest.AddPoint(new Vector3D(3,0,2));
			//splineTest.AddPoint(new Vector3D(5, -2, 2));
			//splineTest.AddPoint(new Vector3D(2, -1, 5));
			VentanaOpenGL ventana = new VentanaOpenGL();
			ventana.AddObject(splineTest);
			ventana.AddObject(splineTest.ProyectedCPR(new Plane(new Vector3D(1, 0, 0), new Vector3D(4, 0, 0))));
			ventana.AddObject(splineTest.NormalPlaneAt(0.05f));
			//ventana.AddObject(splineTest2);
			ventana.Start();
			


		}
	}
}
