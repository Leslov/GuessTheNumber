namespace NumberGameCore
{
	public class NumberGame
	{
		private int[] theNumbers;
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
			Random ran = new Random(Guid.NewGuid().GetHashCode());
			theNumbers = Enumerable.Range(0, 9).OrderBy(x => ran.Next()).Take(4).ToArray();
			GuessCount = 0;
		}

		public int[] GetAnswer() => theNumbers;

		public GuessResult Guess(int[] guessed)
		{
			if (guessed.Distinct().Count() != guessed.Length)
				throw new Exception("Allowed only unique values");
			if (IsLose)
				throw new Exception("You lose! And you can't do more guesses");
			var guessResult = DoFooGuess(guessed, theNumbers);
			GuessCount++;
			IsWon = guessResult.exactCount == settings.NumberCount;
			IsLose = !IsWon && GuessCount >= settings.MaxAttempts;
			return guessResult;
		}

		public static GuessResult DoFooGuess(int[] guessed, int[] answer)
		{
			byte exactCount = 0;
			byte nonExactCount = 0;
			for (int i = 0; i < answer.Length; i++)
			{
				var currentGuessed = guessed[i];
				if (answer.Contains(currentGuessed))
				{
					if (answer[i] == currentGuessed)
						exactCount++;
					else
						nonExactCount++;
				}
			}
			return new GuessResult(exactCount, nonExactCount);
		}
	}
}