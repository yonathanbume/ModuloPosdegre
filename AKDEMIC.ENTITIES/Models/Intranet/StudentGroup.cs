using System;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentGroup
    {
        public Guid GroupId { get; set; }
        public Guid StudentId { get; set; }

        public Group Group { get; set; }
        public Student Student { get; set; }
    }
}
