using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO;

namespace Manager.Tests.Configurations
{
    public static class AutoMapperConfiguration{
        public static IMapper GetConfiguration() {

            var AutoMapperConfig = new MapperConfiguration(cfg => {

                cfg.CreateMap<User, UserDTO>().ReverseMap();
                
            });

            return AutoMapperConfig.CreateMapper();
        }
    }
}