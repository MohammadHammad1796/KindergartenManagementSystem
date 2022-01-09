using AutoMapper;
using KindergartenManagementSystem.Controllers.Apis.Dtos;
using KindergartenManagementSystem.Core.Helpers;

namespace KindergartenManagementSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Jwt, JwtDto>();
        }
    }
}