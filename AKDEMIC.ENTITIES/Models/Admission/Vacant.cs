using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class Vacant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public Guid CareerApplicationTermId { get; set; }
        public Guid? AcademicProgramId { get; set; }

        public int Number { get; set; }

        public AdmissionType AdmissionType { get; set; }
        public CareerApplicationTerm CareerApplicationTerm { get; set; }
        public AcademicProgram AcademicProgram { get; set; }
    }
}
