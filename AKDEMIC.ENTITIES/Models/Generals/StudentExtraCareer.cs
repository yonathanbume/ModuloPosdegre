using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class StudentExtraCareer : Entity, ITimestamp
    {
        public Guid FirstStudentId { get; set; }

        public Guid NewStudentId { get; set; }

        public byte CareerNumber { get; set; }

        public Student FirstStudent { get; set; }

        public Student NewStudent { get; set; }
    }
}
