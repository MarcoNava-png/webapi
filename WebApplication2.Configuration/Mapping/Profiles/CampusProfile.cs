using AutoMapper;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Models;
using WebApplication2.Core.Requests.Campus;

namespace WebApplication2.Configuration.Mapping.Profiles
{
    public class CampusProfile : Profile
    {
        public CampusProfile()
        {
            CreateMap<Campus, CampusDto>()
                .ForMember(dto => dto.Direccion, config => config
                    .MapFrom(model => $"{model.IdDireccionNavigation.Calle} {model.IdDireccionNavigation.NumeroExterior} {model.IdDireccionNavigation.NumeroInterior} {model.IdDireccionNavigation.CodigoPostal.Asentamiento} {model.IdDireccionNavigation.CodigoPostal.Municipio.Nombre}, {model.IdDireccionNavigation.CodigoPostal.Municipio.Estado.Abreviatura}"));
            CreateMap<CampusRequest, Campus>()
                .ForMember(model => model.IdDireccionNavigation, config => config.MapFrom(request => !string.IsNullOrEmpty(request.Calle.Trim()) && !string.IsNullOrEmpty(request.NumeroExterior.Trim()) && request.CodigoPostalId != null
                        ? new Direccion
                        {
                            Calle = request.Calle,
                            NumeroExterior = request.NumeroExterior,
                            NumeroInterior = request.NumeroInterior,
                            CodigoPostalId = request.CodigoPostalId.Value
                        }
                        : null));

            CreateMap<CampusUpdateRequest, Campus>()
                .ForMember(model => model.IdDireccionNavigation, config => config.MapFrom(request => !string.IsNullOrEmpty(request.Calle.Trim()) && !string.IsNullOrEmpty(request.NumeroExterior.Trim()) && request.CodigoPostalId != null
                        ? new Direccion
                        {
                            Calle = request.Calle,
                            NumeroExterior = request.NumeroExterior,
                            NumeroInterior = request.NumeroInterior,
                            CodigoPostalId = request.CodigoPostalId.Value
                        }
                        : null));
        }
    }
}
