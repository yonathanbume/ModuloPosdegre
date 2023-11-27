using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Contest : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime InscriptionStart { get; set; }
        public DateTime InscriptionEnd { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<ContestStudent> ContestStudents { get; set; }
        public ICollection<ContestRequirement> ContestRequirements { get; set; }
    }
}
