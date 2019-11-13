﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palagy
{
	public class SubSets
	{
		public static readonly Dictionary<int, int[]> S26 = new Dictionary<int, int[]>() {
			{ 0, new int[] { } },
			{ 1, new int[] { 0 } },
			{ 2, new int[] { 1 } },
			{ 3, new int[] { 0, 1 } },
			{ 4, new int[] { 0, 1, 2, 3 } },
			{ 5, new int[] { 1, 2, 4 } },
			{ 6, new int[] { 3, 4 } },
			{ 7, new int[] { 3, 4, 5, 6 } },
			{ 8, new int[] { 4, 5, 7 } },
			{ 9, new int[] { 0, 1, 3, 4 } },
			{ 10, new int[] { 0, 1, 2, 3, 4, 5, 9 } },
			{ 11, new int[] { 1, 2, 4, 5, 10} },
			{ 12, new int[] { 0, 1, 3, 4, 6, 7, 9 } },
			{ 13, new int[] { 1, 2, 4, 5, 7, 8, 10} },
			{ 14, new int[] { 3, 4, 6, 7, 12} },
			{ 15, new int[] { 3, 4, 5, 6, 7, 8, 12, 13, 14} },
			{ 16, new int[] { 4, 5, 7, 8, 13, 15} },
			{ 17, new int[] { 9, 10, 12} },
			{ 18, new int[] { 9, 10, 11, 12, 13, 17} },
			{ 19, new int[] { 10, 11, 13, 18} },
			{ 20, new int[] { 9, 10 ,12, 14, 15, 17} },
			{ 21, new int[] {9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 } },
			{ 22, new int[] { 10, 11, 13, 15, 16, 18, 19, 21} },
			{ 23, new int[] {12, 14, 15, 20, 21 } },
			{ 24, new int[] {12, 13, 14, 15, 16, 20, 21, 22, 23 } },
			{ 25, new int[] {13, 15, 16, 21, 22, 24 } },
		};

		public static readonly Dictionary<int, int[]> S26CC = new Dictionary<int, int[]>() {
			{ 0, new int[] { 1, 3, 4, 9, 10, 12 } },
			{ 1, new int[] { 0, 2, 3, 4, 5, 9, 10, 11, 12, 13 } },
			{ 2, new int[] { 1, 4, 5, 10, 11, 13 } },
			{ 3, new int[] { 0, 1, 4, 6, 7, 9, 10, 12, 14, 15 } },
			{ 4, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
			{ 5, new int[] { 1, 2, 4, 7, 8, 10, 11, 13, 15, 16 } },
			{ 6, new int[] { 3, 4, 7, 12, 14, 15 } },
			{ 7, new int[] { 3, 4, 5, 6, 8, 12, 13, 14, 15, 16 } },
			{ 8, new int[] { 4, 5, 7, 13, 15, 16 } },
			{ 9, new int[] { 0, 1, 3, 4, 10, 12, 17, 18, 20, 21 } },
			{ 10, new int[] { 0, 1, 2, 3, 4, 5, 9, 11, 12, 13, 17, 18, 19, 20, 21, 22 } },
			{ 11, new int[] { 1, 2, 4, 5, 10, 13, 18, 19, 21, 22} },
			{ 12, new int[] { 0, 1, 3, 4, 6, 7, 9, 10, 14, 15, 17, 18, 20, 21, 23, 24 } },
			{ 13, new int[] { 1, 2, 4, 5, 7, 8, 10, 11, 15, 16, 18, 19, 21, 22, 24, 25} },
			{ 14, new int[] { 3, 4, 6, 7, 12, 15, 20, 21, 23, 24} },
			{ 15, new int[] { 3, 4, 5, 6, 7, 8, 12, 13, 14, 16, 20, 21, 22, 23, 24, 25} },
			{ 16, new int[] { 4, 5, 7, 8, 13, 15, 21, 22, 24, 25} },
			{ 17, new int[] { 9, 10, 12, 18, 20, 21} },
			{ 18, new int[] { 9, 10, 11, 12, 13, 17, 19, 20, 21, 22} },
			{ 19, new int[] { 10, 11, 13, 18, 21, 22} },
			{ 20, new int[] { 9, 10 ,12, 14, 15, 17, 21, 23, 24} },
			{ 21, new int[] {9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 23, 24, 25 } },
			{ 22, new int[] { 10, 11, 13, 15, 16, 18, 19, 21, 24, 25} },
			{ 23, new int[] {12, 14, 15, 20, 21, 24 } },
			{ 24, new int[] {12, 13, 14, 15, 16, 20, 21, 22, 23, 25 } },
			{ 25, new int[] {13, 15, 16, 21, 22, 24 } },
		};
	}
}
