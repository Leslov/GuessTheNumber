using Extras.Extensions;

namespace GuesserDB.Entities;

public class GuessResult : IEquatable<GuessResult>
{
	public byte exactCount { get; }
	public byte nonExactCount { get; }
	public byte Id { get; }

	public GuessResult(byte exactCount, byte nonExactCount)
	{
		this.exactCount = exactCount;
		this.nonExactCount = nonExactCount;
		var digitsCount = ConstSettings.DigitsCount;
		this.Id = (byte)(nonExactCount + ((3 + digitsCount * 2) * exactCount - Math.Pow(exactCount, 2)) / 2);
	}

	public override string ToString()
	{
		return $"Exact: {exactCount}, nonExact: {nonExactCount}";
	}

	public override bool Equals(object? obj)
	{
		return obj is GuessResult other && Equals(other);
	}

	public bool Equals(GuessResult other)
	{
		return exactCount == other.exactCount && nonExactCount == other.nonExactCount;
	}

	public static bool operator ==(GuessResult obj1, GuessResult obj2) => obj1.Equals(obj2);
	public static bool operator !=(GuessResult obj1, GuessResult obj2) => !obj1.Equals(obj2);

	public override int GetHashCode()
	{
		return HashCode.Combine(exactCount, nonExactCount);
	}

	private static Lazy<byte> possibleCount = new(() => (byte)((int)ConstSettings.DigitsCount + 1).GetSummaRyada());
	public static byte GetPossibleCount() => possibleCount.Value;
}