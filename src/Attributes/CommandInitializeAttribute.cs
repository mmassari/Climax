using System;

namespace Climax
{
	[AttributeUsage(
		validOn: AttributeTargets.Method,
		AllowMultiple = false,
		Inherited = false)]
	public class CommandInitializeAttribute : Attribute
	{

	}
}
