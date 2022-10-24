using System.Collections.Generic;
using System.Reflection;

namespace Climax.UnitTest
{
	public class StackItemMethod : StackItemInitializer
	{
		public override StackItemType ItemType => StackItemType.Method;
		public override string Fullname => $"{Method.DeclaringType.Name}>{Method.Name}()";
		public Dictionary<ParameterInfo, object> Parameters { get; set; }
		public StackItemMethod(MethodBase method, params object[] parameters) : base(method)
		{
			Method = method;
			Parameters = new Dictionary<ParameterInfo, object>();
			var x = 0;
			foreach (var p in method.GetParameters())
			{
				Parameters.Add(p, parameters[x]);
				x++;
			}
		}
	}
}