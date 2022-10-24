using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Climax
{
	public class CliRunner : CommandTypeBase
	{
		private IDictionary<string, string> OptionArguments { get; } = new Dictionary<string, string>();
		private List<string> CommandArguments { get; } = new List<string>();
		public Assembly ScanAssembly { get; private set; }
		public string[] Arguments { get; }
		public string Executable { get; private set; }
		public List<string> Examples { get; private set; }
		public bool PrintHelpOnError { get; private set; }
		internal IEnumerable<CommandFlag> CommandFlags { get; private set; }

		public CliRunner(params string[] args) : base(null)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			ScanAssembly = Assembly.GetEntryAssembly();
			if (Assembly.GetEntryAssembly() != null)
				Executable = Path.GetFileName(
					Assembly.GetEntryAssembly().Location);
			Arguments = args;
			PrintHelpOnError = true;
		}

		public CliRunner WithName(string programName)
		{
			Name = programName;
			return this;
		}
		public CliRunner WithDescription(string description)
		{
			Description = description;
			return this;
		}
		public CliRunner WithAssembly(Assembly assembly)
		{
			ScanAssembly = assembly;
			return this;
		}
		public CliRunner WithExecutable(string executable)
		{
			Executable = executable;
			return this;
		}
		public CliRunner WithExamples(params string[] examples)
		{
			Examples = examples.ToList();
			return this;
		}
		public CliRunner WithInitialize(Action initializeMethod)
		{
			Initializer = initializeMethod;
			return this;
		}
		public CliRunner ThrowOnError()
		{
			PrintHelpOnError = false;
			return this;
		}

		public void Execute()
		{
			CommandFlags = CollectFlags();
			Types = CollectCommands();
			if (Commands.Count() == 0)
				throw new InvalidProgramException("There are no commands defined in the specified assembly");

			CollectArguments();
			if (CommandArguments.Count == 0)
				PrintHelp();

			try
			{
				var m = GetExecutingMethod(this, 0);
				ICommandType type = m.Method.Parent;
				while (type != null)
				{
					type.SetPropertyOptions(OptionArguments);
					type = type.Parent;
				}
				m.Method.Execute(m.Args);
			}
			catch (Exception ex)
			{
				if (PrintHelpOnError)
				{
					TextWriter errorWriter = Console.Error;
					errorWriter.WriteLine($"Error - {ex.Message}\n");
					errorWriter.WriteLine(GetHelp());
					return;
				}
				throw;

			}
		}



		private void CollectArguments()
		{
			var regex = new Regex("[-][a-zA-Z]+");
			var collectCommands = true;
			for (int x = 0; x < Arguments.Length; x++)
			{
				var arg = Arguments[x].ToLower().Trim();
				if (string.IsNullOrWhiteSpace(arg))
					continue;

				var f = CommandFlags.FirstOrDefault(c => c.Name == arg);
				if (f != null)
				{
					f.Property.SetValue(null, true, null);
					continue;
				}

				if (!collectCommands || regex.IsMatch(arg))
				{
					collectCommands = false;
					if (x == Arguments.Length - 1 || regex.IsMatch(Arguments[x + 1]))
						OptionArguments.Add(arg, "");
					else
					{
						x++;
						OptionArguments.Add(arg, Arguments[x]);
					}

				}
				else
					CommandArguments.Add(arg);
			}
		}
		private class CmdArgs
		{
			public ICommandMethod Method { get; set; }
			public IList<string> Args { get; set; } = new List<string>();
			public CmdArgs(ICommandMethod method, IList<string> args)
			{
				Method = method;
				Args = args;
			}
		}
		/// <summary>
		/// Restituisce il metodo da eseguire in base agli argomenti
		/// </summary>
		/// <param name="commands"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCommandException"></exception>
		/// <exception cref="DefaultMethodNotFoundException"></exception>
		private CmdArgs GetExecutingMethod(ICommandType type, int index)
		{
			var arg = CommandArguments[index].Trim().ToLower();
			var cmd = type.Commands.FirstOrDefault(c => c.Name == arg);
			if (cmd is null)
				throw new InvalidCommandException($"The command [{arg}] is not valid");

			if (cmd is CommandMethod)
				return GetMethodArgs(cmd as CommandMethod, index);

			//Questo parametro è un command di tipo classe
			type = cmd as CommandType;

			//Se questo è l'ultimo argomento vedo se ha un metodo di default			
			if (CommandArguments.Count == index + 1)
			{
				var m = type.Methods.FirstOrDefault(c => c.IsDefault);
				if (m is null)
					throw new DefaultMethodNotFoundException(
						$"Default method not found for the command [{arg}]");

				return GetMethodArgs(m, index);
			}

			//Se il parametro successivo è un command valido
			arg = CommandArguments[index + 1].Trim().ToLower();
			if (type.Commands.Any(c => c.Name == arg))
			{
				index++;
				return GetExecutingMethod(type, index++);
			}

			throw new InvalidCommandException(arg);
		}

		private CmdArgs GetMethodArgs(ICommandMethod command, int index)
		{
			if (command.HaveArgs && CommandArguments.Count > index + 1)
			{
				if (command.ParametersCount != CommandArguments.Skip(index + 1).Count())
					throw new MissingParametersException($"The method has {command.ParametersCount} mandatory parameters");

				string[] args = new string[CommandArguments.Count - index - 1];
				Array.Copy(CommandArguments.ToArray(), index + 1, args, 0, 2);
				return new CmdArgs(command, args);
			}
			return new CmdArgs(command, new List<string>());
		}
		private void PrintHelp()
		{
			Console.WriteLine(Name);
			Console.WriteLine(Description);
			Console.WriteLine($"{Executable} [Command] [SubCommand] [options]");
			Console.WriteLine();
			Console.WriteLine("Examples:");
			foreach (var example in Examples)
				Console.WriteLine($"{Executable} {example}");

			Console.WriteLine();
			Console.WriteLine("Commands:");
			foreach (var cmd in Commands)
				Console.WriteLine($"  {cmd.Name.PadRight(12, ' ')} {cmd.Description}");

			Console.WriteLine();
		}
		private string GetHelp()
		{
			return $"{Name}\n{Description}\nUsage: {Executable} [Command] [SubCommand] [options]\n\n" +
					 $"Examples:\n{string.Join("\n", Examples.Select(c => $"{Executable} {c}"))}\n\n" +
					 $"Commands:\n{string.Join("\n", Commands.Select(c => $"  {c.Name.PadRight(12, ' ')} {c.Description}"))}\n\n";
		}
		private IEnumerable<ICommandType> CollectCommands()
		{
			foreach (var type in ScanAssembly.GetDecoratedTypes<CommandAttribute>())
				yield return new CommandType(this, type);
		}
		private IEnumerable<CommandFlag> CollectFlags()
		{
			var ret = new List<CommandFlag>();
			foreach (var property in Assembly.GetExecutingAssembly()
											.GetDecoratedStaticProperties<FlagAttribute>(nameof(Flags)))
				ret.Add(new CommandFlag(property));

			foreach (var prop in ScanAssembly
					  .GetDecoratedStaticProperties<CustomFlagsAttribute, FlagAttribute>())
				ret.Add(new CommandFlag(prop));

			return ret;
		}
	}
}
