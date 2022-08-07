using GuesserDB.Entities;

namespace AutoGuesser.Guessing;

public class ProposedGuess
{
	private EvaluatedFullGuess[] guesses;
	public Guess Guess { get; }

	public ProposedGuess(EvaluatedFullGuess[] guesses)
	{
		this.guesses = guesses;
		this.Guess = guesses.First().FullGuess.GetGuess();
	}

	public double LowestValue => guesses.Select(x => x.Value).Min();
	public double HighestValue => guesses.Select(x => x.Value).Max();
	public double AverageValue => guesses.Select(x => x.Value).Average();
}

public class EvaluatedFullGuess
{
	public FullGuess FullGuess { get; }
	public double Value { get; set; }

	internal EvaluatedFullGuess(FullGuess fullGuess, int value)
	{
		FullGuess = fullGuess;
		Value = value;
	}
}