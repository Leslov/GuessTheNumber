using System.Collections.Concurrent;
using AutoGuesser.Guessing;
using GuesserDB.DBBase;
using GuesserDB.Entities;
using GuesserDB.Entities.Holders;
using NumberGameCore;

namespace AutoGuesser.TurnPlanner;

public class PlannerV2 : TurnPlannerBase
{
	/// <summary>
	/// About 10.5 seconds
	/// </summary>
	protected override async Task<ProposedGuess> GuessNext(CancellationToken token, FullGuess[] guessHistory, Guess[] answerVariants)
	{
		int initialAnswersCount = answerVariants.Length;
		bool isPossibleAnswer(EvaluatedFullGuess efg) => answerVariants.Contains(efg.FullGuess.GetGuess());
		double getPossibilityValue(EvaluatedFullGuess efg) => isPossibleAnswer(efg) ? 10 / (double)initialAnswersCount : 0;


		ushort[] historyFGIds = guessHistory.Select(x => x.Id).ToArray();
		Guess[] previousGuesses = guessHistory.Select(x => x.GetGuess()).ToArray();
		Guess[] allAnswers = SomeGuessHolder.GetAllGuesses().Except(previousGuesses).ToArray();
		var allAnswerIds = allAnswers.Select(x => x.Id).ToArray();
		long count = allAnswers.Length;
		ProposedGuess[] resultPGs = new ProposedGuess[count];
		Parallel.For(0, count, (i, state) =>
		{
			var answer = allAnswers[i];
			EvaluatedFullGuess[] possibleEvaluatedFullGuesses = GetPossibleGuesses(allAnswers, guessHistory, answer);
			//58% cpu
			foreach (var efg in possibleEvaluatedFullGuesses)
			{
				int answersCount = GetPossibleAnswersCount(allAnswerIds,
					historyFGIds.Concat(new[] {efg.FullGuess.Id}).ToArray());
				// 34% cpu
				//double chance = 
				efg.Value = (initialAnswersCount - answersCount + getPossibilityValue(efg)); //* chance;
			}//TODO: Оценка не совсем корректная. Не учитывается вероятность гессрезалта

			resultPGs[i] = new ProposedGuess(possibleEvaluatedFullGuesses);
		});


		return resultPGs.MaxBy(x => x.AverageValue) ?? throw new Exception("Something went wrong!");
	}

	/// <summary>
	/// Possible GuessResults to guess with that guessHistory
	/// </summary>
	private EvaluatedFullGuess[] GetPossibleGuesses(Guess[] allAnswers, FullGuess[] guessHistory, Guess guessed)
	{
		FullGuess[] allGuesses = GeneratePossibleGuessResults(NumbersCount).Select(gr => new FullGuess(guessed, gr)).ToArray();
		FullGuess[] possibleGuesses = allGuesses
			.Where(guess => Guesser.AnyFilteredAnswer(allAnswers, guessHistory, guess)).ToArray();
		return possibleGuesses.Select(x => new EvaluatedFullGuess(x, 0)).ToArray();
	}

	private int GetPossibleAnswersCount(short[] allAnswerIds, params ushort[] fullGuessIds)
	{
		DataHolder db = DataHolder.Instance;
		int count = 0;
		//ushort[] fullGuessIds = guessHistory.Select(x => x.Id).Concat(new[] {fullGuess.Id}).ToArray();
		Parallel.ForEach(allAnswerIds, answerId =>
		{
			if (fullGuessIds.All(fullGuessId => db.GuessMatches[fullGuessId, answerId]))
				count++;
		});
		return count;
	}

	/// <summary>
	/// GetPossibleAnswersCount do better job
	/// </summary>
	private Guess[] GetPossibleAnswers(Guess[] allAnswers, FullGuess[] guessHistory, FullGuess guess) =>
		Guesser.GetFilteredAnswers(allAnswers, guessHistory, guess);
}