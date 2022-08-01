namespace NumberGameCore;

public class NumberGameSettings
{
	public byte NumberCount { get; }
	public int MaxAttempts { get; }

	public NumberGameSettings(byte numberCount, int maxAttempts)
	{
		this.NumberCount = numberCount;
		MaxAttempts = maxAttempts;
	}
}