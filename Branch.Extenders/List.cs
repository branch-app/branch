using System;
using System.Collections.Generic;

namespace Branch.Extenders
{
	public static class ListExtenders
	{
		public static T RandomEntity<T>(this List<T> list)
		{
			if (list.Count == 0) return default(T);
			if (list.Count == 1) return list[0];

			var rnd = new Random(DateTime.Now.Millisecond);
			return list[(rnd.Next(0, list.Count))];
		}
	}
}
