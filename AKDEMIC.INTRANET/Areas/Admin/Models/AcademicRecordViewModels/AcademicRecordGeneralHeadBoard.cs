using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.AcademicRecordViewModels
{
    public class AcademicRecordGeneralHeadBoard
    {
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string CodeFullName { get; set; }
        public string CurrentDate { get; set; }

        public List<AcademicRecordGeneralViewModel> Details { get; set; }
    }
}
