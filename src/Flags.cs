using Climax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Climax
{
	public static class Flags
	{
		[Flag("--help")]
		public static bool ShowHelp { get; set; } = false;

		[Flag("--verbose")]
		public static bool Verbose { get; set; } = false;

		[Flag("--silent")]
		public static bool SilentMode { get; set; } = false;

	}
}
