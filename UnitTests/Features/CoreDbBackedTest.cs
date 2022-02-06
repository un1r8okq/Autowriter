using Autowriter.Core;

namespace UnitTests.Features
{
    public class CoreDbBackedTest
    {
        protected readonly CoreDbConnection _conn;

        public CoreDbBackedTest()
        {
            _conn = new CoreDbConnection("Data Source=:memory:");
        }
    }
}
