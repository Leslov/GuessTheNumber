namespace AutoGuesser.Guessing;

public class ProposedGuess
{
	private Guess[] guesses;

	public ProposedGuess(Guess[] guesses)
	{
		this.guesses = guesses;
	}

	public int LowestValue => guesses.Select(x => x.Value).Min();
	public int HighestValue => guesses.Select(x => x.Value).Max();
	public double AverageValue => guesses.Select(x => x.Value).Average();
}