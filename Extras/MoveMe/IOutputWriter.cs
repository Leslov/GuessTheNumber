namespace Extras;

public interface IOutputWriter
{
	void Write(string output);
	void WriteLine(string output);
	string ReadLine();
}