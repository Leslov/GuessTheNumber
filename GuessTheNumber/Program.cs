using AutoGuesser;
using AutoGuesser.Guessing;
using NumberGameCore;
using NumberGameCore.BaseStuff;
using NumberGameCore.BaseStuff.Holders;

Console.WriteLine("Guess the number!");
var settings = new NumberGameSettings(4, 12);
Console.WriteLine("Hopelen Hopskops");
Console.WriteLine("Choose a player:");
Console.WriteLine("1) Lena (играет игрок)");
Console.WriteLine("2) Pol (Играет комп)");
Console.WriteLine("3) Dragon (Играет игрок с поддержкой компа)");
var key = Console.ReadKey();
Console.Clear();
switch (key.Key)
{
	case ConsoleKey.D1:
	case ConsoleKey.NumPad1:
		PlayYourself();
		break;
	case ConsoleKey.D2:
	case ConsoleKey.NumPad2:
		LetComputerPlay();
		break;
	case ConsoleKey.D3:
	case ConsoleKey.NumPad3:
		PlayWithComputerHelp();
		break;
}

void LetComputerPlay()
{
	var game = new NumberGame(settings);
	var guesser = new ComputerPlayer(game, new ConsoleWriter());
	while (!game.IsEnded)
	{
		FullGuess moveResult = guesser.NextMove();
		Console.WriteLine(moveResult.GetReadableResult());
	}
}
void PlayYourself()
{
	var game = new NumberGame(settings);
	while (!game.IsWon && !game.IsLose)
	{
		Console.Write($"{game.GuessCount + 1}: ");
		int[] guessed = Console.ReadLine().Select(x => int.Parse(x.ToString())).ToArray();
		Guess guess = SomeGuessHolder.GetByGuessed(guessed);
		var result = game.Guess(guess);
		Console.WriteLine(result);
	}
	if (game.IsLose)
		Console.WriteLine("OMG You lose!");
	if (game.IsWon)
		Console.WriteLine($"GZ, You won! Number is {string.Join("", game.GetAnswer())}");
}

void PlayWithComputerHelp()
{
	var writer = new ConsoleWriter();
	var game = new NumberGame(settings);
	var guesser = new Guesser(writer);

	var inputProcessor = new NumberGameInputProcessor(guesser);
	inputProcessor.WriteCommandsDescription(writer);

	writer.WriteLine("Комп загадал число. Введи четырехзначное число!");
	while (!game.IsWon && !game.IsLose)
	{
		try
		{
			string userInput = writer.ReadLine();
			if (!inputProcessor.ProcessInput(userInput))
			{
				writer.WriteLine($"{game.GuessCount + 1}: {userInput}");
				int[] guessed = userInput.Select(x => int.Parse(x.ToString())).ToArray();
				Guess guess = SomeGuessHolder.GetByGuessed(guessed);
				var result = game.Guess(guess);
				guesser.ApplyGuessResult(new FullGuess(guess,result));
				writer.WriteLine(result.ToString());
			}
		}
		catch (Exception e)
		{
			writer.WriteLine(e.ToString());
		}
	}

	if (game.IsLose)
		Console.WriteLine("OMG You lose!");
	if (game.IsWon)
		Console.WriteLine($"GZ, You won! Number is {string.Join("", game.GetAnswer())}");
}

public class NumberGameInputProcessor
{
	private readonly IUserInputProcessor[] inputProcessors;
	public NumberGameInputProcessor(params IUserInputProcessor[] inputProcessors)
	{
		this.inputProcessors = inputProcessors;
	}

	public void WriteCommandsDescription(IOutputWriter writer)
	{
		writer.WriteLine("Доступные команды:");
		foreach (IUserInputProcessor processor in inputProcessors)
		{
			foreach (var command in processor.GetCommandList())
			{
				writer.WriteLine($"{processor.ProcessorName} {command}");
			}
		}
	}
	public bool ProcessInput(string input)
	{
		char separator = ' ';
		var splittedInput = input.Split(separator);
		if (splittedInput.Length < 2)
			return false;
		string processorName = splittedInput[0];
		string commandName = splittedInput[1];
		string args = string.Join(separator, splittedInput.Skip(2));

		var processor = inputProcessors.First(x => x.ProcessorName == processorName);
		var command = processor.GetCommandList().FirstOrDefault(x => x.Name == commandName) ??
			throw new Exception($"Command {commandName} is not supported");
		command.Invoke(args);
		return true;
	}
}