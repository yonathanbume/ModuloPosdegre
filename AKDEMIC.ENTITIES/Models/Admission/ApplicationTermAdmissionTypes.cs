using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermAdmissionType : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }

        public Guid AdmissionTypeId { get; set; }
        public AdmissionType AdmissionType { get; set; }
    }
}
