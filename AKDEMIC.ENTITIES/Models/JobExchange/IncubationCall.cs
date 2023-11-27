using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class IncubationCall: Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }      
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Document { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
