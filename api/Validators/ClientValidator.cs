using api.ViewModels;
using FluentValidation;

namespace api.Validators
{
    public class ClientValidator : AbstractValidator<ClientVm>
    {
        public ClientValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotNull();
        }
    }
}
