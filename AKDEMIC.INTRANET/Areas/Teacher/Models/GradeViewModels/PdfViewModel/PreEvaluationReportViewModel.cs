using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels.PdfViewModel
{
    public class PreEvaluationReportViewModel
    {

        public string Faculty { get; set; }
        public string Carrer { get; set; }
        public string Term { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public decimal Credits { get; set; }
        public string Teacher { get; set; }
        public string Section { get; set; }
        public byte AcademicYear{ get; set; }
        public byte Year { get; set; }
        public string ImagePathLogo { get; set; }

        public List<EvaluationViewModel> Evaluations { get; set; }
        public List<StudentPreGradeViewModel> Students { get; set; }

        public class StudentPreGradeViewModel
        {
            public string Code { get; set; }
            public string FullName { get; set; }
            public List<decimal> Grades { get; set; }
        }

        public class EvaluationViewModel
        {
            public Guid Id { get; set; }
            public bool Taken { get; set; }
            public string Name { get; set; }
            public int Percentage { get; set; }
        }

    }
}
