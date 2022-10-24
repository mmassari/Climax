using System.Reflection;

namespace Climax
{

	public abstract class CommandBase : ICommand
	{
		public ICommandType Parent { get; }
		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public CommandBase(ICommandType parent)
		{
			Parent = parent;
		}
	}
}
