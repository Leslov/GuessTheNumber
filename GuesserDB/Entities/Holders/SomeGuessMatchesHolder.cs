using GuesserDB.DBBase;
using GuesserDB.Entities;

public class SomeGuessMatchesHolder
{
	public static bool IsMatches(FullGuess fullGuess, Guess answer)
	{
		DataHolder db = DataHolder.Instance;
		return db.GuessMatches[fullGuess.Id,answer.Id];
	}
}
