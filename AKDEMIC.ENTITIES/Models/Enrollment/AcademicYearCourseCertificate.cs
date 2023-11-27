using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicYearCourseCertificate : Entity, ITimestamp
    {
        public Guid AcademicYearCourseId { get; set; }

        public Guid CourseCertificateId { get; set; }

        public AcademicYearCourse AcademicYearCourse { get; set; }

        public CourseCertificate CourseCertificate { get; set; }
    }
}
