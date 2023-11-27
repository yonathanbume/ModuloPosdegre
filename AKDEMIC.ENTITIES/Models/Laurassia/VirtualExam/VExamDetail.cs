using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VExamDetail : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ExamResolutionId { get; set; }
        public Guid VQuestionId { get; set; }
        public string StudentId { get; set; }
        public Guid VExamId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Num { get; set; }
        public string Option { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string FileUrl { get; set; } 
        public bool IsQualified { get; set; } = true;
        public string FeedbackObtained { get; set; }
        public decimal MaxPoints { get; set; }
        public decimal PointsObtained { get; set; }
        public string SustentFileUrl { get; set; }
        public virtual ExamResolution ExamResolution { get; set; }
        public virtual VExam VExam { get; set; }
        public virtual VQuestion VQuestion { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
        public ICollection<QualificationLog> QualificationLogs { get; set; }
        public ICollection<QuestionRubricStudent> QuestionRubricStudents { get; set; }
    }
}