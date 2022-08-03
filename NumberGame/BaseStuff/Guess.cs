using NumberGameCore.BaseStuff.Holders;

namespace NumberGameCore.BaseStuff
{
	public struct Guess
	{
		public short Id { get; }
		public int[] Guessed { get; }

		internal Guess(short id, params int[] guessed)
		{
			Id = id;
			this.Guessed = guessed;
		}

		/// <summary>
		/// Запрашиваем совпадения
		/// </summary>
		public GuessResult GetMatches(Guess guessed)
		{
			int[] answer = this.Guessed;
			int len = answer.Length;
			byte exactCount = 0;
			byte nonExactCount = 0;
			for (int i = 0; i < len; i++)
			{
				var currentGuessed = guessed.Guessed[i];
				if (answer[i] == currentGuessed)
					exactCount++;
				else if (answer.Contains(currentGuessed))//TODO: Slowest!
					nonExactCount++;
			}
			return new GuessResult(exactCount, nonExactCount);
		}
		public int DigitsCount => Guessed.Length;
		public int GetDigit(int index) => Guessed[index];
		public override string ToString()
		{
			return $"{string.Join("", Guessed)}";
		}

		public override bool Equals(object? obj)
		{
			if (obj is Guess obj2)
				return obj2.Id == this.Id; //obj2.Guessed.SequenceEqual(Guessed);
			else
				return false;
		}
	}
}
