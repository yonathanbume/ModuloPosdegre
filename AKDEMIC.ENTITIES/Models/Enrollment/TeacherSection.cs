using System;
using System.Collections;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class TeacherSection : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        //public Guid? SectionGroupId { get; set; }
        public string TeacherId { get; set; }
        public bool IsPrincipal { get; set; }
        public Section Section { get; set; }
        //public SectionGroup SectionGroup { get; set; }
        public Teacher Teacher { get; set; }
    }
}
