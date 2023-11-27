using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class CareerHistory: Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public string CoordinatorId { get; set; }
        public string SecretaryId { get; set; }
        public string CareerDirectorId { get; set; }
        public string AcademicDepartmentDirectorId { get; set; }
        public ApplicationUser Coordinator { get; set; }
        public ApplicationUser Secretary { get; set; }
        public ApplicationUser AcademicDepartmentDirector { get; set; }
        public ApplicationUser CareerDirector { get; set; }
    }
}
