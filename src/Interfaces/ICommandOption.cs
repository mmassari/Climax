using System;
using System.Collections.Generic;

namespace Climax
{
	public interface ICommandOption : IOption
	{
		object Default { get; }
		Type DataType { get; }
		object ConvertValue(string value);
	}
}