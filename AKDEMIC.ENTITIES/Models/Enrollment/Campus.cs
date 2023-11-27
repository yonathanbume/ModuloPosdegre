using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Campus : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? DistrictId { get; set; }

        public string Address { get; set; }
        public DateTime? EndAuthorization { get; set; }
        public DateTime? StartAuthorization { get; set; }
        public byte? AuthorizationType { get; set; }
        public int? BuiltArea { get; set; }
        public string Capacity { get; set; }
        public string Comments { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
        public int? GroundArea { get; set; }
        public string INEI { get; set; }
        public bool IsPrincipal { get; set; } = false;
        public bool IsValid { get; set; } = true;

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }
        public string OtherServiceType { get; set; }
        public string Reference { get; set; }
        public string RENIEC { get; set; }
        public string Telephone { get; set; }
        public byte? ServiceType { get; set; }
        
        public District District { get; set; }
        
        public ICollection<Building> Buildings { get; set; }
        public ICollection<ApplicationTermCampus> ApplicationTermCampuses { get; set; }
    }
}
