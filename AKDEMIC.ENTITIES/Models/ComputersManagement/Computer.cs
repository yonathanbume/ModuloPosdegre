using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class Computer : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string YearModel { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpirationDate { get; set; }
        public DateTime RegistDate { get; set; }

        public Guid StateId { get; set; }
        public Guid PersonalId { get; set; }
        public Guid TypeId { get; set; } //Computadoras, Tablet, Servidores
        public Guid ComputerSupplierId { get; set; }
        public Guid DependencyId { get; set; }
        public ComputerState State { get; set; }
        public ComputerType Type { get; set; }
        public ApplicationUser Personal { get; set; }
        public ComputerSupplier ComputerSupplier { get; set; }
        public Dependency Dependency { get; set; }

        public ICollection<ComputerConditionFile> ComputerConditionFiles { get; set; }
        public ICollection<Software> Softwares { get; set; }
        public ICollection<Hardware> Hardwares { get; set; }
    }
}
