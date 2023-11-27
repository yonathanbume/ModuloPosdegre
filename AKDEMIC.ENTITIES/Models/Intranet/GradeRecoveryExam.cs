using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeRecoveryExam
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid SectionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte Status { get; set; } = CORE.Helpers.ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.PENDING;
        public Classroom Classroom { get; set; }
        public Section Section { get; set; }
        public ICollection<GradeRecovery> GradeRecoveryStudents { get; set; }
    }
}
