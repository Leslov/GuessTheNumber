using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGuesser.Guessing;
using NumberGameCore;

namespace AutoGuesser.TurnPlanner
{
	public abstract class TurnPlannerBase
	{
		protected const int NumbersCount = 4;
		private bool isPlanningInProgress = false;
		private IAsyncResult proposeAsyncResult;
		protected ProposedGuess? proposedGuess { get; set; }

		protected abstract Task<ProposedGuess> GuessNext(CancellationToken token, Guess[] guessHistory,
			AnswerVariant[] answerVariants);
		public async Task StartPlanning(Guess[] guessHistory, AnswerVariant[] answerVariants)
		{
			if (!isPlanningInProgress)
			{
				isPlanningInProgress = true;
				proposedGuess = await GuessNext(planningTokenSource.Token, guessHistory, answerVariants);
				/*var proposeGuess = GuessNext;
				proposeAsyncResult =
					proposeGuess.BeginInvoke(planningTokenSource.Token, guessHistory,
						answerVariants, OnPlanningCompleted, proposeAsyncResult);*/
				isPlanningInProgress = false;
			}
		}

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
			for (int exacts = 0; exacts <= numbersCount; exacts++)
			{
				for (int nonExacts = 0; nonExacts <= numbersCount - exacts; nonExacts++)
				{
					guessResults.Add(new GuessResult(exacts, nonExacts));
				}
			}

			return guessResults.ToArray();
		}

		public static TurnPlannerBase CreateV1Planner() => new PlannerV1();
	}
}
