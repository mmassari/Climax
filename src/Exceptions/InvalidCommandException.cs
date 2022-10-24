using System;
using System.Runtime.Serialization;

namespace Climax
{
	[Serializable]
	public class InvalidCommandException : Exception
	{
		public string CommandName { get; private set; }

		public InvalidCommandException()
		{
		}

		public InvalidCommandException(string commandName) : base($"Command <{commandName}> is not a valid command name")
		{
			CommandName = commandName;
		}

		public InvalidCommandException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}