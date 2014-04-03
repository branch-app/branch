using System;
using System.Collections.Generic;
using System.Linq;

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

		public static void Shuffle<T>(this IList<T> list)
		{
			var random = new Random();
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = random.Next(n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}


		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
		{
			while (source.Any())
			{
				yield return source.Take(chunkSize);
				source = source.Skip(chunkSize);
			}
		}

	}
}
