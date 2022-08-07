using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extras.Something_Useful
{
	public class GuessChances
	{
		private PositionExactChances[] _positionExactChances;

		public GuessChances(byte positionsCount, byte numbersCount)
		{
			_positionExactChances = Enumerable.Range(1, positionsCount)
				.Select(x => new PositionExactChances(numbersCount, (byte)x)).ToArray();
		}
		
	}

	internal class PositionExactChances
	{
		private readonly byte _numbersCount;
		public byte Position { get; }

		internal PositionExactChances(byte numbersCount, byte position)
		{
			_numbersCount = numbersCount;
			Position = position;
			Chances = new decimal[numbersCount];
		}
		public decimal[] Chances { get; set; }
		public decimal SummChances => Math.Round(Chances.Sum(), 4);
		public bool IsValidChances() => SummChances == 1;
	}
}
