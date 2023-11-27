using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ProjectRubricItem
    {
        public Guid Id { get; set; }
        public ProjectRubric Rubric { get; set; }
        public Guid RubricId { get; set; }
        public string Name { get; set; }
        public string Indicator { get; set; }
        public short Min { get; set; }
        public short Max { get; set; }
    }
}
