using Climax;
using System.Reflection;

namespace Climax.UnitTest
{
	public partial class SystemCommand
	{
		[Command("rebooter", "Programma che si occupa delle operazioni di sblocco/blocco della macchina")]
		public class SystemRebooterCommand
		{

			[Command("auto",description: "Sblocca la macchina, svolge delle operazioni a macchina sbloccata e poi si riblocca in automatico")]
			[DefaultCommand]
			public void LockAndUnlockMachine()=>ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

			[Command("daylight", description: "Sblocca la macchina ed attende il cambio orario e poi si riblocca in automatico")]
			public void DaylightChange() =>ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

			[Command("unlock", description: "Sblocca la macchina, se è bloccata, e la lascia sbloccata")]
			public void UnlockMachine() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

			[Command("lock", description: "Blocca la macchina se la trova sbloccata.")]
			public void LockMachine() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());
		}
	}
}
