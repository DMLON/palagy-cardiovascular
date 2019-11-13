using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SplineOpenTK
{
	public class Plane
	{
		private Vector3D _Normal { get; set; }
		public Vector3D Normal {
			get { return _Normal; }
			set
			{
				//Gramm-Schmidt procedure
				_Normal = value;
				var Normal_temp = new Vector3D(Normal.X, Normal.Y, Normal.Z);
				Normal_temp.Normalize();
				Director1 = new Vector3D(1, 0, 0);
				Director1 = Director1 - Vector3D.DotProduct(Director1, Normal_temp) * Normal_temp;
				Director1.Normalize();
				Director2 = Vector3D.CrossProduct(Director1, Normal_temp);
				Director2.Normalize();
			}
		}
		public Vector3D Director1 { get; private set; }
		public Vector3D Director2 { get; private set; }
		public Vector3D Origen { get; private set; }
		public Plane(Vector3D normal,Vector3D origen)
		{
			Normal = normal;
			Origen = origen;
		}
	}
}
