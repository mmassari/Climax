using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Climax
{
	internal class CommandOptionParameter : CommandOptionBase
	{
		public ParameterInfo Parameter { get; }
		public CommandOptionParameter(ParameterInfo parameter) 
		{
			var att = parameter.GetCustomAttribute<OptionAttribute>();
			if (att != null)
			{
				Name = att.NameIsDefined ? "-" + att.Name.ToLower() : "-" + parameter.Name.ToLower().Replace("option", "");
				Description = att.Description;
			}
			else
			{
				Name = "-" + parameter.Name.ToLower().Replace("option", "");
				Description = "";
			}

			DataType = parameter.ParameterType;
			Default = parameter.ParameterType.ToDefault();
			Parameter = parameter;
		}

	}
}
