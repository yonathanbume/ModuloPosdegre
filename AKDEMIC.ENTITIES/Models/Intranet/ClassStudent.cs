using System;
using System.Collections;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ClassStudent
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public bool IsAbsent { get; set; }

        public Class Class { get; set; }
        public Student Student { get; set; }

        public ICollection<StudentAbsenceJustification> StudentAbsenceJustifications { get; set; }
    }
}
