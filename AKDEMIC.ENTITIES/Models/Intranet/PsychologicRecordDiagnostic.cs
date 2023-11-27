using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PsychologicRecordDiagnostic
    {
        public Guid Id { get; set; }
        public Guid PsychologicalRecordId { get; set; }
        public Guid PsychologicalDiagnosticId { get; set; }
        public PsychologicalRecord PsychologicalRecord { get; set; }
        public PsychologicalDiagnostic PsychologicalDiagnostic { get; set; }
        public DateTime DateHistorical { get; set; }
        public bool IsCurrent { get; set; }
    }
}
