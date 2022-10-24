using Climax;
using System.Reflection;

namespace Climax.UnitTest
{
	[Command("sched", "Gestione delle schedulazioni windows TIE")]
	public class SchedCommand
	{
		[Command("sync", "Sincronizza le schedulazioni dal db e le crea sul sistema")]
		public void Sync(bool force = false) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(),force);

		[Command("fix", "Verifica e sistema le schedulazioni installate")]
		public void Fix(int taskId) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(),taskId);

		[Command("show", "Visualizza tutte le schedulazioni TIE installate")]
		public void Show() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

		[Command("run", "Esegue una schedulazione come se fosse avviata dallo scheduler")]
		public void Run(int taskId) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(),taskId);
	}

}
