using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGameCore.BaseStuff.Holders
{
	internal class SomeGuessResultHolder
	{
		private static Lazy<GuessResult[]> guessResults = new(GetClassic);
		private static GuessResult[] GetClassic()
		{
			const int digitsCount = 4;
			var variants = new List<GuessResult>();
			for (byte ex = 0; ex <= digitsCount; ex++)
			{
				for (byte nonEx = 0; nonEx <= digitsCount - ex; nonEx++)
				{
					variants.Add(new GuessResult(ex, nonEx));
				}
			}

			return variants.ToArray();
		}

		public static void Init() => _ = guessResults.Value;

		public static GuessResult GetGuessResultById(byte guessResultId) => guessResults.Value[guessResultId];
	}
}
