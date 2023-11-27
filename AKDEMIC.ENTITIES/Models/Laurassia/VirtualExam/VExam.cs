using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VExam : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Description { get; set; }
        public int Attempts { get; set; }
        public TimeSpan LongTime { get; set; }
        public bool IsShuffle { get; set; }
        public bool QuestionPaging { get; set; } = false;
        public bool GoBack { get; set; } = false;
        public bool IsRecovery { get; set; } = false;
        public bool TimePerQuestion { get; set; } = false;
        public string Title { get; set; }
        public bool Show { get; set; } = true;
        public byte? QuestionLimit { get; set; }
        public virtual ICollection<VQuestion> VQuestions { get; set; }
        public virtual ICollection<VExamDetail> VExamDetail { get; set; }
        public virtual ICollection<VExamFeedback> VExamFeedback { get; set; }
        public virtual ICollection<VExamStudent> VExamStudent { get; set; }
        public virtual ICollection<ExamSegment> ExamSegments { get; set; }
        public virtual ICollection<RecoveryExamStudent> RecoveryExamStudents { get; set; }
    }
}
