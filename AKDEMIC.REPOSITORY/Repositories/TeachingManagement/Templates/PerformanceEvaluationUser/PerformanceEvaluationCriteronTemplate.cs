using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class PerformanceEvaluationCriteronTemplate
    {
        public Guid EvaluationId { get; set; }
        public string Evaluation { get; set; }
        public string Image { get; set; }
        public string FullName { get; set; }
        public decimal RaitingPercentage { get; set; }
        public decimal Raiting { get; set; }
        public string RaitingStr { get; set; }
        public string TeacherRaitingDescription { get; set; }
        public string GraphSVG { get; set; }
        public string TeacherId { get; set; }
        public object ChartDataJson { get; set; }
        public bool PercentageRaitingScale { get; set; }
        public List<PeformanceEvaluationRubricTemplate> Rubrics { get; set; }
        public List<CriterionTemplate> Criterions { get; set; }
        public string Description { get; set; }
    }

    public class PeformanceEvaluationRubricTemplate
    {
        public string Description { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public class CriterionTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Raiting { get; set; }
        public string RaitingStr { get; set; }
    }
}
