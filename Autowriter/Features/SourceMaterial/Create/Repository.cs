using System.Data;
using Autowriter.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public interface ICreateSourceMaterial
        {
            public SourceMaterial CreateSource(DateTime createdDateTime, string content);
        }

        public class Repository : ICreateSourceMaterial
        {
            public const string TableName = "source_material";

            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;

                DbHelpers.EnsureDbIsInitialised(connection);
            }

            public SourceMaterial CreateSource(DateTime created, string content)
            {
                const string query = $"INSERT INTO {TableName} " +
                    "(created, content) VALUES (@created, @content) " +
                    "RETURNING id, created, content";
                var parameters = new { created, content };
                var createdSource = _connection.QuerySingle<SourceMaterial>(query, parameters);

                return createdSource;
            }
        }
    }
}
