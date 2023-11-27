using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComActivity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComCourseModuleId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public ComCourseModule ComCourseModule { get; set; }
    }
}
