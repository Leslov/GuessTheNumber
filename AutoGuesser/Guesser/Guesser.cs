using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGuesser.TurnPlanner;
using Extras;
using Extras.Extensions;
using Extras.Something_Useful;
using GuesserDB.Entities;
using GuesserDB.Entities.Holders;
using NumberGameCore;

namespace AutoGuesser.Guessing
{
	public class Guesser : IUserInputProcessor
	{
		protected readonly List<FullGuess> guessHistory = new();
		protected Guess[] answerVariants { get; private set; }
		protected readonly IOutputWriter outputWriter;

		public Guesser(IOutputWriter outputWriter)
		{
			answerVariants = SomeGuessHolder.GetAllGuesses();
			this.outputWriter = outputWriter;
		}

		#region Commands

		public virtual string ProcessorName => "guesser";
		public bool ShowChancesTableAfterEachTurn { get; set; }

		public MyCommand[] GetCommandList()
		{
			return new MyCommand[]
			{
				new("info",
					"Показать таблицу шансов. Параметр 0 или 1 - выключить/включить отображение после каждого хода",
					OnInfoInvoke),
				new("help", "Запросить оптимальный ход", (s) => GetNext()),
				new("is_possible", "Является ли ход возможным решением. Пример вызова: 'is_possible 1234'",
					OnIsPossibleInvoke)
			};
		}

		protected void OnInfoInvoke(string args)
		{
			bool? argBoolean = args == "0" || args == "1" ? (bool?)(args == "1") : (bool?)null;
			if (argBoolean.HasValue)
				ShowChancesTableAfterEachTurn = argBoolean.Value;
			else
				outputWriter.WriteLine(GetReadableChanceTable());
		}

		protected void OnIsPossibleInvoke(string args)
		{
			int[] guessed = args.Select(x => (int)x).ToArray();
			Guess guess = SomeGuessHolder.GetByGuessed(guessed);
			bool isPossible = answerVariants.Contains(guess);
			string possibleString = isPossible ? "Possible" : "Impossible!";
			outputWriter.WriteLine($"The answer {args} is {possibleString}");
		}

		#endregion


		public Guess GetNext()
		{
			if (!guessHistory.Any())
				return answerVariants.Random();//new byte[] { 0, 1, 2, 3 };
			ProposedGuess propGuess = nextTurnPlanner.GuessNext(guessHistory.ToArray(), answerVariants);
			return propGuess.Guess;
		}

		public void ApplyGuessResult(FullGuess guessed)
		{
			nextTurnPlanner.StopPlanningNextTurn();
			guessHistory.Add(guessed);
			int guessValue = FilterPossibleAnswers();
			outputWriter.WriteLine($"FullGuess value = {guessValue}");

			if (ShowChancesTableAfterEachTurn)
				outputWriter.WriteLine(GetReadableChanceTable());
		}
		public void StartPlanning() => nextTurnPlanner.StartPlanning(guessHistory.ToArray(), answerVariants);

		public decimal[][] GenerateChanceTable()
		{
			int columnsCount = 4;
			int rowsCount = 10;
			decimal[][] table = new decimal[rowsCount][];
			for (byte rowId = 0; rowId < rowsCount; rowId++)
			{
				table[rowId] = new decimal[columnsCount + 1];
				for (byte columnId = 0; columnId < columnsCount; columnId++)
				{
					byte[] digits = answerVariants.Select(x => x.GetDigit(columnId)).ToArray();
					decimal exactsCount = (decimal)digits.Count(x => x == rowId);
					table[rowId][columnId] = exactsCount / digits.Length;
				}

				table[rowId][columnsCount] = table[rowId].Sum();
			}

			return table;
		}

		public string GetReadableChanceTable()
		{
			string remainingAnswerVariantsCount = $"Possible answers count = {answerVariants.Length}{Environment.NewLine}";
			var table = GenerateChanceTable();
			string getRow(int i)
			{
				var row = table[i];
				return $"{i}\t{string.Join("\t", row.Select(x => String.Format("{0:P2}", x)))}";
			}
			string tableHeader =
				$"Num\t{string.Join("\t", Enumerable.Range(1, 4).Select(x => x.ToString()))}\tSumm{Environment.NewLine}";

			var rowsIndexRange = Enumerable.Range(0, table.Length);
			string[] strRows = rowsIndexRange.Select(getRow).ToArray();
			string full = string.Join(Environment.NewLine, strRows);
			return remainingAnswerVariantsCount + tableHeader + full;
		}

		protected int FilterPossibleAnswers()
		{
			int previousCount = answerVariants.Length;
			answerVariants = GetFilteredAnswers(answerVariants, guessHistory);
			return previousCount - answerVariants.Length;
		}

		public static Guess[] GetFilteredAnswers(Guess[] answerVariants, IEnumerable<FullGuess> guessHistory, params FullGuess[] nextGuesses)
		{
			return answerVariants.Where(answer =>
				nextGuesses.All(guess => guess.IsMatches(answer))
				&& guessHistory.All(guess => guess.IsMatches(answer))).ToArray();
		}
		public static int GetFilteredAnswersCount(Guess[] answerVariants, IEnumerable<FullGuess> guessHistory, params FullGuess[] nextGuesses)
		{
			return answerVariants.Count(answer =>
				nextGuesses.All(guess => guess.IsMatches(answer))
				&& guessHistory.All(guess => guess.IsMatches(answer)));
		}
		public static bool AnyFilteredAnswer(Guess[] answerVariants, IEnumerable<FullGuess> guessHistory, params FullGuess[] nextGuesses)
		{
			return answerVariants.Any(answer =>
				nextGuesses.All(guess => guess.IsMatches(answer))
				&& guessHistory.All(guess => guess.IsMatches(answer)));
		}

		#region Next turn planning

		private TurnPlannerBase nextTurnPlanner = TurnPlannerBase.CreateV1Planner();

		#endregion
	}
}
