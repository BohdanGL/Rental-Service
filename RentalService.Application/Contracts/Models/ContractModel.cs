using RentalService.Application.Equipments.Models;
using RentalService.Application.Facilities.Models;

namespace RentalService.Application.Contracts.Models
{
    public class ContractModel
    {
        public int Id { get; set; }
        public FacilityModel Facility { get; set; }
        public EquipmentModel Equipment { get; set; }
        public int EquipmentCount { get; set; }
    }
}
