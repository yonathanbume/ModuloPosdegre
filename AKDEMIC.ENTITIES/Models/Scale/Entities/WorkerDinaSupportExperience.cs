using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerDinaSupportExperience
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public Guid WorkerDinaId { get; set; }
        public WorkerDina WorkerDina { get; set; }
    }
}
