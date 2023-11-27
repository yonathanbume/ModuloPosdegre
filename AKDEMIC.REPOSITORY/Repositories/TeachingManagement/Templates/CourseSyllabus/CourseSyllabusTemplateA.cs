using System;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.CourseSyllabus
{
    public class CourseSyllabusTemplateA
    {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string TermName { get; set; }

        public Guid TermId { get; set; }
    }
}