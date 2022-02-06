using Dapper;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public interface IReadSourceMaterial
        {
            public Repository.SourceMaterial? GetSource(int id);
        }

        public class Repository : IReadSourceMaterial
        {
            private readonly CoreDbConnection _connection;

            public Repository(CoreDbConnection connection)
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
