using System;
using System.Collections.Generic;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ReportBachelorViewModels
{

    public class DataViewModel
    {
        public List<Guid> LstPrograms { get; set; }
        public List<Guid> LstCareers { get; set; }
        public List<Guid> LstFaculties { get; set; }
        public int GradeType { get; set; }
    }
}
