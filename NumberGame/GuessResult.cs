namespace NumberGameCore;

public struct GuessResult : IEquatable<GuessResult>
{
	public readonly int exactCount;
	public readonly int nonExactCount;

	public GuessResult(int exactCount, int nonExactCount)
	{
		this.exactCount = exactCount;
		this.nonExactCount = nonExactCount;
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
}