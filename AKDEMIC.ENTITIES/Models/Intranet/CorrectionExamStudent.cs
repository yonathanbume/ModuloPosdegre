using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class CorrectionExamStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CorrectionExamId { get; set; }
        public CorrectionExam CorrectionExam { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime? GradePublicationDate { get; set; }
        public byte Status { get; set; } = ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.PENDING;
        public int? Grade { get; set; }
    }
}
