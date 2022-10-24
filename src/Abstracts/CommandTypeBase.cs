using System;
using System.Collections.Generic;
using System.Linq;

namespace Climax
{
	public abstract class CommandTypeBase : CommandBase, ICommandType
	{
		public object Instance { get; protected set; }
		public Type Type { get; protected set; }
		public List<ICommandMethod> Methods { get; protected set; }
		public IEnumerable<ICommandType> Types { get; protected set; }
		public virtual List<ICommand> Commands => Types.ToList<ICommand>();
		public Action Initializer { get; protected set; }
		public List<ICommandOption> Options { get; protected set; }
		public CommandTypeBase(ICommandType parent) : base(parent)
		{
			Types = new List<ICommandType>();
		}
		public void SetPropertyOptions(IDictionary<string, string> args)
		{
			//Setto il valore delle property
			foreach (var prop in GetProperties())
				if (args.Any(c => c.Key == prop.Name))
					prop.Property.SetValue(Instance, prop.GetValue(args), null);

		}
		private IEnumerable<CommandOptionProperty> GetProperties()
		{
			if (Options != null)
			{
				foreach (var opt in Options.Where(c=>c is CommandOptionProperty))
					yield return opt as CommandOptionProperty;
			}
		}
	}
}
