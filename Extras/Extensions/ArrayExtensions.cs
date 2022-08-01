using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extras.Extensions
{
	public static class ArrayExtensions
	{
		public static T Random<T>(this T[] array)
		{
			Random random = new Random(Guid.NewGuid().GetHashCode());
			return array[random.Next(0, array.Length)];
		}

		public static T[] ExceptByIndex<T>(this T[] array, int index)
		{
			return array.Where((v, i) => i != index).ToArray();
		}
	}
}
