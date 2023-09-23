using api.Validators;
using api.ViewModels;

namespace api.Tests.Validators
{
    public class ClientValidatorTests
    {
        private ClientValidator validator = new ClientValidator();

        [Fact]
        public async void Handle_AddClientCommand_Validation()
        {
            // Arrange
            var clientVm = new ClientVm();

            // Act
            var results = await validator.ValidateAsync(clientVm);

            // Assert
            var errors = results.ToDictionary();
            Assert.Contains("'Id' must not be empty.", errors["Id"]);
            Assert.Contains("'First Name' must not be empty.", errors["FirstName"]);
            Assert.Contains("'Last Name' must not be empty.", errors["LastName"]);
            Assert.Contains("'Email' must not be empty.", errors["Email"]);
            Assert.Contains("'Phone Number' must not be empty.", errors["PhoneNumber"]);
        }

        [Fact]
        public async void Handle_AddClientCommand_Validation_Email()
        {
            // Arrange
            var clientVm = new ClientVm
            {
                Id = "1",
                FirstName = "John",
                LastName = "Cena",
                Email = "not email",
                PhoneNumber = "123"
            };

            // Act
            var results = await validator.ValidateAsync(clientVm);

            // Assert
            var errors = results.ToDictionary();
            Assert.Contains("'Email' is not a valid email address.", errors["Email"]);
        }
    }
}
