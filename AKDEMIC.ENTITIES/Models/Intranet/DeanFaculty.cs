using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DeanFaculty : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid FacultyId { get; set; }
        public string DeanId { get; set; }
        public string SecretaryId { get; set; }
        public string DeanResolution { get; set; }
        public string DeanResolutionFile { get; set; }
        public ApplicationUser Dean { get; set; }
        public ApplicationUser Secretary { get; set; }
        public Faculty Faculty { get; set; }
    }
}
