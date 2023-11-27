using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeachingLoadSubType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid TeachingLoadTypeId { get; set; }
        public TeachingLoadType TeachingLoadType { get; set; }

        public bool FixedTime { get; set; }
        public int? MinHours { get; set; }
        public int MaxHours { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
