using System;
using System.Collections.Generic;
using System.Linq;

namespace Climax
{
	internal abstract class CommandOptionBase : OptionBase, ICommandOption
	{
		public object Default { get; protected set; }
		public Type DataType { get; protected set; }

		public object ConvertValue(string value)
		{
			try
			{

				if (DataType == typeof(bool))
					return Convert.ToBoolean(value);
				else if (DataType == typeof(string))
					return value;
				else if (DataType == typeof(short))
					return Convert.ToInt16(value);
				else if (DataType == typeof(int))
					return Convert.ToInt32(value);
				else if (DataType == typeof(long))
					return Convert.ToInt64(value);
				else if (DataType == typeof(DateTime))
					return DateTime.Parse(value);
				else if (DataType == typeof(TimeSpan))
					return TimeSpan.Parse(value);
				else if (DataType.IsEnum)
					return Enum.Parse(DataType, value);
				else
					throw new DataTypeNotSupportedException(Name, DataType);
			}
			catch (Exception ex)
			{
				var t = ex.GetType();
				throw new InvalidParameterException("A parameter is not valid");
			}
		}
	}
}
