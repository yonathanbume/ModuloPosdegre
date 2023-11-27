using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class CareerAcademicPerformanceTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public int Total { get; set; }
        public int Unbeaten { get; set; }
        public double UnbeatenPercentage { get; set; }
        public int OneDisapprovedCourse { get; set; }
        public double OneDisapprovedCoursePercentage { get; set; }
        public int TwoDisapprovedCourse { get; set; }
        public double TwoDisapprovedCoursePercentage { get; set; }
        public int ThreeDisapprovedCourse { get; set; }
        public double ThreeDisapprovedCoursePercentage { get; set; }
        public int Reserve { get; set; }
        public double ReservePercentage { get; set; }

    }
}
