using AutoMapper;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Grupo;

namespace WebApplication2.Configuration.Mapping.Profiles
{
    public class GrupoProfile : Profile
    {
        public GrupoProfile()
        {
            CreateMap<Grupo, GrupoDto>()
                .ForMember(dto => dto.PlanEstudios, config => config.MapFrom(model => model.IdPlanEstudiosNavigation.NombrePlanEstudios))
                .ForMember(dto => dto.PeriodoAcademico, config => config.MapFrom(model => model.IdPeriodoAcademicoNavigation.Nombre))
                .ForMember(dto => dto.Turno, config => config.MapFrom(model => model.IdTurnoNavigation.Nombre));
            CreateMap<Grupo, GrupoDetalleDto>()
                .ForMember(dto => dto.PlanEstudios, config => config.MapFrom(model => model.IdPlanEstudiosNavigation.NombrePlanEstudios))
                .ForMember(dto => dto.PeriodoAcademico, config => config.MapFrom(model => model.IdPeriodoAcademicoNavigation.Nombre))
                .ForMember(dto => dto.Turno, config => config.MapFrom(model => model.IdTurnoNavigation.Nombre));
            CreateMap<GrupoRequest, Grupo>();
            CreateMap<GrupoUpdateRequest, Grupo>();
        }
    }
}
