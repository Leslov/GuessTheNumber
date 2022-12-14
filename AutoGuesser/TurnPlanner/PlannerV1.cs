using System.Collections.Concurrent;
using AutoGuesser.Guessing;
using GuesserDB.DBBase;
using GuesserDB.Entities;
using GuesserDB.Entities.Holders;
using NumberGameCore;

namespace AutoGuesser.TurnPlanner;

public class PlannerV1 : TurnPlannerBase
{
	protected override async Task<ProposedGuess> GuessNext(CancellationToken token, FullGuess[] guessHistory, Guess[] answerVariants)
	{
		ushort[] historyFGIds = guessHistory.Select(x => x.Id).ToArray();
		Guess[] previousGuesses = guessHistory.Select(x => x.GetGuess()).ToArray();
		Guess[] allAnswers = SomeGuessHolder.GetAllGuesses().Except(previousGuesses).ToArray();
		EvaluatedFullGuess[] possibleEvaluatedFullGuesses =
			allAnswers.SelectMany(guessed => GetPossibleGuesses(allAnswers, guessHistory, guessed)).ToArray();
		//24 seconds - single core
		int initialAnswersCount = answerVariants.Length;





		bool isPossibleAnswer(EvaluatedFullGuess efg) => answerVariants.Contains(efg.FullGuess.GetGuess());
		double getPossibilityValue(EvaluatedFullGuess efg) => isPossibleAnswer(efg) ? 10 / (double)initialAnswersCount : 0;

		/*Parallel.ForEach(possibleEvaluatedFullGuesses, efg =>
		{
			int answersCount = GetPossibleAnswers(allAnswers, guessHistory, efg.FullGuess).Count();
			efg.Value = (initialAnswersCount - answersCount) + getPossibilityValue(efg);
		});//Медленно*/

		Parallel.ForEach(possibleEvaluatedFullGuesses, efg =>
		{
			int answersCount = GetPossibleAnswersCount(allAnswers.Select(x => x.Id).ToArray(),
				historyFGIds.Concat(new[] { efg.FullGuess.Id }).ToArray());
			//double chance = 
			efg.Value = ((initialAnswersCount - answersCount) + getPossibilityValue(efg)); //* chance;
		});//Оценка не совсем корректная. Не учитывается вероятность гессрезалта

		//5.5 seconds - all cores


		var proposedGuesses = possibleEvaluatedFullGuesses.GroupBy(x => x.FullGuess.GetGuess())
			.Select(x => new ProposedGuess(x.ToArray()))
			.OrderByDescending(x => x.AverageValue)
			.ToArray();
		return proposedGuesses.First();
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