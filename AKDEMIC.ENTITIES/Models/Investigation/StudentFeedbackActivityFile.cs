using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class StudentFeedbackActivityFile
    {
        public Guid Id { get; set; }
        public StudentFeedbackActivity StudentFeedbackActivity { get; set; }
        public Guid StudentFeedbackActivityId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
