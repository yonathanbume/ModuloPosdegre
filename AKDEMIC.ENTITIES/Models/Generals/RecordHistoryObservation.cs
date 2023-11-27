using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class RecordHistoryObservation : Entity,ITimestamp
    {
        public Guid Id { get; set; }
        public string Observation { get; set; }
        public Guid RecordHistoryId { get; set; }
        public RecordHistory RecordHistory { get; set; }
        public byte RecordHistoryStatus { get; set; }
    }
}
