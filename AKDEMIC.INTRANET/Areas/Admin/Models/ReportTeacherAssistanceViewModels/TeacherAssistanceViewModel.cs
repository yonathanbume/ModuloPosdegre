namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherAssistanceViewModels
{
    public class TeacherAssistanceViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int Assistance { get; set; }
        public int NonAssistance { get; set; }
        public int FirstLate { get; set; }
        public int SecondLate { get; set; } 
    }
}
