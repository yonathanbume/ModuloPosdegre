using System;

namespace AKDEMIC.INTRANET.ViewModels.InstitutionalAlertViewModels
{
    public class InstitutionalAlertViewModel
    {
        public Guid Id { get; set; }
        public string Applicant { get; set; }
        public string Register{ get; set; }
        public string Dependency { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public string Status { get; set; }
    }
}
