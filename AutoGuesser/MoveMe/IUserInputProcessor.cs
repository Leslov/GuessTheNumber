using AutoGuesser.Something_Useful;

namespace AutoGuesser;

public interface IUserInputProcessor
{
	string ProcessorName { get; }
	MyCommand[] GetCommandList();
}