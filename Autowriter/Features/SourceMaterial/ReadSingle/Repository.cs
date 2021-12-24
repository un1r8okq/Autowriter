using System.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public interface IReadSourceMaterial
        {
            public Repository.SourceMaterial? GetSource(int id);
        }

        public class Repository : IReadSourceMaterial
        {
            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;
            }

            public SourceMaterial? GetSource(int id)
            {
                var query = "SELECT id, created, content FROM source_material WHERE id = @id";
                var param = new { id };
                return _connection.QuerySingleOrDefault<SourceMaterial>(query, param);
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
