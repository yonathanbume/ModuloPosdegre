using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CurriculumViewModels
{
    public class CoursesCurriculumViewModel
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
        public string Requisites { get; set; }
        public string Certificates { get; set; }
    }

    public class CurriculumReport
    {
        public int[] Cicles { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Code { get; set; }
        public List<CoursesCurriculumViewModel> CurriculumList { get; set; }
        public string Logo { get; set; }
    }
}
