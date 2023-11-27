using System;
using System.Collections.Generic;

namespace AKDEMIC.PDF.Services.AcademicRecordGenerator.Models
{
    public class TermModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Number { get; set; }
        public List<CourseModel> Courses { get; set; }
    }
}
