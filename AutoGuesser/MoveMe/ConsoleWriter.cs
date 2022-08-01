namespace AutoGuesser.Guessing;

public class ConsoleWriter : IOutputWriter
{
	public void Write(string output) => Console.Write(output);

	public void WriteLine(string output) => Console.WriteLine(output);

	public string ReadLine() => Console.ReadLine();

	public void NewLine() => Console.WriteLine();
}