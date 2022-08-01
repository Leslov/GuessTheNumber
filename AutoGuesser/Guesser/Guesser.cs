using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGuesser.Something_Useful;
using AutoGuesser.TurnPlanner;
using Extras.Extensions;
using NumberGameCore;

namespace AutoGuesser.Guessing
{
	public class Guesser : IUserInputProcessor
	{
		protected readonly List<Guess> guessHistory = new();
		protected AnswerVariant[] answerVariants { get; private set; }
		protected readonly IOutputWriter outputWriter;

		public Guesser(IOutputWriter outputWriter)
		{
			answerVariants = GenerateAnswerVariants();
			this.outputWriter = outputWriter;
		}

		#region Commands

		public virtual string ProcessorName => "guesser";
		public bool ShowChancesTableAfterEachTurn { get; set; }

		public MyCommand[] GetCommandList()
		{
			return new MyCommand[]
			{
				new MyCommand("info",
					"Показать таблицу шансов. Параметр 0 или 1 - выключить/включить отображение после каждого хода",
					OnInfoInvoke),
				new MyCommand("help", "Запросить оптимальный ход", (s) => GetNext()),
				new MyCommand("is_possible", "Является ли ход возможным решением. Пример вызова: 'is_possible 1234'",
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
			bool isPossible = answerVariants.Any(x => guessed.SequenceEqual(x.GetFullNumber()));//TODO: Можно изи оптимизировать
			string possibleString = isPossible ? "Possible" : "Impossible!";
			outputWriter.WriteLine($"The answer {args} is {possibleString}");
		}

		#endregion

		public static AnswerVariant[] GenerateAnswerVariants()
		{
			var variants = new List<AnswerVariant>();
			for (int a = 0; a < 10; a++)
			{
				for (int b = 0; b < 10; b++)
				{
					if (b == a)
						continue;
					for (int c = 0; c < 10; c++)
					{
						if (c == a || c == b)
							continue;
						for (int d = 0; d < 10; d++)
						{
							if (d == a || d == b || d == c)
								continue;
							variants.Add(new AnswerVariant(a, b, c, d));
						}
					}
				}
			}
			return variants.ToArray();
		}

		public int[] GetNext()
		{
			if (!guessHistory.Any())
				return answerVariants.Random().GetFullNumber();//new byte[] { 0, 1, 2, 3 };
			throw new NotImplementedException();
		}

		public void ApplyGuessResult(int[] guessed, GuessResult guessResult)
		{
			
			nextTurnPlanner.StopPlanningNextTurn();
			guessHistory.Add(new Guess(guessed, guessResult));
			var guessValue = FilterPossibleAnswers();
			outputWriter.WriteLine($"Guess value = {guessValue}");
			nextTurnPlanner.StartPlanning(guessHistory.ToArray(), answerVariants);
			if (ShowChancesTableAfterEachTurn)
				outputWriter.WriteLine(GetReadableChanceTable());
		}

		public decimal[][] GenerateChanceTable()
		{
			int columnsCount = 4;
			int rowsCount = 10;
			decimal[][] table = new decimal[rowsCount][];
			for (int rowId = 0; rowId < rowsCount; rowId++)
			{
				table[rowId] = new decimal[columnsCount + 1];
				for (int columnId = 0; columnId < columnsCount; columnId++)
				{
					int[] digits = answerVariants.Select(x => x.GetDigit(columnId)).ToArray();
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

		public static AnswerVariant[] GetFilteredAnswers(AnswerVariant[] answerVariants, IEnumerable<Guess> guessHistory, params Guess[] nextGuesses)
		{
			return answerVariants.Where(answer =>
				guessHistory.All(guess => guess.IsMatches(answer))
				&& nextGuesses.All(guess => guess.IsMatches(answer))).ToArray();
		}

		#region Next turn planning

		private TurnPlannerBase nextTurnPlanner = TurnPlannerBase.CreateV1Planner();

		#endregion
	}
}
