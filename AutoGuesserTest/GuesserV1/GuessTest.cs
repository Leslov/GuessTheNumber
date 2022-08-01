using System.Drawing;
using AutoGuesser;
using AutoGuesser.Guessing;
using NumberGameCore;
using NUnit.Framework;

namespace AutoGuesserTest.GuesserV1
{
	public class GuessTest
	{
		/*private GuessResult guessResult { get; }
		//private int[] guessed { get; } = new[] {0, 1, 2, 3};

		//private Guess guess;
		[SetUp]
		public void SetUp()
		{
			guess = new Guess(guessed, guessResult);
		}*/

		[TestCase(new int[] { 9, 8, 7, 6 }, 4, 0, new int[] { 9, 8, 7, 6 }, true)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 4, 0, new int[] { 9, 8, 7, 5 }, false)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 2, 0, new int[] { 9, 8, 5, 0 }, true)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 2, 0, new int[] { 9, 8, 7, 6 }, false)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 2, 2, new int[] { 9, 8, 6, 7 }, true)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 2, 2, new int[] { 9, 8, 7, 0 }, false)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 0, 2, new int[] { 8, 9, 1, 2 }, true)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 0, 2, new int[] { 8, 9, 7, 6 }, false)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 0, 0, new int[] { 0, 1, 2, 3 }, true)]
		[TestCase(new int[] { 9, 8, 7, 6 }, 0, 0, new int[] { 6, 1, 2, 3 }, false)]
		public void IsMatchesTest(int[] previousGuess, int exacts, int nonExacts, int[] newGuess, bool expected)
		{
			//Arrange
			Guess guess = new Guess(previousGuess, new GuessResult(exacts, nonExacts));
			var answerVariant = new AnswerVariant(newGuess);

			//Act
			bool actual = guess.IsMatches(answerVariant);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
