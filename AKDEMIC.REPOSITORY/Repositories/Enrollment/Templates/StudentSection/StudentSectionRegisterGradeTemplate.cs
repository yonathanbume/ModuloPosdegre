using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class StudentSectionRegisterGradeTemplate
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentFullName { get; set; }
        public int Status { get; set; }
        public Guid? SectionGroupId { get; set; }
        public string StudentUserName { get; set; }
    }
}
