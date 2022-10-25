using Climax;
using FluentAssertions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Xunit;

namespace Climax.UnitTest
{
	public class Execute_Tests
	{
		public Execute_Tests()
		{
			AssemblyUtilities.SetEntryAssembly();
			ExecutionStack.Initialize();
		}

		[Fact]
		public void Execute_Explicitly_With_Parameters_Should_Pass()
		{
			//Arrange
			string[] args = new string[] { "system", "clock", "add", "hour", "-5" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			//Act
			cli.Execute();

			//Assert
			ExecutionStack.GetExecutedMethod()
				.Should()
				.NotBeNull();
			ExecutionStack.GetExecutedMethod().Name
				.Should()
				.Be(nameof(SystemCommand.SystemClockCommand.AddToDate));
			ExecutionStack.GetParameterValue<DatePart>(0)
				.Should()
				.Be(DatePart.hour);
			ExecutionStack.GetParameterValue<int>(1)
				.Should()
				.Be(-5);
		}

		[Fact]
		public void Execute_Implicitly_When_Type_Have_DefaultCommand_Should_Pass()
		{
			string[] args = new string[] { "system", "clock" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Execute();
			ExecutionStack.GetExecutedMethod()
				.Should()
				.NotBeNull();
			ExecutionStack.GetExecutedMethod().Name
				.Should()
				.Be(nameof(SystemCommand.SystemClockCommand.FixClockAuto));
		}

		[Fact]
		public void Execute_Nested_Should_Call_All_Initializers()
		{
			string[] args = new string[] { "system", "clock", "local" };
			var cli = new CliRunner(args)				
				.WithInitialize(() => ExecutionStack.AddRootInitializer(MethodBase.GetCurrentMethod()))
				.OnError(OnErrorBehavior.ThrowError);

			cli.Execute();

			ExecutionStack.GetInitializers().Count.Should().Be(3);
			ExecutionStack.GetInitializers()[0].Name.Should().Be("RootInitializer");
			ExecutionStack.GetInitializers()[1].Name.Should().Be("SystemInitializer");
			ExecutionStack.GetInitializers()[2].Name.Should().Be("ClockInitializer");
		}

		[Fact]
		public void Execute_Nested_Should_Set_All_Properties()
		{
			string[] args = new string[] {
				"system", "clock", "local",
				"-url", "www.google.com",
				"-log",@"""c:\temp\test.log""",
				"-lite"
			};
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Execute();

			ExecutionStack.GetPropertyValue<string>(nameof(SystemCommand.SystemClockCommand.WorldClockUrl))
				.Should()
				.Be("www.google.com");
			ExecutionStack.GetPropertyValue<string>(nameof(SystemCommand.LogFilename))
				.Should()
				.Be(@"""c:\temp\test.log""");
			ExecutionStack.GetPropertyValue<bool>(nameof(SystemCommand.Lite))
				.Should()
				.Be(true);
		}

		[Fact]
		public void Execute_Should_Set_Properties_And_Flags()
		{
			string[] args = new string[] {
				"system", "clock", "local",
				"-url", "www.google.com",
				"-log",@"""c:\temp\test.log""",
				"--help","--silent"
			};
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Execute();

			ExecutionStack.GetPropertyValue<string>(nameof(SystemCommand.SystemClockCommand.WorldClockUrl))
				.Should()
				.Be("www.google.com");
			ExecutionStack.GetPropertyValue<string>(nameof(SystemCommand.LogFilename))
				.Should()
				.Be(@"""c:\temp\test.log""");
			Flags.ShowHelp.Should().BeTrue();
			Flags.SilentMode.Should().BeTrue();
		}

		[Fact]
		public void Execute_Should_Set_Builtin_And_Custom_Flags()
		{
			string[] args = new string[] {
				"system", "clock", "local",
				"--help","--silent",
				"--verbose", "--my"
			};
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Execute();

			Flags.ShowHelp.Should().BeTrue();
			Flags.SilentMode.Should().BeTrue();
			Flags.Verbose.Should().BeTrue();
			CustomFlags.MyFlag.Should().BeTrue();
		}

		[Fact]
		public void Execute_Nested_Should_Set_Properties_And_Parameters()
		{
			//Arrange
			string[] args = new string[] {
				"system", "clock", "add", "hour", "-5",
				"-url", "www.google.com",
				"-lite"
			};
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			//Act
			cli.Execute();

			//Assert
			ExecutionStack.GetExecutedMethod()
				.Should()
				.NotBeNull();
			ExecutionStack.GetExecutedMethod().Name
				.Should()
				.Be(nameof(SystemCommand.SystemClockCommand.AddToDate));
			ExecutionStack.GetParameterValue<DatePart>(0)
				.Should()
				.Be(DatePart.hour);
			ExecutionStack.GetParameterValue<int>(1)
				.Should()
				.Be(-5);
			ExecutionStack.GetPropertyValue<string>(nameof(SystemCommand.SystemClockCommand.WorldClockUrl))
				.Should()
				.Be("www.google.com");
			ExecutionStack.GetPropertyValue<bool>(nameof(SystemCommand.Lite))
				.Should()
				.Be(true);
		}

		[Fact]
		public void Execute_Explicitly_With_MissingParameter_Should_Throw_MissingParameters()
		{
			string[] args = new string[] { "system", "clock", "add", "hour" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);
			cli.Invoking(y => y.Execute())
				 .ShouldThrow<MissingParametersException>()
				 .WithMessage($"The method has 2 mandatory parameters");
		}

		[Fact]
		public void Execute_Explicitly_With_InvalidParameter_Should_Throw_InvalidParameter()
		{
			string[] args = new string[] { "system", "clock", "add", "hour", "a" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Invoking(y => y.Execute())
				 .ShouldThrow<InvalidParameterException>()
				 .WithMessage($"A parameter is not valid");
		}

		[Fact]
		public void Execute_Explicitly_With_InvalidCommand_Should_Throw_InvalidCommand()
		{
			string[] args = new string[] { "system", "clock", "remove", "hour", "a" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Invoking(y => y.Execute())
				 .ShouldThrow<InvalidCommandException>()
				 .WithMessage($"Command <remove> is not a valid command name");
		}

		[Fact]
		public void Execute_Implicitly_When_Type_HaveNo_DefaultCommand_Should_Throw_DefaultMethodNotFound()
		{
			string[] args = new string[] { "maint" };
			var cli = new CliRunner(args)
				.OnError(OnErrorBehavior.ThrowError);

			cli.Invoking(y => y.Execute())
				 .ShouldThrow<DefaultMethodNotFoundException>()
				 .WithMessage($"Default method not found for the command [maint]");
		}
	}
}