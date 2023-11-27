using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public class CurriculumTemplate
    {
        public int Cycle { get; set; }

        public string CodeCourse { get; set; }
        public string NameCourse { get; set; }
        public string NameArea { get; set; }
        public decimal Credits { get; set; }
        public decimal RequiredCredits { get; set; }
        public int SeminarHours { get; set; }
        public int PracticalHours { get; set; }
        public int TheoreticalHours { get; set; }
        public int VirtualHours { get; set; }
        public List<PreRequisiteTemplate> Requisites { get; set; }
        public List<CertificatesTemplate> Certificates { get; set; }
    }

    public class PreRequisiteTemplate
    {
        public string Name { get; set; }
    }

    public class CertificatesTemplate
    {
        public string Name { get; set; }
    }
}
