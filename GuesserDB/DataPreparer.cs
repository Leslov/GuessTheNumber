using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extras;
using Extras.Extensions;
using GuesserDB.DBBase;
using GuesserDB.Entities;
using GuesserDB.Entities.Holders;

namespace GuesserDB
{
	public class DataPreparer
	{
		private DataHolder db = DataHolder.Instance;
		public async Task FillAllData(ConsoleWriter writer)
		{
			db.ClearAll();

			Guess[] guesses = PrepareGuesses();
			GuessResult[] guessResults = PrepareGuessResults();
			FullGuess[] fullGuesses = PrepareFullGuesses(guesses, guessResults);
			bool[,] guessMatches = await PrepareGuessMatches(writer, fullGuesses, guesses);
			//db.Save();
		}

		private async Task<bool[,]> PrepareGuessMatches(ConsoleWriter writer, FullGuess[] fullGuesses, Guess[] guesses)
		{
			//GuessMatches[] guessMatches = new GuessMatches[fullGuesses.Length * guesses.Length];
			bool[,] boolyResults = new bool[fullGuesses.Length, guesses.Length];
			Parallel.ForEach(fullGuesses, fullGuess => 
			{
				foreach (Guess guess in guesses)
				{
					GuessResult guessResultExpected = fullGuess.GetGuessResult();

					GuessResult guessResultActual = guess.GetMatches(fullGuess.GetGuess());
					if (guessResultExpected == guessResultActual)
						boolyResults[fullGuess.Id, guess.Id] = true;
					/*guessMatches[index] = new GuessMatches
					{
						//Id = index,
						FullGuessId = fullGuess.Id,
						GuessId = guess.Id
					};
				index++;*/
				}
			});

			db.GuessMatches = boolyResults;
			return boolyResults;
		}

		private FullGuess[] PrepareFullGuesses(Guess[] guesses, GuessResult[] guessResults)
		{
			FullGuess[] fullGuesses = new FullGuess[guesses.Length * guessResults.Length];
			long index = 0;
			foreach (Guess guess in guesses)
			{
				foreach (GuessResult guessResult in guessResults)
				{
					fullGuesses[index] = new FullGuess(guess, guessResult);
					index++;
				}
			}
			db.FullGuess.SetRange(fullGuesses);
			return fullGuesses;
		}

		private Guess[] PrepareGuesses()
		{
			Guess[] guesses = SomeGuessHolder.GetAllGuesses();
			db.Guess.SetRange(guesses);
			return guesses;
		}

		private GuessResult[] PrepareGuessResults()
		{
			const byte digitsCount = ConstSettings.DigitsCount;
			int arrLen = GuessResult.GetPossibleCount();
			int index = 0;
			var variants = new GuessResult[arrLen];
			for (byte ex = 0; ex <= digitsCount; ex++)
			{
				for (byte nonEx = 0; nonEx <= digitsCount - ex; nonEx++)
				{
					variants[index++] = new GuessResult(ex, nonEx);
				}
			}

			db.GuessResult.SetRange(variants);
			return variants;
		}
	}
}
