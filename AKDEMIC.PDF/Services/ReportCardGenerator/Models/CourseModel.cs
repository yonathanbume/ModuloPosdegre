using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.ReportCardGenerator.Models
{
    public class CourseModel
    {
        public int? AcademicYear { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Credits { get; set; }
        public int? Grade { get; set; }
        public string Observation { get; set; }
        public bool Approved { get; set; }
        public bool Withdraw { get; set; }
        public byte? Type { get; set; }
        public string Date { get; set; }
        public string Section { get; set; }
    }
}
