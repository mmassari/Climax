using Climax;
using System;
using System.Reflection;

namespace Climax.UnitTest
{
	[Command("system", "Programmi per la gestione del sistema")]
	public partial class SystemCommand
	{
		private string logFilename;
		[Option("log")]
		public string LogFilename
		{
			get => logFilename;
			set
			{
				ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
				logFilename = value;
			}
		}
		private bool lite = false;
		[Option("lite")]
		public bool Lite
		{
			get => lite;
			set
			{
				lite = value;
				ExecutionStack.AddProperty(MethodBase.GetCurrentMethod().GetPropertyInfo(), value);
			}
		}
		[CommandInitialize]
		public void SystemInitializer() => ExecutionStack.AddInitializer(MethodBase.GetCurrentMethod());

		[Command("check", "Lancia il check del sistema ed invia i log al server centrale")]
		[DefaultCommand]
		public void SystemCheck() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

		[Command("hdd", "Raccoglie dati sui dischi di memoria (funziona solo da sbloccato)")]
		public void CheckHardDrives() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

	}

}
