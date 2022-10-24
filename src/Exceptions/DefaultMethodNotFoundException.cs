using System;
using System.Runtime.Serialization;

namespace Climax
{
	[Serializable]
	public class DefaultMethodNotFoundException : Exception
	{
		public DefaultMethodNotFoundException()
		{
		}

		public DefaultMethodNotFoundException(string message) : base(message)
		{
		}

		public DefaultMethodNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected DefaultMethodNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}