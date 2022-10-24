using System.Collections.Generic;
using System.Reflection;

namespace Climax
{
	public interface ICommandMethod : ICommand
	{
		bool HaveArgs { get; }
		bool IsDefault { get; }
		MethodInfo Method { get; }
		List<ICommandOption> Options { get; }
		int ParametersCount { get; }

		void Execute(IList<string> args);
	}
}