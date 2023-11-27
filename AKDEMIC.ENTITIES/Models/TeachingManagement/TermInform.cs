using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TermInform : Entity,  ITimestamp
    {
        public Guid Id { get; set; }
        public byte RequestType { get; set; }
        public byte Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public ICollection<TeacherTermInform> TeacherTermInform { get; set; }
    }
}
