using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYear
{
    public class AcademicYearTemplateA
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public List<AcademicYearTemplateACourse> Courses { get; set; }
    }

    public class AcademicYearTemplateACourse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Course { get; set; }
        public bool IsElective { get; set; }
        public List<AcademicYearTemplateAPreRequesite> PreRequisites { get; set; }
    }

    public class AcademicYearTemplateAPreRequesite
    {
        public Guid Id { get; set; }
        public string Course { get; set; }
    }
}