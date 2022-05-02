using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Autowriter.Core.Tests.Architecture
{
    public class CommandAndQueryTests
    {
        private static readonly ArchUnitNET.Domain.Architecture Architecture;
        private static readonly IObjectProvider<Class> CommandsOrQueries;

        static CommandAndQueryTests()
        {
            Architecture = new ArchLoader()
                .LoadAssemblies(typeof(Autowriter.Core.ServiceCollectionExtensions).Assembly)
                .Build();
            CommandsOrQueries = Classes().That()
                .ImplementInterface("MediatR.IRequest", useRegularExpressions: true)
                .And().DoNotHaveNameEndingWith("Handler")
                .As("CommandsOrQueries");
        }

        [Fact]
        public void CommandsOrQueriesShouldBeCalledCommandOrQuery()
        {
            var rule = Classes().That().Are(CommandsOrQueries)
                .Should().HaveName("Command").OrShould().HaveName("Query");

            rule.Check(Architecture);
        }

        [Fact]
        public void ClassesCalledCommandOrQueryShouldBeCommandsOrQueries()
        {
            var rule = Classes().That().HaveName("Command")
                .Or().HaveName("Query")
                .Should().Be(CommandsOrQueries);

            rule.Check(Architecture);
        }

        [Fact]
        public void CommandsAndQueriesShouldBeNested()
        {
            var rule = Classes().That().Are(CommandsOrQueries)
                .Should().BeNested()
                .Because("Nested classes are used to group a features command/query, handler, and response together");

            rule.Check(Architecture);
        }
    }
}
