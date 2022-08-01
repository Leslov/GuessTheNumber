namespace AutoGuesser.Guessing;

public interface IOutputWriter
{
	void Write(string output);
	void WriteLine(string output);
	string ReadLine();
}