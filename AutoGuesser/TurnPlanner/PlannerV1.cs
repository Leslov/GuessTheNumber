using System.Collections.Concurrent;
using AutoGuesser.Guessing;
using NumberGameCore;

namespace AutoGuesser.TurnPlanner;

public class PlannerV1 : TurnPlannerBase
{
	protected override async Task<ProposedGuess> GuessNext(CancellationToken token, Guess[] guessHistory, AnswerVariant[] answerVariants)
	{
		AnswerVariant[] previousGuesses = guessHistory.Select(x => new AnswerVariant(x.Guessed)).ToArray();
		AnswerVariant[] allAnswers = Guesser.GenerateAnswerVariants().Except(previousGuesses).ToArray();
		Guess[] possibleGuessesWithResult =
			allAnswers.SelectMany(guessed => GetPossibleGuesses(allAnswers, guessHistory, guessed)).ToArray();

		int initialAnswersCount = answerVariants.Length;





		/*foreach (Guess guess in possibleGuessesWithResult)
		{
			int answersCount = GetPossibleAnswers(allAnswers, guessHistory, guess).Count();
			guess.Value = initialAnswersCount - answersCount;
		}*/
		
		var tasks = possibleGuessesWithResult.Select(async guess =>
		{
			int answersCount = await Task.Run(() => GetPossibleAnswers(allAnswers, guessHistory, guess).Count(), token);
			guess.Value = initialAnswersCount - answersCount;
		});
		await Task.WhenAll(tasks);



		var proposedGuesses = possibleGuessesWithResult.GroupBy(x => x.Guessed)
			.Select(x => new ProposedGuess(x.ToArray()))
			.OrderBy(x => x.AverageValue)
			.ToArray();
		return proposedGuesses.First();
	}

	/*/// <summary>
	/// Possible GuessResults to guess with that guessHistory
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private GuessResult[] GetPossibleResponses(Guess[] guessHistory, AnswerVariant guessed)
	{
		throw new NotImplementedException();
	}*/
	/// <summary>
	/// Possible GuessResults to guess with that guessHistory
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private Guess[] GetPossibleGuesses(AnswerVariant[] allAnswers, Guess[] guessHistory, AnswerVariant guessed)
	{
		Guess[] allGuesses = GeneratePossibleGuessResults(NumbersCount).Select(gr => new Guess(guessed, gr)).ToArray();
		Guess[] possibleGuesses = allGuesses
			.Where(guess => Guesser.GetFilteredAnswers(allAnswers, guessHistory, guess).Length > 0).ToArray();
		return possibleGuesses;
	}

	/// <summary>
	/// FIltered FOO
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private AnswerVariant[] GetPossibleAnswers(AnswerVariant[] allAnswers, Guess[] guessHistory, Guess guess)
	{
		return Guesser.GetFilteredAnswers(allAnswers, guessHistory, guess);
	}
}