using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class SupervisorDepartment : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? AcademicDepartmentId { get; set; }
        public ApplicationUser User { get; set; }
        public AcademicDepartment AcademicDepartment { get; set; }
    }
}
