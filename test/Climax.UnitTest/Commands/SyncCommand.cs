using Climax;
using System;
using System.Reflection;

namespace Climax.UnitTest
{

	[Command("sync", "Programmi per il riallieamento dei dati")]
	public class SyncCommand
	{
		private bool liteMode = false;
		private bool shrink = false;

		[Option("lite", "Modalità di importazione light")]
		public bool LiteMode
		{
			get => liteMode;
			set
			{
				liteMode = value;
				ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
			}
		}
		[Option("shrink", "Indica se deve fare lo shrink del db alla fine")]
		public bool Shrink
		{
			get => shrink;
			set
			{
				shrink = value;
				ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
			}
		}

		[CommandInitialize]
		public void Init() => ExecutionStack.AddInitializer(MethodBase.GetCurrentMethod());

		[Command("export", "Esporta i dati in CSV e li rende disponibili al trasferimento")]
		public void Export() =>ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

		[Command("import", "Importa i file scaricati dal server sul database")]
		public void Import(bool lite) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(), lite);

		[Command("transfer", "Trasferisce i dati esportati e scarica i dati dal server")]
		public void Transfer(string name)=> ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(),name);
	}
}
