using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class NonTeachingLoadActivity : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid NonTeachingLoadId { get; set; }
        public NonTeachingLoad NonTeachingLoad { get; set; }

        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}
