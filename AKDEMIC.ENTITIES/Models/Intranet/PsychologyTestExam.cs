using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PsychologyTestExam : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public bool Answer { get; set; }

        public Guid PsychologyTestQuestionId { get; set; }
        public Guid PsychologicalRecordId { get; set; }

        public PsychologyTestQuestion PsychologyTestQuestion { get; set; }
        public PsychologicalRecord PsychologicalRecord { get; set; }

    }
}
