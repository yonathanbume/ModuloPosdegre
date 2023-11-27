using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComClassUser : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComClassId { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsAbsent { get; set; }

        public ComClass ComClass { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
