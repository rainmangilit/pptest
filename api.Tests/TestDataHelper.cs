using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Tests
{
    public class TestDataHelper
    {
        public DataContext GetDataContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: $"Test-{Guid.NewGuid()}");

            var dbContextOptions = builder.Options;
            var dataContext = new DataContext(dbContextOptions);

            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();

            new DataSeeder(dataContext).Seed();

            return dataContext;
        }
    }
}
