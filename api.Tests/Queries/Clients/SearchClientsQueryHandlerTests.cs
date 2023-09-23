using api.Queries.Clients;

namespace api.Tests.Queries.Clients
{
    public class SearchClientsQueryHandlerTests : TestBase
    {
        private SearchClientsQueryHandler handler;

        public SearchClientsQueryHandlerTests()
        {
            handler = new SearchClientsQueryHandler(dataContext);
        }

        [Fact]
        public async void Handle_SearchClientQuery_Found()
        {
            // Arrange
            var query = new SearchClientQuery("John");

            // Act
            var results = await handler.Handle(query, default);

            // Assert
            Assert.Single(results);
            Assert.Contains(results, x => x.Id == "xosiosiosdhad");
        }

        [Fact]
        public async void Handle_SearchClientQuery_NotFound()
        {
            // Arrange
            var query = new SearchClientQuery("James");

            // Act
            var results = await handler.Handle(query, default);

            // Assert
            Assert.Empty(results);
        }
    }
}
