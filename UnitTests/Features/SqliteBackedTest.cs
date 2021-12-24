using System.Data;
using Autowriter;

namespace UnitTests.Features
{
    public class SqliteBackedTest
    {
        protected readonly IDbConnection _conn;

        public SqliteBackedTest()
        {
            _conn = DbConnectionFactory.Build("Data Source=:memory:");
        }
    }
}
