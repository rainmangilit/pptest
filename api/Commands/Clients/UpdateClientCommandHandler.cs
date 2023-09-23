using api.Data;
using api.Mappers;
using api.Repositories;
using api.ViewModels;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Commands.Clients
{
    public record UpdateClientCommand(ClientVm Client) : IRequest<Result?>;

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result?>
    {
        private readonly DataContext dataContext;
        private readonly IEmailRepository emailRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly Mapper mapper;
        private readonly IValidator<ClientVm> validator;

        public UpdateClientCommandHandler(DataContext dataContext,
            IEmailRepository emailRepository,
            IDocumentRepository documentRepository,
            IValidator<ClientVm> validator)
        {
            this.dataContext = dataContext;
            this.emailRepository = emailRepository;
            this.documentRepository = documentRepository;
            mapper = MapperConfig.Initialize();
            this.validator = validator;
        }

        public async Task<Result?> Handle(UpdateClientCommand request, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(request.Client);
            if (!validationResult.IsValid)
            {
                return new Result(validationResult.ToDictionary());
            }

            var existingClient = await dataContext.Clients.FirstOrDefaultAsync(x => x.Id == request.Client.Id, cancellationToken: cancellationToken);
            if (existingClient == null)
            {
                return new Result("Client not found");
            }

            if (existingClient.Email != request.Client.Email)
            {
                await emailRepository.Send(request.Client.Email, "Hi there - welcome to my Carepatron portal.");
                await documentRepository.SyncDocumentsFromExternalSource(request.Client.Email);
            }

            mapper.Map(request.Client, existingClient);

            await dataContext.SaveChangesAsync(cancellationToken);

            return new Result("Client successfully updated");
        }
    }
}
