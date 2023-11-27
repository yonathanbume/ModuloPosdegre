using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class RecoveryExamStudent
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public VExam Exam { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
