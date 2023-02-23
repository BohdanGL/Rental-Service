namespace RentalService.Domain.Entities
{
    public class Contract
    {
        public int Id { get; set; }
        public Facility Facility { get; set; }
        public Equipment Equipment { get; set; }
        public int EquipmentCount { get; set; }
    }
}
