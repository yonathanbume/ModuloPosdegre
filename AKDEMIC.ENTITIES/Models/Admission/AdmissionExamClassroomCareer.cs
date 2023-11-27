using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamClassroomCareer
    {
        public Guid AdmissionExamClassroomId { get; set; }
        public Guid CareerId { get; set; }
        
        public AdmissionExamClassroom AdmissionExamClassroom { get; set; }
        public Career Career { get; set; }
    }
}
