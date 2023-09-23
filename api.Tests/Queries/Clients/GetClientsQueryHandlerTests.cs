using api.Queries.Clients;

namespace api.Tests.Queries.Clients
{
    public class GetClientsQueryHandlerTests : TestBase
    {
        private GetClientsQueryHandler handler;

        public GetClientsQueryHandlerTests()
        {
            handler = new GetClientsQueryHandler(dataContext);
        }

        [Fact]
        public async void Handle_GetClientsQuery()
        {
            // Arrange
            var query = new GetClientsQuery();

            // Act
            var results = await handler.Handle(query, default);

            // Assert
            Assert.Single(results);
            Assert.Contains(results, x => x.Id == "xosiosiosdhad");
        }
    }
}
