using System.Data;
using Autowriter.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public interface ICreateSourceMaterial
        {
            public Response.SourceMaterial CreateSource(DateTime createdDateTime, string content);
        }

        public class Repository : ICreateSourceMaterial
        {
            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;

                DbHelpers.EnsureDbIsInitialised(connection);
            }

            public Response.SourceMaterial CreateSource(DateTime created, string content)
            {
                const string query = $"INSERT INTO {DbHelpers.SourceMaterialTableName} " +
                    "(created, content) VALUES (@created, @content) " +
                    "RETURNING id, created, content";
                var parameters = new { created, content };
                var createdSource = _connection.QuerySingle<Response.SourceMaterial>(query, parameters);

                return createdSource;
            }
        }
    }
}
