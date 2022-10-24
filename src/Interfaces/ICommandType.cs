using System;
using System.Collections.Generic;

namespace Climax
{
	public interface ICommandType : ICommand
	{
		object Instance { get; }
		List<ICommandOption> Options { get; }
		IEnumerable<ICommandType> Types { get; }
		List<ICommand> Commands { get; }
		List<ICommandMethod> Methods { get; }
		Action Initializer { get; }
		void SetPropertyOptions(IDictionary<string, string> args);

	}
}