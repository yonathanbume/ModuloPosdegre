using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerDina
    {
        public Guid Id { get; set; }
        public string OrcidCode { get; set; }
        public string ThesisAdvisoryExperience { get; set; }
        public string EvaluatorExperience { get; set; }
        public string Publications { get; set; }
        public string ResearchProjects { get; set; }
        public string OrcidProjects { get; set; }
        public string IntellectualPropertyRights { get; set; }
        public string IndustrialDevelopmentProducts { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WorkerDinaSupportExperience> ExperienceAsThesisSupportMember { get; set; }

    }
}
