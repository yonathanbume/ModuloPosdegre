using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class PostulantAdmissionRequirement : Entity, ITimestamp
    {
        public Guid AdmissionRequirementId { get; set; }
        public Guid PostulantId { get; set; }
        
        public string File { get; set; }

        public bool IsValidated { get; set; }
        public DateTime? ValidationDate { get; set; }

        public AdmissionRequirement AdmissionRequirement { get; set; }
        public Postulant Postulant { get; set; }
    }
}
