using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Climax.UnitTest
{
	public class Help_Tests
	{
		public Help_Tests()
		{
			AssemblyUtilities.SetEntryAssembly();
			ExecutionStack.Initialize();
		}

		[Fact]
		public void Execute_Should_DisplayFirstLevelHelp_When_ProvidedHelpFlag()
		{
			//Arrange
			string[] args = new string[] { "--help" };
			var cli = new CliRunner(args)
				.WithName("Product")
				.WithDescription("Product Description")
				.OnError(OnErrorBehavior.DisplayHelp);
			StringBuilder builder = new StringBuilder();
			TextWriter writer = new StringWriter(builder);
			Console.SetOut(writer);

			//Act
			cli.Execute();

			//Assert
			var lines = builder.ToString().Split(new string[] { Environment.NewLine}, StringSplitOptions.None);
			lines.Should().NotBeNull();
			lines[0].Should().Be("Product");
			lines[1].Should().Be("Product Description");
			lines[2].Should().Be($"{Path.GetFileName(Assembly.GetEntryAssembly().Location)} [Command] [SubCommand] [options]");
			lines[3].Should().BeEmpty();
			lines[4].Should().Be("Examples:");
			lines[5].Should().BeEmpty();
			lines[6].Should().Be("Commands:");
		}
	}
}
