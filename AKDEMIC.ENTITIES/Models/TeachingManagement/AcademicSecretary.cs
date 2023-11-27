using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class AcademicSecretary
    {
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
