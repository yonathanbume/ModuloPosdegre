using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeachingLoadType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public bool Enabled { get; set; } = true;

        public ICollection<TeachingLoadSubType> TeachingLoadSubTypes { get; set; }
        public ICollection<NonTeachingLoad> NonTeachingLoads { get; set; }
    }
}
