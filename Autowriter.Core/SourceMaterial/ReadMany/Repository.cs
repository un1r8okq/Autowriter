using Dapper;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public interface IReadSourceMaterials
        {
            public IEnumerable<Repository.SourceMaterial> GetSources();
        }

        public class Repository : IReadSourceMaterials
        {
            private readonly CoreDbConnection _connection;

            public Repository(CoreDbConnection connection)
            {
                _connection = connection;
            }

            public IEnumerable<SourceMaterial> GetSources() =>
                _connection
                    .Query<SourceMaterial>($"SELECT id, created, content FROM source_material")
                    .OrderByDescending(model => model.Created)
                    .ToList();

            public class SourceMaterial
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}