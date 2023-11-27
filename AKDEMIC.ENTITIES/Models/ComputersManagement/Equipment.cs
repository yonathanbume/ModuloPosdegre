using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class Equipment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string YearModel { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime RegistDate { get; set; }
        public Guid StateId { get; set; }
        public Guid PersonalId { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public Guid? DependencyId { get; set; }

        public ComputerState State { get; set; }
        public ApplicationUser Personal { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public Dependency Dependency { get; set; }
    }
}
