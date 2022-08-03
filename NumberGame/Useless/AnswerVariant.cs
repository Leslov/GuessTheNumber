namespace NumberGameCore.IMPIDOR
{
	public class AnswerVariantVPNDRS
	{
		private int digitsCount;
		private int answer;
		public AnswerVariantVPNDRS(int answer)
		{
			this.answer = answer;
		}
		public AnswerVariantVPNDRS(params int[] numbers)
		{
			int agg(int a, int b)
			{
				var foo2 = Math.Max(0, a) + (numbers[b] << (4 * (digitsCount - b - 1)));
				return foo2;
			}
			this.digitsCount = numbers.Length;
			this.answer = Enumerable.Range(-1, numbers.Length + 1).Aggregate(agg);
		}
		public int GetAnswer() => answer;
		public int GetDigitsCount() => digitsCount;
		public int GetDigit(int index) => 0xF & (answer >> (4 * (digitsCount - index - 1)));
		public int[] GetFullNumber() => Enumerable.Range(0, digitsCount).Select(x => GetDigit(x)).ToArray();
		public override string ToString()
		{
			return $"{string.Join("", GetFullNumber())}";
		}

		public override bool Equals(object? obj)
		{
			if (obj is AnswerVariantVPNDRS obj2)
				return obj2.GetAnswer() == this.GetAnswer();
			else
				return false;
		}
	}
}