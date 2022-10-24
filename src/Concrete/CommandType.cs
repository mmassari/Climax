using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Climax
{
	internal class CommandType : CommandTypeBase
	{
		public override List<ICommand> Commands => base.Commands.Concat(Methods).ToList();
		public CommandType(ICommandType parent, Type type) : base(parent)
		{
			var att = type.GetCustomAttribute<CommandAttribute>();
			if (att == null)
				throw new InvalidOperationException("The entity miss the Command attribute");

			Name = att.NameIsDefined ? att.Name.ToLower() : type.Name.ToLower().Replace("command", "");
			Description = att.Description;
			Instance = Activator.CreateInstance(type);
			Type = type;
			Types = type.GetNestedTypes(BindingFlags.Public | BindingFlags.Instance)
				.Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
				.Select(c => (ICommandType)new CommandType(this, c)).ToList();

			Options = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				 .Where(m => m.GetCustomAttributes(typeof(OptionAttribute), false).Length > 0)
				 .Select(c => new CommandOptionProperty(c))
				 .ToList<ICommandOption>();

			var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
					 .FirstOrDefault(m => m.GetCustomAttributes(typeof(CommandInitializeAttribute), false).Length > 0);
			if (method != null)
				Initializer = (Action)Delegate.CreateDelegate(typeof(Action), Instance, method);

			Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				 .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
				 .Select(c => (ICommandMethod)new CommandMethod(this, c))
				 .ToList();
		}
	}
}
