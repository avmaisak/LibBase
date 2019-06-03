using System;
using System.Collections.Generic;

namespace LibBase.Extensions
{
	/// <summary>
	/// Extension methods for IEnumerable.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Distinct By Expression.
		/// </summary>
		/// <typeparam name="TSource">Source.</typeparam>
		/// <typeparam name="TKey">Key.</typeparam>
		/// <param name="source">IEnumerable.</param>
		/// <param name="keySelector">Select expression.</param>
		/// <returns></returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var seenKeys = new HashSet<TKey>();
			foreach (var element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}
	}
}
