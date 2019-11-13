using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class Vector2D
    {
        public int x;
        public int y;
        public Vector2D(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public double DistanceTo(Vector2D v)
        {
            return Math.Sqrt((v.x - x) * (v.x - x) + (v.y - y) * (v.y - y));
        }
        public double Length { get
            {
                return Math.Sqrt(x * x + y * y);
            } }

    }
}
