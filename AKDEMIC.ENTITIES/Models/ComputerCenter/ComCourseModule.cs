using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComCourseModule : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComCourseId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public ComCourse ComCourse { get; set; }
    }
}
