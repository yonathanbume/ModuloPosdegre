using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class PerformanceEvaluationUserDetail
    {
        public Guid? SectionId { get; set; }
        public List<Role> Roles { get; set; }
        public decimal Total { get; set; }
        public decimal Qualification { get; set; } //Basada en puntaje máximo segun cantidad de evaluaciones
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public List<PECriterion> Criterions { get; set; }
        public int EvaluatedStudents { get; set; }
        public object[] ChartData { get; set; }
        public string ChartDataJSON => ChartData != null ? JsonConvert.SerializeObject(ChartData) : string.Empty;
        public List<RaitingScale> RaitingScales { get; set; }
    }

    public class RaitingScale
    {
        public string Description { get; set; }
        public byte Value { get; set; }
    }
    public class Question
    {
        public Guid Id { get; set; }
        public Guid? CriterionId { get; set; }
        public string Description { get; set; }
        public int Max { get; set; }
        public List<Response> Responses { get; set; }
        public decimal Average { get; set; }
    }

    public class Response
    {
        public int Count { get; set; }
        public int Value { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
    }
    public class PECriterion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
