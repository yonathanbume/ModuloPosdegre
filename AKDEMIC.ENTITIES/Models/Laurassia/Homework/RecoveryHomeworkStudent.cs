using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class RecoveryHomeworkStudent
    {
        public Guid Id { get; set; }
        public Guid HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
