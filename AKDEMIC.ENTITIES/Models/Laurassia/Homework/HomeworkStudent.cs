using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("HomeworkStudent")]
    public class HomeworkStudent : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HomeworkId { get; set; }
        public DateTime Date { get; set; }
        public string Feedback { get; set; }
        public decimal? Score { get; set; }
        public DateTime? QualificationDate { get; set; }
        public string QualificationTeacherId { get; set; }
        public ApplicationUser QualificationTeacher { get; set; }
        public Homework Homework { get; set; }
        //[ForeignKey("Student")]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public Guid? SubmissionId { get; set; }
        public byte? SubmissionStatus { get; set; }
        public ICollection<RubricItemStudent> RubricItemStudents { get; set; }
        public ICollection<HomeworkStudentFile> HomeworkStudentFiles { get; set; }
        public ICollection<HomeworkStudentFeedbackFile> HomeworkStudentFeedbackFiles { get; set; }
        
    }
}