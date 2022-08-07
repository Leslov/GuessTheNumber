namespace Extras.Something_Useful;

public class MyCommand
{
	private readonly Action<string> action;

	public MyCommand(string name, string description, Action<string> action)
	{
		this.Name = name;
		this.Description = description;
		this.action = action;
	}

	public string Description { get; }

	public string Name { get; }

	public void Invoke(string args) => action.Invoke(args);
	public override string ToString()
	{
		return $"{Name}\t - {Description}";
	}
}