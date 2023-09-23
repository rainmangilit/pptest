using api.Data;
using api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Queries.Clients
{
    public record SearchClientQuery(string Term) : IRequest<IList<Client>>;

    public class SearchClientsQueryHandler : IRequestHandler<SearchClientQuery, IList<Client>>
    {
        private readonly DataContext dataContext;

        public SearchClientsQueryHandler(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IList<Client>> Handle(SearchClientQuery request, CancellationToken cancellationToken = default)
        {
            return await dataContext.Clients
                .Where(x => x.FirstName.ToLower().Contains(request.Term) || x.LastName.ToLower().Contains(request.Term))
                .ToListAsync(cancellationToken);
        }
    }
}
