using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Autowriter.Core.Tests.Architecture
{
    public class HandlerTests
    {
        private static readonly ArchUnitNET.Domain.Architecture Architecture;
        private static readonly IObjectProvider<Class> Handlers;
        private static readonly IObjectProvider<Class> FeatureClasses;

        static HandlerTests()
        {
            Architecture = new ArchLoader()
                .LoadAssemblies(typeof(Autowriter.Core.ServiceCollectionExtensions).Assembly)
                .Build();
            Handlers = Classes().That()
                .AreAssignableTo("MediatR.RequestHandler", useRegularExpressions: true).Or()
                .ImplementInterface("MediatR.IRequestHandler", useRegularExpressions: true)
                .As("Handlers");
            FeatureClasses = Classes().That().ResideInNamespace("Autowriter.Core.Features");
        }

        [Fact]
        public void HandlersShouldBeCalledHandler()
        {
            var rule = Classes().That().Are(Handlers)
                .Should().HaveName("Handler");

            rule.Check(Architecture);
        }

        [Fact]
        public void ClassesCalledHandlerShouldBeHandlers()
        {
            var rule = Classes().That().HaveNameEndingWith("Handler")
                .Should().Be(Handlers);

            rule.Check(Architecture);
        }

        [Fact]
        public void HandlersShouldBeNested()
        {
            var rule = Classes().That().Are(Handlers)
                .Should().BeNested()
                .Because("Nested classes are used to group a features command/query, handler, and response together");

            rule.Check(Architecture);
        }

        [Fact]
        public void HandlersShouldNotBeReferencedDirectly()
        {
            var rule = Classes().That().Are(FeatureClasses)
                .And().AreNot(Handlers)
                .Should().NotDependOnAny(Handlers)
                .Because("Handlers are supposed to be abstract and resolved using the Moq library");

            rule.Check(Architecture);
        }
    }
}
