using System;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.StudentInformation
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public Guid CurriculumId { get; set; }
        
        public Guid AcademicProgramId { get; set; }

        public string Picture { get; set; }

        public string FullName { get; set; }

        public string Code { get; set; }

        public string Career { get; set; }

        public string Modality { get; set; }

        public string CurrentTerm { get; set; }

        public string Dni { get; set; }

        public int MeritOrder { get; set; }

        public string AdmissionTerm { get; set; }

        public string GraduationTerm { get; set; }
    }
}
