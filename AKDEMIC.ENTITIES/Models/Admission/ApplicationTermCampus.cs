using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermCampus : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }

        //CampusdelExamen
        public Guid CampusId { get; set; }
        public Campus Campus { get; set; }

        public bool ForExam { get; set; }
        public bool ToApply { get; set; }
    }
}
