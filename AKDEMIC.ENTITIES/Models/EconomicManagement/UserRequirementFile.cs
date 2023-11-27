using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class UserRequirementFile
    {
        public Guid Id { get; set; }
        public Guid UserRequirementId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public UserRequirement UserRequirement { get; set; }
    }
}
