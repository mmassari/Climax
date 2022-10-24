using System;
using System.Runtime.Serialization;

namespace Climax
{
	[Serializable]
	public class MissingParametersException : Exception
	{
		public MissingParametersException()
		{
		}

		public MissingParametersException(string message) : base(message)
		{
		}

		public MissingParametersException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MissingParametersException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}