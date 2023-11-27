using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.VisitManagement
{
    public class VisitorInformation
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public Guid? ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }
        public string CompanyRuc { get; set; }
    }
}
