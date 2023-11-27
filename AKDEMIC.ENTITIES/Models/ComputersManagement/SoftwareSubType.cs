using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class SoftwareSubType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public SoftwareType Type { get; set; }
    }
}
