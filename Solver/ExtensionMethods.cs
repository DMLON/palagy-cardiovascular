using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public static class ExtensionMethods
    {
        public static T TakeOutItem<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                var it = item;
                list.Remove(item);
                return it;
            }
            return default(T);
        }

        public static T RemoveAndGet<T>(this IList<T> list, int index)
        {
            lock (list)
            {
                T value = list[index];
                list.RemoveAt(index);
                return value;
            }
        }

		public static int GetValue(this Direction d)
		{
			switch (d)
			{
				case Direction.Up:
					return 0;
				case Direction.Down:
					return 1;
				case Direction.North:
					return 2;
				case Direction.South:
					return 3;
				case Direction.East:
					return 4;
				case Direction.West:
					return 5;
				default:
					break;
			}
			return 0;
		}
	}
}
