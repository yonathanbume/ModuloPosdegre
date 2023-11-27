using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectScheduleHistoric : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string FileUrl { get; set; }

        [NotMapped]
        public bool Active { get; set; }
    }
}
