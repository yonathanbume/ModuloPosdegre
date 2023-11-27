using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public bool IsTeacher { get; set; }
        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }
    }
}
