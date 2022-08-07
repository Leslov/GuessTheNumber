using GuesserDB.DBBase;

namespace GuesserDB.Entities.Holders;

public class SomeFullGuessHolder
{
	private static Lazy<FullGuess[,]> fullGuessesEzNav = new(InitEzNav);

	private static FullGuess[,] InitEzNav()
	{
		var db = DataHolder.Instance;
		var fullGuesses = db.FullGuess.ToArray();

		var grouped = fullGuesses.GroupBy(x => x.GetGuess());
		FullGuess[,] result = new FullGuess[grouped.Count(), grouped.First().Count()];
		foreach (var groupedSameGuesses in grouped)
		{
			FullGuess[] sameGuesses = groupedSameGuesses.OrderBy(x => x.GuessResultId).ToArray();
			Guess guess = groupedSameGuesses.Key;
			foreach (FullGuess fullGuess in sameGuesses)
			{
				result[guess.Id, fullGuess.GuessResultId] = fullGuess;
			}
		}

		return result;
	}

	public static FullGuess GetFullGuess(Guess guessed, GuessResult result)
	{
		return fullGuessesEzNav.Value[guessed.Id, result.Id];
	}
}