using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGuesser.Guessing;
using GuesserDB.Entities;
using NumberGameCore;

namespace AutoGuesser.TurnPlanner
{
	public abstract class TurnPlannerBase
	{
		protected const int NumbersCount = 4;
		private bool isPlanningInProgress = false;
		private IAsyncResult proposeAsyncResult;
		protected ProposedGuess? proposedGuess { get; private set; }

		public async Task StartPlanning(FullGuess[] guessHistory, Guess[] answerVariants)
		{
			if (!isPlanningInProgress)
			{
				planningTokenSource.TryReset();
				isPlanningInProgress = true;
				proposedGuess = await GuessNext(planningTokenSource.Token, guessHistory, answerVariants);
				/*var proposeGuess = GuessNext;
				proposeAsyncResult =
					proposeGuess.BeginInvoke(planningTokenSource.Token, guessHistory,
						answerVariants, OnPlanningCompleted, proposeAsyncResult);*/
				isPlanningInProgress = false;
			}
		}

		public ProposedGuess GuessNext(FullGuess[] guessHistory, Guess[] answerVariants) =>
			GuessNext(new CancellationToken(false), guessHistory, answerVariants).Result;
		protected abstract Task<ProposedGuess> GuessNext(CancellationToken token, FullGuess[] guessHistory,
			Guess[] answerVariants);

		/*private void OnPlanningCompleted(IAsyncResult resObj)
		{
			var proposeGuess = GuessNext;
			proposedGuess = proposeGuess.EndInvoke(resObj);
			isPlanningInProgress = false;
		}*/

		private CancellationTokenSource planningTokenSource { get; } = new();
		public void StopPlanningNextTurn()
		{
			planningTokenSource?.Cancel();
			proposedGuess = null;
		}

		protected static GuessResult[] GeneratePossibleGuessResults(int numbersCount)
		{
			List<GuessResult> guessResults = new List<GuessResult>();
			for (byte exacts = 0; exacts <= numbersCount; exacts++)
			{
				for (byte nonExacts = 0; nonExacts <= numbersCount - exacts; nonExacts++)
				{
					guessResults.Add(new GuessResult(exacts, nonExacts));
				}
			}

			return guessResults.ToArray();
		}

		public static TurnPlannerBase CreateV1Planner() => new PlannerV2();
	}
}
