using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert
{
    public class InstitutionalAlertTemplate
    {
        public Guid Id { get; set; }
        public string Applicant { get; set; }
        public string Register { get; set; }
        public string Dependency { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public string Status { get; set; }
    }
}
