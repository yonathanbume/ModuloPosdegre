using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkCertificateRecord
    {
        public Guid Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string Area { get; set; }
        public string Charge { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
