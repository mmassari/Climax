using System;

namespace Climax
{
	[AttributeUsage(
		validOn: AttributeTargets.Class, 
		AllowMultiple = false,
		Inherited = false)]
	public class CustomFlagsAttribute : Attribute
	{

	}
}
