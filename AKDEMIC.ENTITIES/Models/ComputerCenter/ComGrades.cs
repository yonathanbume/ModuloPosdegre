using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComGrades
    {
        public Guid Id { get; set; }
        public Guid ComEvaluationCriteriaId { get; set; }
        public ComEvaluationCriteria ComEvaluationCriteria { get; set; }
        public Guid ComUserGroupId { get; set; }
        public ComUserGroup ComUserGroup { get; set; }
        public decimal Value { get; set; }
        public bool Taken { get; set; }
    }
}
