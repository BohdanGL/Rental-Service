using AutoMapper;
using RentalService.Application.Contracts.Models;
using RentalService.Application.Equipments.Models;
using RentalService.Application.Facilities.Models;
using RentalService.Domain.Entities;

namespace RentalService.Application.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // facility maps
            CreateMap<Facility, FacilityModel>();

            // equipment maps
            CreateMap<Equipment, EquipmentModel>();

            // contract maps
            CreateMap<Contract, ContractModel>();
        }
    }
}
