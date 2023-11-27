using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeRectification : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid? GradeId { get; set; }
        public Guid? EvaluationId { get; set; }
        public Guid? SubstituteExamId { get; set; }
        public string TeacherId { get; set; }
        public int Type { get; set; }
        public int State { get; set; } = 1; // 1: Created 2: UpdatedByTeacher
        public decimal GradePrevious { get; set; }
        public decimal GradeNew { get; set; }
        public string ReasonFile { get; set; }
        public ApplicationUser Teacher { get; set; }
        public Grade Grade { get; set; }
        public ENTITIES.Models.Enrollment.Evaluation Evaluation { get; set; }
        public SubstituteExam SubstituteExam { get; set; }
    }
}
