using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extras.Extensions;
using GuesserDB.DBBase;

namespace GuesserDB.Entities.Holders
{
	internal class SomeGuessResultHolder
	{
		private static readonly Lazy<GuessResult[]> guessResults = new(DataHolder.Instance.GuessResult.ToArray);

		public static void Init() => _ = guessResults.Value;

		public static GuessResult GetGuessResultById(byte guessResultId) => guessResults.Value[guessResultId];
	}
}
