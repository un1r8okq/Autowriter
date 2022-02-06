using Dapper;

namespace Autowriter.Core.Features.WritingGeneration
{
    public partial class Generate
    {
        public interface IReadSourceMaterial
        {
            IEnumerable<string> GetSources();
        }

        public class Repository : IReadSourceMaterial
        {
            private readonly CoreDbConnection _connection;

            public Repository(CoreDbConnection connection)
            {
                _connection = connection;
            }

            public IEnumerable<string> GetSources() =>
                _connection.Query<string>("SELECT content FROM source_material");
        }
    }
}
