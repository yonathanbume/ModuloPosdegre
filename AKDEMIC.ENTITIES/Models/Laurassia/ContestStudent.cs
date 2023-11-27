using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ContestStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public bool? Approbed { get; set; }
        public DateTime? ApprobedDatetime { get; set; }
        public string Commentary { get; set; }
        public string Place { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid ContestId { get; set; }
        public Contest Contest { get; set; }
        public ICollection<ContestStudentRequirement> ContestStudentRequirements { get; set; }
    }
}
