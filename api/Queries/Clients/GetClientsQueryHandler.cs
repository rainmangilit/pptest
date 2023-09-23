using api.Data;
using api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Queries.Clients
{
    public record GetClientsQuery() : IRequest<IList<Client>>;

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IList<Client>>
    {
        private readonly DataContext dataContext;

        public GetClientsQueryHandler(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IList<Client>> Handle(GetClientsQuery request, CancellationToken cancellationToken = default)
        {
            return await dataContext.Clients.ToListAsync(cancellationToken);
        }
    }
}
