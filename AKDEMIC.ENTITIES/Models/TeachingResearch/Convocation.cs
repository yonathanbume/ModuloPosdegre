using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingResearch
{
    public class Convocation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ConvocationSupervisor> Supervisors { get; set; }
        public ICollection<ConvocationFile> Files { get; set; }
        public ICollection<ConvocationQuestion> Questions { get; set; }
        public ICollection<ConvocationPostulant> Postulants { get; set; }
    }
}
