using AutoMapper;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.PlanEstudios;

namespace WebApplication2.Configuration.Mapping.Profiles
{
    public class PlanEstudioProfile : Profile
    {
        public PlanEstudioProfile()
        {
            CreateMap<PlanEstudios, PlanEstudioDto>()
                .ForMember(dto => dto.Periodicidad, conf => conf.MapFrom(model => model.IdPeriodicidadNavigation.DescPeriodicidad));

            CreateMap<PlanEstudiosRequest, PlanEstudios>();
            CreateMap<PlanEstudiosUpdateRequest, PlanEstudios>();
        }
    }
}
