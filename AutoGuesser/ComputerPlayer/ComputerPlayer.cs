using AutoGuesser.Guessing;
using Extras;
using Extras.Extensions;
using GuesserDB.Entities;
using GuesserDB.Entities.Holders;
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

		public FullGuess NextMove()
		{
			Guess guessed = guesser.GetNext();
			var result = game.Guess(guessed);
			var fullGuess = SomeFullGuessHolder.GetFullGuess(guessed, result);
			guesser.ApplyGuessResult(fullGuess);
			return fullGuess;
		}
	}
}