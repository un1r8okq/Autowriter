using System.Data;
using Autowriter.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Delete
    {
        public interface IDeleteSourceMaterial
        {
            public void DeleteSource(int id);
        }

        public class Repository : IDeleteSourceMaterial
        {
            private readonly IDbConnection _connection;

            public Repository(IDbConnection connection)
            {
                _connection = connection;

                DbHelpers.EnsureDbIsInitialised(connection);
            }

            public void DeleteSource(int id) =>
                _connection
                    .Execute($"DELETE FROM {DbHelpers.SourceMaterialTableName} WHERE id = @id", new { id });
        }
    }
}
