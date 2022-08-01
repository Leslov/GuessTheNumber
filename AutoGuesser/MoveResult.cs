using NumberGameCore;

namespace AutoGuesser;

public struct MoveResult
{
	private readonly string readableResult;
	public MoveResult(int[] guessed, GuessResult guessResult)
	{
		readableResult = $"{string.Join("", guessed)} - {guessResult}";
	}

	public string GetReadableResult()
	{
		return readableResult;
	}
}