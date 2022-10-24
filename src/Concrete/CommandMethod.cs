using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Climax
{
	internal class CommandMethod : CommandBase, ICommandMethod
	{
		public bool IsDefault { get; protected set; }
		public MethodInfo Method { get; }
		public List<ICommandOption> Options { get; }
		public bool HaveArgs => Options.Count > 0;
		public int ParametersCount => Method.GetParameters().Count();
		public CommandMethod(CommandType parent, MethodInfo method) : base(parent)
		{
			var cmd = method.GetCustomAttribute<CommandAttribute>();
			if (cmd == null)
				throw new InvalidOperationException("The entity miss the Command attribute");

			Name = cmd.NameIsDefined ? cmd.Name.ToLower() : method.Name.ToLower().Replace("command", "");
			Description = cmd.Description;
			IsDefault = method.CustomAttributeIsDefined<DefaultCommandAttribute>();
			Method = method;
			Options = method.GetParameters()
			 .Select(c => new CommandOptionParameter(c))
			 .ToList<ICommandOption>();
		}
		public void Execute(IList<string> args)
		{
			foreach (var initializer in GetInitializers())
				initializer.Invoke();

			Method.Invoke(Parent.Instance, GetParameters(args));
		}

		private object[] GetParameters(IList<string> args)
		{
			var parameters = new List<object>();
			for (int i = 0; i < Options.Count; i++)
				parameters.Add(Options[i].ConvertValue(args[i]));
			return parameters.ToArray();
		}

		private IEnumerable<Action> GetInitializers()
		{
			var initializers = new List<Action>();
			ICommandType type = Parent;
			while (type != null)
			{
				if (type.Initializer != null)
					initializers.Insert(0, type.Initializer);
				type = type.Parent;
			}
			return initializers;
		}

	}
}
