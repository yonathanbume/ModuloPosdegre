using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SubstituteExamDetail
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<SubstituteExam> SubstituteExams { get; set; }
    }
}
