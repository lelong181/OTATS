using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Utils
{
	public static class ObjectExtensions
	{
		public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
		{
			return listToClone.Select((T item) => (T)item.Clone()).ToList();
		}
	}
}
