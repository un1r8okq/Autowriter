using System.Data;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAutowriterCoreServices(this IServiceCollection services)
        {
            RegisterDbConnection(services);

            Features.SourceMaterial.Count.ConfigureServices(services);
            Features.SourceMaterial.Create.ConfigureServices(services);

            services.AddMediatR(typeof(Features.SourceMaterial.Count));
        }

        private static void RegisterDbConnection(IServiceCollection services)
        {
            const string dbConnectionString = "Data Source=Autowriter.sqlite";
            var sqliteConnection = new SqliteConnection(dbConnectionString);
            services.AddSingleton<IDbConnection>(_ => sqliteConnection);
        }
    }
}
