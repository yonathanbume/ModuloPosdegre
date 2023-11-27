using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ProjectSchedule
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime DateTime { get; set; }
        public string File { get; set; }
    }
}
