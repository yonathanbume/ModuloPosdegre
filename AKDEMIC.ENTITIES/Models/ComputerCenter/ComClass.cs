using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComClass : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComGroupId { get; set; }
        public ComGroup ComGroup { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

       // public ICollection<ComClassUser> ComClassUsers { get; set; }
    }
}
