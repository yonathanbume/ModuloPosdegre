using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public sealed class AcademicYearCourseTemplateA
    {
        public Guid Id { get; set; }
        public string Area { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Cycle { get; set; }
        public decimal Credit { get; set; }
        public decimal RequiredCredit { get; set; }
        public decimal TotalHours { get; set; }
        public decimal PlannedHours { get; set; }
        public decimal PracticalHours { get; set; }
        public string Regularized { get; set; }
        public int AcademicYearNumber { get; set; }
        public List<string> Requisites { get; set; }
        public List<string> Certificates { get; set; }
        public List<string> CodRequisites { get; set; }
        public int Category { get; set; }
    }
}