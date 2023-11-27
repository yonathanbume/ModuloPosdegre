using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class Hardware : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComputerId { get; set; }
        public Guid StateId { get; set; }
        public Guid HardwareTypeId { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }

        public Computer Computer { get; set; }
        public ComputerState State { get; set; }
        public HardwareType HardwareType { get; set; }
    }
}
