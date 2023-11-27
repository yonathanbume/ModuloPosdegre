using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert
{
    public class InstitutionalAlertDetailTemplate
    {
        public Guid Id { get; set; }
        public string Applicant { get; set; }
        public string Dependency { get; set; }
        public string RegisterDate { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }

        public string Assistant { get; set; }
    }
}
