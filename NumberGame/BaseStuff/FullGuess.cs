using NumberGameCore.BaseStuff.Holders;

namespace NumberGameCore.BaseStuff;

public struct FullGuess
{
	private readonly short guessId;
	private readonly GuessResult guessResult;

	public FullGuess(short guessId, GuessResult guessResult)
	{
		this.guessId = guessId;
		this.guessResult = guessResult;
	}

	public FullGuess(Guess guess, GuessResult guessResult)
	{
		this.guessId = SomeGuessHolder.GetGuessId(guess);
		this.guessResult = guessResult;
	}

	public Guess GetGuess() => SomeGuessHolder.GetGuessById(guessId);
	public GuessResult GetGuessResult() => guessResult;//SomeGuessResultHolder.GetGuessResultById(guessResultId);
	public bool IsMatches(Guess answer)//TODO: Мейби эту хуйню тоже заранее генерить? Можно даже засейвить и потом грузить с файла. Смотрим по производительности
	{
		var guessResultExpected = GetGuessResult();

		GuessResult guessResultActual = answer.GetMatches(GetGuess());
		return guessResultExpected == guessResultActual;
	}

	public string GetReadableResult() => $"{string.Join("", GetGuess().Guessed)} - {GetGuessResult()}";
}