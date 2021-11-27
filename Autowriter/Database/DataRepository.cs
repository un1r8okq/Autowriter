using Dapper;
using System.Data;

namespace Autowriter.Database
{
    public interface IDataRepository
    {
        public int Execute(string sql, object parameters);

        public IEnumerable<T> Query<T>(string sql);

        public IEnumerable<T> Query<T>(string sql, object parameters);
    }

    public class DataRepository : IDataRepository
    {
        private readonly IDbConnection _connection;

        public DataRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Execute(string sql, object parameters) =>
            _connection.Execute(sql, parameters);

        public IEnumerable<T> Query<T>(string sql) =>
            _connection.Query<T>(sql);

        public IEnumerable<T> Query<T>(string sql, object parameters) =>
            _connection.Query<T>(sql, parameters);
    }
}
