using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ContestStudentRequirement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ContestStudentId { get; set; }
        public ContestStudent ContestStudent { get; set; }
        public Guid ContestRequirementId { get; set; }
        public ContestRequirement ContestRequirement { get; set; }
        public bool Approved { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
