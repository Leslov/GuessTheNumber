using GuesserDB.DBBase;
using GuesserDB.Entities;

public class SomeGuessMatchesHolder
{
	private static DataHolder db = DataHolder.Instance;
	public static bool IsMatches(FullGuess fullGuess, Guess answer) => db.GuessMatches[fullGuess.Id, answer.Id];
}
