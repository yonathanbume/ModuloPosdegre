using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class SectionAnnouncement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? HomeworkId { get; set; }
        public Guid? VExamId { get; set; }
        public Guid SectionId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public ApplicationUser User { get; set; }
        public Homework Homework { get; set; }
        public Section Section { get; set; }
        public VExam VExam { get; set; }
    }
}
