using Climax;
using System;
using System.Reflection;
using System.Security.Policy;

namespace Climax.UnitTest
{
	public enum DatePart
	{
		year, month, day, hour, minute, second
	}
	public partial class SystemCommand
	{
		[Command("clock", description: "Aggiusta l'orologio della macchina.")]
		public class SystemClockCommand
		{
			private string worldClockUrl = "";

			[Option("url", "Indica l'indirizzo del servizio web da cui sincronizzare da internet l'ora")]
			public string WorldClockUrl
			{
				get => worldClockUrl;
				set
				{
					worldClockUrl = value;
					ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
				}
			}

			[CommandInitialize]
			public void ClockInitializer()=>ExecutionStack.AddInitializer(MethodBase.GetCurrentMethod());
			
			[Command("auto", description: "Aggiusta automaticamente l'ora della macchina in base al Time Zone")]
			[DefaultCommand]
			public void FixClockAuto() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

			[Command("add", description: "Aggiunge minuti/ore, giorni all'ora locale della macchina")]
			public void AddToDate(DatePart part, int value) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(), part, value);

			[Command("local", description: "Sincronizza l'ora con il server locale")]
			public void LocalSync() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

			[Command("remote", description: "Sincronizza l'ora con un servizio web di data/ora")]
			public void RemoteSync() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());
		}
	}
}
