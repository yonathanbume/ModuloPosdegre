using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeRecovery : Entity , ITimestamp
    {
        public Guid Id { get; set; }
        public decimal? ExamScore { get; set; }
        public decimal? PrevFinalScore { get; set; }
        public bool? Absent { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseTermId { get; set; }
        public Guid? GradeId { get; set; }
        public Grade Grade { get; set; }
        public CourseTerm CourseTerm { get; set; }
        public Student Student { get; set; }
        public Guid? GradeRecoveryExamId { get; set; }
        public GradeRecoveryExam GradeRecoveryExam { get; set; }
    }
}
