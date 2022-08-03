using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extras.Extensions;

namespace NumberGameCore.BaseStuff.Holders
{
	public class SomeGuessHolder
	{
		private static Lazy<Guess[,,,]> lazyGuessesEzNav = new(() =>
		{
			if (guessesEzNav == null)
				InitClassic();
			return guessesEzNav;
		});
		private static Lazy<Guess[]> lazyGuesses = new(() =>
		{
			if (guesses == null)
				InitClassic();
			return guesses;
		});

		private static Guess[,,,] guessesEzNav = null;
		private static Guess[] guesses = null;

		private static void InitClassic()
		{
			var ezNavGuesses = new Guess[10, 10, 10, 10];
			var ordinaryGuesses = new Guess[5040];
			short index = 0;
			for (int a = 0; a < 10; a++)
			{
				for (int b = 0; b < 10; b++)
				{
					if (b == a)
						continue;
					for (int c = 0; c < 10; c++)
					{
						if (c == a || c == b)
							continue;
						for (int d = 0; d < 10; d++)
						{
							if (d == a || d == b || d == c)
								continue;
							var guess = new Guess(index, a, b, c, d);
							ezNavGuesses[a, b, c, d] = guess;
							ordinaryGuesses[index] = guess;
							index++;
						}
					}
				}
			}

			guessesEzNav = ezNavGuesses;
			guesses = ordinaryGuesses;
		}

		public static void Init() => _ = lazyGuesses.Value;

		public static Guess GetGuessById(short guessId) => lazyGuesses.Value[guessId];

		public static Guess GetRandom() => lazyGuesses.Value.Random();

		public static int GetCount() => lazyGuesses.Value.Length;

		public static int GetDigitsCount() => 4;

		public static Guess[] GetAllGuesses() => lazyGuesses.Value.ToArray();

		public static short GetGuessId(Guess guess)
		{
			int[] g = guess.Guessed;
			return lazyGuessesEzNav.Value[g[0], g[1], g[2], g[3]].Id;
		}

		public static Guess GetByGuessed(params int[] g)
		{
			if (g?.Length != 4)
				throw new ArgumentException("Length must be 4");
			return lazyGuessesEzNav.Value[g[0], g[1], g[2], g[3]];
		}
	}
}
