using api.Data;

namespace api.Tests
{
    public class TestBase
    {
        protected readonly DataContext dataContext;

        public TestBase()
        {
            var testHelper = new TestDataHelper();
            dataContext = testHelper.GetDataContext();
        }
    }
}
