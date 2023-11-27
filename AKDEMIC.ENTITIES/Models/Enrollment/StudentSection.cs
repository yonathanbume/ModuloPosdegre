using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class StudentSection : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public Guid? SectionGroupId { get; set; }
        public Guid StudentId { get; set; }
        public int FinalGrade { get; set; }
        public string Observations { get; set; }
        public int Status { get; set; } = 0;
        public int Try { get; set; } = 1;
        public DateTime? SyllabusDownloadDate { get; set; }
        public Section Section { get; set; }
        public SectionGroup SectionGroup { get; set; }
        public Student Student { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }
}