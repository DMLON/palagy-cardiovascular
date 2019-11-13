using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Proyecto_Final
{
	public class VentanaOpenGL
	{
		GameWindow window;
		List<IRenderObject> objects;
		public VentanaOpenGL()
		{
			window = new GameWindow(800, 800);
			objects = new List<IRenderObject>();

		}

		public void AddObject(IRenderObject obj)
		{
			objects.Add(obj);
		}

		public void Start()
		{
			window.Load += OnLoad;
			window.RenderFrame += OnRender;
			window.Resize += OnResize;
			window.KeyPress += OnKeyPress;
			window.MouseWheel += OnMouseWheelMove;

			window.Run();
		}
		float planePos = 0;

		void OnMouseWheelMove(object o, MouseWheelEventArgs e)
		{
			//Mueve plano con la ruedita
			planePos += (float)e.DeltaPrecise/100;
			objects.Remove(objects.Last());
			objects.Add(((SplineRender)(objects.First())).NormalPlaneAt(planePos));
		}

		void OnKeyPress(object o, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case '+':
					foreach (var item in objects)
						((SplineRender)item).IncreaseSpeed();
					break;
				case '-':
					foreach (var item in objects)
						((SplineRender)item).DecreaseSpeed();
					break;
				default:
					break;
			
			}
		}

		void OnResize(object o,EventArgs e)
		{
			GL.Viewport(0, 0, window.Width, window.Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-5, 5, -5, 10, -10, 10);
			GL.MatrixMode(MatrixMode.Modelview);


		}

		void OnRender(object o, EventArgs e)
		{
			//GL.ClearColor(0f,0, 0, 0);
			//GL.Enable(EnableCap.DepthTest);
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.LoadIdentity();
			//GL.Ortho(0, 800, 0, 600, 0, 0);
			//GL.MatrixMode(MatrixMode.Modelview);
			
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-5, 5, -5, 5, -10, 10);
			GL.MatrixMode(MatrixMode.Modelview);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			//GL.Color4(255, 0, 0, 255);
			foreach (var item in objects)
			{
				item.Draw();
			}

			window.SwapBuffers();
		}

		void OnLoad(object o,EventArgs e)
		{
			GL.ClearColor(0, 0, 0, 0);
			GL.Disable(EnableCap.Lighting);
		}
	}
}
