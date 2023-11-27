using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class EnrollmentInformationTemplate
    {
        public Guid CurriculumId { get; set; }

        public Guid CareerId { get; set; }

        public int Status { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public string Faculty { get; set; }

        public int CurrentAcademicYear { get; set; }

        public string Curriculum { get; set; }

        public string AdmissionTerm { get; set; }

        public string Document { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime BirthDay { get; set; }
    }
}
