using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extras.Extensions
{
	public static class IntExtensions
	{
		/// <summary>
		/// f(n) = 1 + 2 + 3 + ... + n
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int GetSummaRyada(this int n) => (1 + n) * n / 2;
	}
}
