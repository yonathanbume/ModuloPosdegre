namespace AKDEMIC.INTRANET.Areas.Admin.Models.UserAssistanceViewModels
{
    public class WorkingDayViewModel
    {
        public string UserName { get; set; }
        
        public double Date { get; set; }

        public bool IsAbsent { get; set; }

        public bool? FirstLate { get; set; }
        public double FirstEntryTime { get; set; }
        public double FirstExitTime { get; set; }
        public double? FirstEntry { get; set; }
        public double? FirstExit { get; set; }

        public bool? SecondLate { get; set; }
        public double SecondEntryTime { get; set; }
        public double SecondExitTime { get; set; }
        public double? SecondEntry { get; set; }
        public double? SecondExit { get; set; }
    }
}
