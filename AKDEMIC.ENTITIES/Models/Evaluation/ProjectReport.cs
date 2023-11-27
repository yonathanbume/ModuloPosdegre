using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectReport
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public byte Type { get; set; }
        public byte? Approved { get; set; }
    }
}
