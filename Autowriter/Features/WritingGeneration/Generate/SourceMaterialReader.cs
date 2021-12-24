using System.Data;
using Dapper;

namespace Autowriter.Features.WritingGeneration.Generate
{
    public interface IReadSourceMaterial
    {
        IEnumerable<string> GetSources();
    }

    public class SourceMaterialReader : IReadSourceMaterial
    {
        private readonly IDbConnection _connection;

        public SourceMaterialReader(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<string> GetSources() =>
            _connection.Query<string>("SELECT content FROM source_material");
    }
}
