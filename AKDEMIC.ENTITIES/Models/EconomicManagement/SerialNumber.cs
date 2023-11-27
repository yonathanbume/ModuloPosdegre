using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class SerialNumber : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Series { get; set; }

        public bool IsBankSerialNumber { get; set; }

        public byte DocumentType { get; set; } = 1; //1 = boleta interna, 2 = boleta, 3 = factura
        
        public ApplicationUser User { get; set; }
    }
}
