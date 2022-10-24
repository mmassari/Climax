using System;

namespace Climax
{
	[AttributeUsage(
		validOn: AttributeTargets.Class |  AttributeTargets.Method, 
		AllowMultiple = false, 
		Inherited = false)]
	public class CommandAttribute : Attribute
	{
		public string Name { get; }
		public string Description { get; }
		public bool NameIsDefined => string.IsNullOrWhiteSpace(Name) ? false : true;
		public CommandAttribute(string name = "", string description = "")
		{
			Name = name;
			Description = description;
		}
	}
}
