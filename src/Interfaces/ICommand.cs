namespace Climax
{
	public interface ICommand
	{
		ICommandType Parent { get; }
		string Description { get; }
		string Name { get; }
	}
}