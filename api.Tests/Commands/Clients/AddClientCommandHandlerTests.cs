using api.Commands.Clients;
using api.Repositories;
using api.Validators;
using api.ViewModels;
using Moq;

namespace api.Tests.Commands.Clients
{
    public class AddClientCommandHandlerTests : TestBase
    {
        private Mock<IEmailRepository> emailRepository;
        private Mock<IDocumentRepository> documentRepository;
        private AddClientCommandHandler handler;

        public AddClientCommandHandlerTests()
        {
            emailRepository = new Mock<IEmailRepository>();
            emailRepository.Setup(repo => repo.Send(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            documentRepository = new Mock<IDocumentRepository>();
            documentRepository.Setup(repo => repo.SyncDocumentsFromExternalSource(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            handler = new AddClientCommandHandler(dataContext,
                emailRepository.Object,
                documentRepository.Object,
                new ClientValidator());
        }

        [Fact]
        public async void Handle_AddClientCommand_Success()
        {
            // Arrange
            var clientVm = new ClientVm
            {
                Id = "1",
                FirstName = "John",
                LastName = "Cena",
                Email = "test@test.com",
                PhoneNumber = "123"
            };
            var command = new AddClientCommand(clientVm);

            // Act
            var results = await handler.Handle(command, default);

            // Assert
            Assert.Equal("Client successfully created", results.Message);
        }

        [Fact]
        public async void Handle_AddClientCommand_ClientAlreadyExist()
        {
            // Arrange
            var clientVm = new ClientVm
            {
                Id = "xosiosiosdhad",
                FirstName = "John",
                LastName = "Cena",
                Email = "test@test.com",
                PhoneNumber = "123"
            };
            var command = new AddClientCommand(clientVm);

            // Act
            var results = await handler.Handle(command, default);

            // Assert
            Assert.Equal("Client already exist", results.Message);
        }
    }
}
