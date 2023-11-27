using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectActivity
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public decimal Budget { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
