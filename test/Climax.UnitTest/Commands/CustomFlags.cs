using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Climax.UnitTest
{
	[CustomFlags]
	public static class CustomFlags
	{
		[Flag("--my","Mio flag personalizzato")]
		public static bool MyFlag { get; set; } = false;
	}
}
