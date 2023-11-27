using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class MedicalConsultReason
    {
        public Guid Id { get; set; }
        public Guid? PsychologicalRecordId { get; set; }
        public string Description { get; set; }
        public DateTime DateHistorical { get; set; }
        public PsychologicalRecord PsychologicalRecord { get; set; }
    }
}
