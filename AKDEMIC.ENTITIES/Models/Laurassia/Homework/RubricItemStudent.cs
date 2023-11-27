using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class RubricItemStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public string Feedback { get; set; }
        public Guid HomeworkStudentId { get; set; }
        public Guid? RubricItemDetailId { get; set; }
        public HomeworkStudent HomeworkStudent { get; set; }
        public RubricItemDetail RubricItemDetail { get; set; }
        public Guid RubricItemId { get; set; }
        public RubricItem RubricItem { get; set; }
    }
}
