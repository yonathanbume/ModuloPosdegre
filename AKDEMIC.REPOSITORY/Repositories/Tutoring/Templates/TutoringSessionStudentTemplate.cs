using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates
{
    public class TutoringSessionStudentTemplate
    {
        public Guid Id { get; set; }
        public Guid SesssionId { get; set; }
        public Guid StudentId { get; set; }
        public string NameStudent { get; set; }
        public int CurrentAcademicYear { get; set; }
        public string Dni { get; set; }
        public string UserName { get; set; }
        public string Campuus { get; set; }
        public string Career { get; set; }
        public  string AdmissionTerm { get; set; }
        public string Session { get; set; }
        public string location { get; set; }
        public string TutorName { get; set; }
        public string DateTime { get; set; }
    }
}
