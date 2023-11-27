using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DeferredExamStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid DeferredExamId { get; set; }
        public DeferredExam DeferredExam { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime? GradePublicationDate { get; set; }
        public byte Status { get; set; }
        public int? Grade { get; set; }
    }
}
