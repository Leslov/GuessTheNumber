using System.Collections.Concurrent;
using AutoGuesser.Guessing;
using NumberGameCore;
using NumberGameCore.BaseStuff;
using NumberGameCore.BaseStuff.Holders;

namespace AutoGuesser.TurnPlanner;

public class PlannerV1 : TurnPlannerBase
{
	protected override async Task<ProposedGuess> GuessNext(CancellationToken token, FullGuess[] guessHistory, Guess[] answerVariants)
	{
		Guess[] previousGuesses = guessHistory.Select(x => x.GetGuess()).ToArray();
		Guess[] allAnswers = SomeGuessHolder.GetAllGuesses().Except(previousGuesses).ToArray();
		EvaluatedFullGuess[] possibleEvaluatedFullGuesses =
			allAnswers.SelectMany(guessed => GetPossibleGuesses(allAnswers, guessHistory, guessed)).ToArray();
		//66.6 seconds - single core
		int initialAnswersCount = answerVariants.Length;//TODO: Способ оптимизровать:
		/// 1) СОхранять результаты
		/// 2) Сделать вычисление многопоточным





		/*foreach (FullGuess guess in possibleGuessesWithResult)
		{
			int answersCount = GetPossibleAnswers(allAnswers, guessHistory, guess).Count();
			guess.Value = initialAnswersCount - answersCount;
		}*/
		bool isPossibleAnswer(EvaluatedFullGuess efg) => answerVariants.Contains(efg.FullGuess.GetGuess());
		double getPossibilityValue(EvaluatedFullGuess efg) => isPossibleAnswer(efg) ? 10 / (double) answerVariants.Length : 0;

		var tasks = possibleEvaluatedFullGuesses.Select(async efg =>
		{
			int answersCount = await Task.Run(() => GetPossibleAnswers(allAnswers, guessHistory, efg.FullGuess).Count(), token);
			efg.Value = (initialAnswersCount - answersCount) + getPossibilityValue(efg);
		});//TODO: Оценка не совсем корректная. Не учитывается вероятность гессрезалта
		await Task.WhenAll(tasks);
		//25 seconds - all cores


		var proposedGuesses = possibleEvaluatedFullGuesses.GroupBy(x => x.FullGuess.GetGuess())
			.Select(x => new ProposedGuess(x.ToArray()))
			.OrderByDescending(x => x.AverageValue)
			.ToArray();
		var foo = proposedGuesses.Where(x => x.Guess.Id
		                                     == SomeGuessHolder.GetByGuessed(9, 2, 1, 7).Id);
		return proposedGuesses.First();
	}

	/*/// <summary>
	/// Possible GuessResults to guess with that guessHistory
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private GuessResult[] GetPossibleResponses(FullGuess[] guessHistory, Guess guessed)
	{
		throw new NotImplementedException();
	}*/
	/// <summary>
	/// Possible GuessResults to guess with that guessHistory
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private EvaluatedFullGuess[] GetPossibleGuesses(Guess[] allAnswers, FullGuess[] guessHistory, Guess guessed)
	{
		FullGuess[] allGuesses = GeneratePossibleGuessResults(NumbersCount).Select(gr => new FullGuess(guessed, gr)).ToArray();
		FullGuess[] possibleGuesses = allGuesses
			.Where(guess => Guesser.GetFilteredAnswers(allAnswers, guessHistory, guess).Length > 0).ToArray();
		return possibleGuesses.Select(x => new EvaluatedFullGuess(x, 0)).ToArray();
	}

	/// <summary>
	/// FIltered FOO
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private Guess[] GetPossibleAnswers(Guess[] allAnswers, FullGuess[] guessHistory, FullGuess guess)
	{
		return Guesser.GetFilteredAnswers(allAnswers, guessHistory, guess);
	}
}