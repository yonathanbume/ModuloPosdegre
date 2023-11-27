using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class InstitutionalAlert
    {
        public Guid Id { get; set; }
        public int? Type { get; set; }        
        public string Description { get; set; }
        
        public DateTime RegisterDate { get; set; }
        public DateTime? AttentionDate { get; set; }
        public bool Status { get; set; }

        public Guid? DependencyId{ get; set; }
        public Dependency Dependency { get; set; }

        public string ApplicantId { get; set; }
        public ApplicationUser Applicant { get; set; }

        public string AssistantId { get; set; }
        public ApplicationUser Assistant { get; set; }

        [NotMapped]
        public string AlertStatus { get; set; }
        [NotMapped]
        public int AlertCount { get; set; }
    }
}
