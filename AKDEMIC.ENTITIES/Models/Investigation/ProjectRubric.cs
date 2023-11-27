using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ProjectRubric
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public bool Status { get; set; }
        public ICollection<ProjectRubricItem> ProjectRubricItems { get; set; }
    }
}
