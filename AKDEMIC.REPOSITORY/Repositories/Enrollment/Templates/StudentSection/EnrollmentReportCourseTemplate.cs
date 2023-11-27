using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class EnrollmentReportCourseTemplate
    {
        public byte AcademicYear { get; set; }
        public string AcademicYearText { get; set; }

        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Section { get; set; }

        public decimal Credits { get; set; }
        public string Try { get; set; }
        public string Modality { get; set; }

        //public SectionDetailViewModel SectionDetailViewModel { get; set; }

        public int TheoricalHours { get; set; } = 0;
        public int PracticalHours { get; set; } = 0;
        public int LaboratoryHours { get; set; } = 0;
        public string Regime { get; set; } = "";
    }
}
