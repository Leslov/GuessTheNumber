using NumberGameCore.BaseStuff;
using NumberGameCore.BaseStuff.Holders;

namespace NumberGameCore
{
	public class NumberGame
	{
		private Guess answer;
		private readonly NumberGameSettings settings;
		public int GuessCount { get; private set; }
		public bool IsWon { get; private set; }
		public bool IsLose { get; private set; }
		public bool IsEnded => IsWon || IsLose;
		public NumberGame(NumberGameSettings settings)
		{
			this.settings = settings;
			NewGame();
		}

		private void NewGame()
		{
			answer = SomeGuessHolder.GetRandom();
			GuessCount = 0;
		}

		public Guess GetAnswer() => answer;

		public GuessResult Guess(Guess guessed)
		{
			if (IsLose)
				throw new Exception("You lose! And you can't do more guesses");

			var guessResult = answer.GetMatches(guessed);
			GuessCount++;
			IsWon = guessResult.exactCount == settings.NumberCount;
			IsLose = !IsWon && GuessCount >= settings.MaxAttempts;
			return guessResult;
		}
	}
}