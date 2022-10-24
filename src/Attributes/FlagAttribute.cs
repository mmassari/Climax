using System;

namespace Climax
{
	public class FlagAttribute : Attribute
	{
		public string Name { get; }
		public string Description { get; }
		public FlagAttribute(string name, string description="")
		{
			Name = name;
			Description = description;
		}
	}
}
