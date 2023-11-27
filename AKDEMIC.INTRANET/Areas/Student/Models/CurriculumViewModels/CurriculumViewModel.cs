using System.Collections.Generic;


namespace AKDEMIC.INTRANET.Areas.Student.Models.CurriculumViewModels
{
    public class CurriculumViewModel
    {
        public int Cycle { get; set; }
      
            public string CodeCourse { get; set; }
            public string NameCourse { get; set; }
            public decimal Credits { get; set; }
            public int SeminarHours { get; set; }
            public int PracticalHours { get; set; }
            public int TheoreticalHours { get; set; }
            public int VirtualHours { get; set; }
            public List<PreRequisiteViewModel> Requisites { get; set; }
            public string AcademicProgram { get; set; }
    }
}
