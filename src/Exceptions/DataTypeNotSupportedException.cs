using System;
using System.Runtime.Serialization;

namespace Climax
{
	[Serializable]
	public class DataTypeNotSupportedException : Exception
	{
		private string name;
		private Type dataType;

		public DataTypeNotSupportedException()
		{
		}

		public DataTypeNotSupportedException(string message) : base(message)
		{
		}

		public DataTypeNotSupportedException(string name, Type dataType)
		{
			this.name = name;
			this.dataType = dataType;
		}

		public DataTypeNotSupportedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected DataTypeNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}