using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ExecuteObservation
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
        public DateTime? EndDatetime { get; set; }
        public string UrlFile { get; set; }
    }
}
