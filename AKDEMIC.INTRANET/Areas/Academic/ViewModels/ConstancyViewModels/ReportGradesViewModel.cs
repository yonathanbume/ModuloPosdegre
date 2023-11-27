using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.ConstancyViewModels
{
    public class ReportGradesViewModel
    {
        public string HeaderText { get; set; }
        public string Department { get; set; }
        public string UniversityName { get; set; }
        public string FullName { get; set; }
        public string ActiveTerm { get; set; }
        public string ImagePathLogo { get; set; }
        public string CodeStudent { get; set; }
        public string AcademicYear { get; set; }
        public string Career { get; set; }
        public string Campus { get; set; }
        public string Average { get; set; } 
        public List<GradeVm> Grades { get; set; } 
    }

    public class GradeVm
    {
        public string Ciclo { get; set; }
        public string Code { get; set; }
        public string Course { get; set; }
        public string Credits { get; set; }
        public string Grades { get; set; }
        public string Observation { get; set; }
        public string Asistance { get; set; }
    }
}
