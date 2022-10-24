using Climax;
using System;
using System.Reflection;

namespace Climax.UnitTest
{
	[Command("config", "Configurazione della cassa")]
	public sealed class ConfigCommand
	{
		private int arg = 5;

		[Option("arg","argomento numerico")]
		public int Arg
		{
			get => arg;
			set
			{
				arg = value;
				ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
			}
		}
		[Command("ui", "Esegue il programma di configurazione")]
		[DefaultCommand]
		public void RunUI() =>
			ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

	}
}
