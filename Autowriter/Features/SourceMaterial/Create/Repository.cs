using System.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public interface ICreateSourceMaterial
        {
            public Repository.SourceMaterial CreateSource(DateTime createdDateTime, string content);
        }

        public class Repository : ICreateSourceMaterial
        {
            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;
            }

            public SourceMaterial CreateSource(DateTime created, string content)
            {
                const string query = $"INSERT INTO source_material " +
                    "(created, content) VALUES (@created, @content) " +
                    "RETURNING id, created, content";
                var parameters = new { created, content };
                var createdSource = _connection.QuerySingle<SourceMaterial>(query, parameters);

                return createdSource;
            }

            public class SourceMaterial
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
