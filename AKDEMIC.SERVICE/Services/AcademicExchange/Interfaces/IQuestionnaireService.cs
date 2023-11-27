using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IQuestionnaireService
    {
        Task<Questionnaire> GetByScholarshipId(Guid scholarshipId);
        Task<Questionnaire> Get(Guid id);
        Task Insert(Questionnaire model);
        Task Update(Questionnaire entity);
        Task Delete(Questionnaire entity);
    }
}
