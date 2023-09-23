using api.Commands.Clients;
using api.Queries.Clients;
using api.ViewModels;
using MediatR;

namespace api.MinimalApis
{
    public static partial class ClientApi
    {
        public static void RegisterClientApi(this WebApplication app)
        {
            app.MapGet("/clients", async (IMediator mediator) =>
            {
                return await mediator.Send(new GetClientsQuery());
            })
            .WithName("get clients");

            app.MapGet("/clients/search", async (IMediator mediator, string term) =>
            {
                return await mediator.Send(new SearchClientQuery(term.ToLower()));
            })
            .WithName("search clients");

            app.MapPost("/clients", async (IMediator mediator, ClientVm client) =>
            {
                return await mediator.Send(new AddClientCommand(client));
            })
            .WithName("add client");

            app.MapPut("/clients", async (IMediator mediator, ClientVm client) =>
            {
                return await mediator.Send(new UpdateClientCommand(client));
            })
            .WithName("update client");
        }
    }
}
