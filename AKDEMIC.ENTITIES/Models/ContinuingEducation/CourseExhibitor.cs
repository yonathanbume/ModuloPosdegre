using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class CourseExhibitor : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string Responsible { get; set; }
        public string Topic { get; set; }
        public string OriginOrganization { get; set; }
    }
}
