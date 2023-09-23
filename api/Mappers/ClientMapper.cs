using api.Models;
using api.ViewModels;

namespace api.Mappers
{
    public class ClientMapper : AutoMapper.Profile
    {
        public ClientMapper()
        {
            CreateMap<ClientVm, Client>()
                .ReverseMap();
        }
    }
}
