using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Climax
{
	internal class CommandFlag : CommandOptionBase
	{
		private const string PREFIX = "--";
		public PropertyInfo Property { get; }
		public CommandFlag(PropertyInfo property)
		{
			Property = property;

			var att = property.GetCustomAttribute<FlagAttribute>();
			if (att == null)
				throw new InvalidOperationException("The entity miss the Option attribute");
			
			Name = PREFIX + att.Name.ToLower().Replace(PREFIX,"");
			Description = att.Description;
			DataType = typeof(bool);
			Default = false;
		}
		public bool GetValue(IDictionary<string, bool> args)
		{
			bool isSpecified = args.Any(c => c.Key == Name);
			return isSpecified ? true : false;
		}
	}
}
