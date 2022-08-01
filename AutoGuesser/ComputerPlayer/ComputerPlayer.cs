
using AutoGuesser.Guessing;
using Extras.Extensions;
using NumberGameCore;

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

		public MoveResult NextMove()
		{
			int[] guessed = guesser.GetNext();
			var result = game.Guess(guessed);
			guesser.ApplyGuessResult(guessed, result);
			return new MoveResult(guessed, result);
		}
	}
}