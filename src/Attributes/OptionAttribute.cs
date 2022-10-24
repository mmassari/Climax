using System;

namespace Climax
{
	[AttributeUsage(
		validOn: AttributeTargets.Property | AttributeTargets.Parameter,
		AllowMultiple = false,
		Inherited = false)]
	public class OptionAttribute : Attribute
	{
		public string Name { get; }
		public string Description { get; }
		public bool NameIsDefined => string.IsNullOrWhiteSpace(Name) ? false : true;

		public OptionAttribute(string name="", string description="")
		{
			Name = name;
			Description = description;
		}
	}

}
