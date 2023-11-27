using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class StudentCourseCertificate : Entity, ITimestamp
    {
        public Guid CourseCertificateId { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }

        public CourseCertificate CourseCertificate { get; set; }
    }
}
