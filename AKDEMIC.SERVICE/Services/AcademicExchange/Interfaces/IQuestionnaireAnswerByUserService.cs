using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.AcademicExchange;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IQuestionnaireAnswerByUserService
    {
        Task InsertRange(List<QuestionnaireAnswerByUser> answers);
        Task<IEnumerable<QuestionnaireAnswerByUser>> GetAllByPostulationId(Guid postulationId);
        Task DeleteRange(IEnumerable<QuestionnaireAnswerByUser> entities);
    }
}
