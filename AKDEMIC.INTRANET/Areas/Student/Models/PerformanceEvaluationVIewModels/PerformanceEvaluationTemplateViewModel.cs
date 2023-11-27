using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Student.Models.PerformanceEvaluationVIewModels
{
    public class PerformanceEvaluationTemplateViewModel
    {
        public Guid? Id { get; set; }
        public Guid SectionId { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public byte Max { get; set; }
        public Guid TemplateId { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Commentary { get; set; }
        public string Target { get; set; }
        public string Instructions { get; set; }
        public List<PerformanceEvaluationScaleViewModel> Scales { get; set; }
        public List<PerformanceEvaluationQuestionViewModel> Questions { get; set; }
        public List<PerformanceEvaluationCriterionViewModel> Criterions { get; set; }
    }

    public class PerformanceEvaluationScaleViewModel
    {
        public int Value { get; set; }
        public string Description { get; set; }
    }

    public class PerformanceEvaluationQuestionViewModel
    {
        public DateTime? CreatedAt { get; set; }
        public Guid? Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Description { get; set; }
        public byte Value { get; set; }
        public Guid? CriterionId { get; set; }
    }

    public class PerformanceEvaluationCriterionViewModel
    {
        public DateTime? CreatedAt { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
