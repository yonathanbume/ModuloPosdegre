using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICompetenceEvaluationService
    {
        Task Delete(CompetenceEvaluation competenceEvaluation);
        Task DeleteById(Guid id);
        Task DeleteRange(IEnumerable<CompetenceEvaluation> competenceEvaluations);
        Task Insert(CompetenceEvaluation competenceEvaluation);
        Task InsertRange(IEnumerable<CompetenceEvaluation> competenceEvaluations);
        Task Update(CompetenceEvaluation competenceEvaluation);
        Task<CompetenceEvaluation> Get(Guid id);
        Task<IEnumerable<CompetenceEvaluation>> GetAll();
    }
}