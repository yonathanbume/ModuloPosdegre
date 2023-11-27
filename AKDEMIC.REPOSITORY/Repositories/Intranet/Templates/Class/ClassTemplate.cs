using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class
{
    public class ClassTemplate
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string SectionCode { get; set; }
        public Guid TermId { get; set; }
        public string TermName { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
