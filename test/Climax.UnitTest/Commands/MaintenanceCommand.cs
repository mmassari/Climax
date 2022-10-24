using Climax;
using System.Reflection;

namespace Climax.UnitTest
{
	[Command("maint", "Programmi di manutenzione della cassa")]
	public class MaintenanceCommand
	{
		public enum BackupType { Full, Db, Bin }
		[Command("checkupg", "Controlla ed esegue eventuali upgrade da eseguire")]
		public void CheckUpgrade() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());

		[Command("backup", "Esegue il backup del database o eseguibili o entrambi")]
		public void Backup(BackupType type, bool nocopy) => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod(), type, nocopy);

		[Command("queries", "Esegue le query inviate dal server centrale")]
		public void RunQueries() => ExecutionStack.AddMethod(MethodBase.GetCurrentMethod());
	}

}
