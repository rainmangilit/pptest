using AutoMapper;

namespace api.Mappers
{
    public class MapperConfig
    {
        public static Mapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClientMapper());
            });

            return new Mapper(config);
        }
    }
}
