using api.Data;
using api.Mappers;
using api.Models;
using api.Repositories;
using api.ViewModels;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Commands.Clients
{
    public record AddClientCommand(ClientVm Client) : IRequest<Result>;

    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, Result>
    {
        private readonly DataContext dataContext;
        private readonly IEmailRepository emailRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly Mapper mapper;
        private readonly IValidator<ClientVm> validator;

        public AddClientCommandHandler(DataContext dataContext,
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

        public async Task<Result> Handle(AddClientCommand request, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(request.Client);
            if (!validationResult.IsValid)
            {
                return new Result(validationResult.ToDictionary());
            }

            if (await dataContext.Clients.AnyAsync(x => x.Id == request.Client.Id, cancellationToken: cancellationToken))
            {
                return new Result("Client already exist");
            }

            var client = mapper.Map<Client>(request.Client);
            await dataContext.AddAsync(client, cancellationToken);
            await emailRepository.Send(request.Client.Email, "Hi there - welcome to my Carepatron portal.");
            await documentRepository.SyncDocumentsFromExternalSource(request.Client.Email);

            await dataContext.SaveChangesAsync(cancellationToken);

            return new Result("Client successfully created", new { client.Id });
        }
    }
}
