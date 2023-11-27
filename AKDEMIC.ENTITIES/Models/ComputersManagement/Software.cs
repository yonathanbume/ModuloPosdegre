using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class Software : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Version { get; set; }
        public int Year { get; set; }
        public string Serie { get; set; }
        public string TypeLicense { get; set; }
        public string Features { get; set; }

        public Guid ComputerId { get; set; }
        public Guid TypeId { get; set; }
        public Guid SubTypeId { get; set; }
        public Computer Computer { get; set; }
        public SoftwareType Type { get; set; }
        public SoftwareSubType SubType { get; set; }


    }
}
