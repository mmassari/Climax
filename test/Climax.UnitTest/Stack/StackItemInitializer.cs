using System.Reflection;

namespace Climax.UnitTest
{
	public class StackItemInitializer : StackItem
	{
		public override StackItemType ItemType => StackItemType.Initializer;
		public override string Fullname => $"{Method.DeclaringType.Name}>{Name}()";
		public MethodBase Method { get; set; }
		public StackItemInitializer(MethodBase method, string name) : base(name)
		{
			Method = method;
		}
		public StackItemInitializer(MethodBase method) : base(method.Name)
		{
			Method = method;
		}
	}
}