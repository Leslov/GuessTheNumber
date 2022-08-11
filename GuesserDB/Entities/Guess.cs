namespace GuesserDB.Entities
{
	public class Guess
	{
		public short Id { get; }
		public byte[] Guessed { get; }

		internal Guess(short id, params byte[] guessed)
		{
			Id = id;
			this.Guessed = guessed;
			//DigitsCount = (byte)Guessed.Length;
		}

		/// <summary>
		/// Запрашиваем совпадения
		/// </summary>
		public GuessResult GetMatches(Guess guessed)
		{
			byte[] answer = this.Guessed;
			int len = answer.Length;
			byte exactCount = 0;
			byte nonExactCount = 0;
			for (byte i = 0; i < len; i++)
			{
				byte currentGuessed = guessed.GetDigit(i);
				if (answer[i] == currentGuessed)
					exactCount++;
				else if (answer.Contains(currentGuessed))
					nonExactCount++;
			}
			return new GuessResult(exactCount, nonExactCount);
		}
		//public byte DigitsCount { get; }
		public byte GetDigit(byte index) => Guessed[index];
		public override string ToString()
		{
			return $"{string.Join("", Guessed)}";
		}
		public static bool operator ==(Guess obj1, Guess obj2) => obj1.Equals(obj2);
		public static bool operator !=(Guess obj1, Guess obj2) => !obj1.Equals(obj2);
		public override bool Equals(object? obj)
		{
			if (obj is Guess obj2)
				return obj2.Id == this.Id; //obj2.Guessed.SequenceEqual(Guessed);
			else
				return false;
		}
	}
}
