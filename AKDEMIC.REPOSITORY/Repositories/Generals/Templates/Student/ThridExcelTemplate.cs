using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class ThridExcelTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public int AcademicYear { get; set; }
        public string Course { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string FirstTryTerm { get; set; }
        public decimal FirstTryGrade{ get; set; }
        
        public string SecondTryTerm { get; set; }
        public decimal SecondTryGrade { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
