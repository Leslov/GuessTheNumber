using AutoGuesser.Something_Useful;
using Extras.Extensions;
using NumberGameCore;

namespace AutoGuesser.Guessing
{

	public class Guess
	{
		public Guess(int[] guessed, GuessResult guessResult)
		{
			GuessResult = guessResult;
			Guessed = guessed;
		}
		public Guess(AnswerVariant answerVariant, GuessResult guessResult)
		{
			GuessResult = guessResult;
			Guessed = answerVariant.GetFullNumber();
		}
		public GuessResult GuessResult { get; set; }
		public int[] Guessed { get; }

		public bool IsMatchesCrappy(AnswerVariant variant)
		{
			if (variant.DigitsCount != Guessed.Length)
				throw new ArgumentException($"Variant length must be equals to {Guessed.Length}");

			int[] variantNumbers = variant.GetFullNumber();
			int[] range = Enumerable.Range(0, variantNumbers.Length).ToArray();

			#region exacts

			int exactIntersectionsExpected = GuessResult.exactCount;
			int exactIntersectionsActual = range.Select(i => variantNumbers[i] == Guessed[i]).Where(x => x).Count();

			#endregion

			#region non Exacts

			int nonExactIntersectionsExpected = GuessResult.nonExactCount;
			int nonExactIntersectionsActual = range
				.Select(i => Guessed.ExceptByIndex(i).Contains(variantNumbers[i])).Where(x => x).Count();

			#endregion

			return exactIntersectionsActual == exactIntersectionsExpected &&
				   nonExactIntersectionsActual == nonExactIntersectionsExpected;
		}

		public bool IsMatches(AnswerVariant answerVariant)
		{
			if (answerVariant.DigitsCount != Guessed.Length)
				throw new ArgumentException($"Variant length must be equals to {Guessed.Length}");
			
			var guessResultExpected = GuessResult;

			int[] variantNumbers = answerVariant.GetFullNumber();
			GuessResult guessResultActual = NumberGame.DoFooGuess(Guessed, variantNumbers);
			return guessResultExpected == guessResultActual;
		}
		public int Value { get; set; }
	}
}
