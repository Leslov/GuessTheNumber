using GuesserDB.DBBase;
using GuesserDB.Entities.Holders;

namespace GuesserDB.Entities;

public class FullGuess
{
	public short GuessId { get; }
	private readonly GuessResult guessResult;
	public short GuessResultId => guessResult.Id;

	/*public FullGuess(long id, short guessId, GuessResult guessResult)
	{
		this.Id = id;
		this.GuessId = guessId;
		this.guessResult = guessResult;
	}*/

	public FullGuess(Guess guess, GuessResult guessResult)
	{
		this.Id = (ushort)(guess.Id * GuessResult.GetPossibleCount() + guessResult.Id);
		this.GuessId = SomeGuessHolder.GetGuessId(guess);
		this.guessResult = guessResult;
	}
	public ushort Id { get; }
	public Guess GetGuess() => SomeGuessHolder.GetGuessById(GuessId);
	public GuessResult GetGuessResult() => guessResult;//SomeGuessResultHolder.GetGuessResultById(guessResultId);
	public bool IsMatchesSlow(Guess answer)
	{
		GuessResult guessResultExpected = GetGuessResult();

		GuessResult guessResultActual = answer.GetMatches(GetGuess());
		return guessResultExpected == guessResultActual;
	}

	public bool IsMatches(Guess answer) => SomeGuessMatchesHolder.IsMatches(this, answer);

	public string GetReadableResult() => $"{string.Join("", GetGuess().Guessed)} - {GetGuessResult()}";
}
