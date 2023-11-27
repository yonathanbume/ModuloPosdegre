using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EvaluationType : Entity, IKeyNumber
    {
        public Guid Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public ICollection<CompetenceEvaluation> CompetenceEvaluations { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
    }
}
