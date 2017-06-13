using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BridgeportClaims.Common.Extensions
{
	public static class CollectionBaseExtentions
	{
		public static List<T> ToList<T>(this CollectionBase collection)
		{
			return collection.Cast<T>().ToList();
		}

		public static IEnumerable<T> Join<T>(this CollectionBase collection1, CollectionBase collection2)
		{
			return Join<T>(collection1, collection2, null);
		}

		public static IEnumerable<T> Join<T>(this CollectionBase collection1, 
			CollectionBase collection2, IEqualityComparer<T> comparer)
		{
			var all = collection1.Cast<T>().ToList();
			foreach (T item in collection2)
			{
				if (comparer != null)
				{
					if (!all.Contains<T>(item, comparer))
					{
						all.Add(item);
					}
				}
				else
				{
					if (!all.Contains(item))
					{
						all.Add(item);
					}
				}
			}
			return all;
		}
	}
}
