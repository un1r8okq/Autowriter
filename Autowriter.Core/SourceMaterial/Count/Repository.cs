using System.Data;
using Dapper;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Count
    {
        public interface ICountSourceMaterials
        {
            public int CountSources();
        }

        public class Repository : ICountSourceMaterials
        {
            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;
            }

            public int CountSources() =>
                _connection.QuerySingle<int>($"SELECT COUNT(*) FROM source_material");
        }
    }
}
