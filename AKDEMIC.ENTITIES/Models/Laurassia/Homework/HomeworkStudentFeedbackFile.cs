using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class HomeworkStudentFeedbackFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid HomeworkStudentId { get; set; }
        public HomeworkStudent HomeworkStudent { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
