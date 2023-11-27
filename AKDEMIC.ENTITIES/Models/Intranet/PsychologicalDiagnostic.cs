using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PsychologicalDiagnostic
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public ICollection<PsychologicRecordDiagnostic> PsychologicRecordDiagnostic{ get; set; }
    }
}
