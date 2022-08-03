using AutoGuesser.Guessing;
using Extras.Extensions;
using NumberGameCore;
using NumberGameCore.BaseStuff;

namespace AutoGuesser
{
	public class ComputerPlayer
	{
		private readonly Guesser guesser;
		private readonly NumberGame game;
		public ComputerPlayer(NumberGame game, IOutputWriter outputWriter)
		{
			guesser = new Guesser(outputWriter);
			this.game = game;
		}

		public FullGuess NextMove()
		{
			Guess guessed = guesser.GetNext();
			var result = game.Guess(guessed);
			var fullGuess = new FullGuess(guessed, result);
			guesser.ApplyGuessResult(fullGuess);
			return fullGuess;
		}
	}
}