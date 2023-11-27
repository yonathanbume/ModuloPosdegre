using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
        Task<Questionnaire> GetByScholarshipId(Guid scholarshipId);
    }
}
