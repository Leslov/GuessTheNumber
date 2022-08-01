using AutoGuesser;
using NUnit.Framework;

namespace AutoGuesserTest
{
	public class AnswerVariantShortTest
	{
		[Test]
		public void AnswerVariantShortTestConstructor()
		{
			int[] expected = new[]{ 1, 2, 3, 4 };
			AnswerVariant foo = new AnswerVariant(expected);

			int[] actual = foo.GetFullNumber();
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AnswerVariantShortTest_GetDigit()
		{
			int n1 = 1;
			int n2 = 2;
			int n3 = 3;
			int n4 = 4;

			AnswerVariant foo = new AnswerVariant(n1, n2, n3, n4);

			int actual_n1 = foo.GetDigit(0);
			int actual_n2 = foo.GetDigit(1);
			int actual_n3 = foo.GetDigit(2);
			int actual_n4 = foo.GetDigit(3);
			Assert.AreEqual(n1, actual_n1);
			Assert.AreEqual(n2, actual_n2);
			Assert.AreEqual(n3, actual_n3);
			Assert.AreEqual(n4, actual_n4);
		}
	}
}