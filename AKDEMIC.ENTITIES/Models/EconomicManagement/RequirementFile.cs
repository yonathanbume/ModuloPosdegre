using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class RequirementFile
    {
        public Guid Id { get; set; }
        public Guid RequirementId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public Requirement Requirement { get; set; }
    }
}
