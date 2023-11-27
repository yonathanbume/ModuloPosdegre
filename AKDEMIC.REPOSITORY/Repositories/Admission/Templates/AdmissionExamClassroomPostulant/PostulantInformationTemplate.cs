using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomPostulant
{
    public class PostulantInformationTemplate
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string AdmissionType { get; set; }
        public string ApplicationTerm { get; set; }
        public string Picture { get; set; }
        public int Seat { get; set; }
        public string Classroom { get; set; }
        public string Floor { get; set; }
        public string Building { get; set; }
        public string Campus { get; set; }
        public string CampusAddress { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
