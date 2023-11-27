using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.Report_eventViewModels
{
    public class ReportEventViewModel
    {
        public Guid Id { get; set; }        
        public string StartEndDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal GeneralCost { get; set; }
        public string EventDate { get; set; }
        public string OrganizerName { get; set; }
        public string Place { get; set; }

    }
}
