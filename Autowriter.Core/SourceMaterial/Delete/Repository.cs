using Dapper;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Delete
    {
        public interface IDeleteSourceMaterial
        {
            public void DeleteSource(int id);
        }

        public class Repository : IDeleteSourceMaterial
        {
            private readonly CoreDbConnection _connection;

            public Repository(CoreDbConnection connection)
            {
                _connection = connection;
            }

            public void DeleteSource(int id) =>
                _connection.Execute($"DELETE FROM source_material WHERE id = @id", new { id });
        }
    }
}
