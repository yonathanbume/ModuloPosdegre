using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationUser
    {
        public Guid Id { get; set; }
        public PerformanceEvaluationTemplate PerformanceEvaluationTemplate { get; set; }
        public Guid PerformanceEvaluationTemplateId { get; set; }
        public string FromRoleId { get; set; }
        public ApplicationRole FromRole { get; set; }
        public string FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public string ToTeacherId { get; set; }
        public Teacher ToTeacher { get; set; }
        public Guid? PerformanceEvaluationId { get; set; }
        public PerformanceEvaluation PerformanceEvaluation { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public Guid? SectionId { get; set; }
        public Section Section { get; set; }
        public string Commentary { get; set; }
        public ICollection<PerformanceEvaluationResponse> Responses { get; set; }
    }
}
