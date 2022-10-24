using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Climax
{
	internal class CommandOptionProperty : CommandOptionBase
	{
		public PropertyInfo Property { get; }
		public CommandOptionProperty(PropertyInfo property)
		{
			Property = property;

			var att = property.GetCustomAttribute<OptionAttribute>();
			if (att == null)
				throw new InvalidOperationException("The entity miss the Option attribute");
			
			Name = att.NameIsDefined ? "-" + att.Name.ToLower() : "-" + property.Name.ToLower().Replace("option", "");
			Description = att.Description;
			DataType = property.PropertyType;
			Default = property.PropertyType.ToDefault();
		}
		public object GetValue(IDictionary<string, string> args)
		{
			string value = "";
			bool isSpecified = args.Any(c => c.Key == Name);
			if (isSpecified)
				value = args.First(c => c.Key == Name).Value;

			if (DataType == typeof(bool))
				return isSpecified ? true : false;
			else
				return ConvertValue(value);
		}
	}
}
