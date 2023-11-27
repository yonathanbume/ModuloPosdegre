using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DirectedCourses
{
    public class DirectedCourseStudentTemplate
    {
        public Guid Id { get; set; }
        public string Teacher { get; set; }
        public string Term { get; set; }
        public string Course { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public Guid CareerId { get; set; }
        public Guid FacultyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public decimal Grade { get; set; }
        public Guid Courseid { get; set; }
        public string Date { get; set; }
        public string Document { get; set; }
    }
}
