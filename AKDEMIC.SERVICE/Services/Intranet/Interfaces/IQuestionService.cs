using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> Get(Guid id);
        Task<Question> Add(Question question);
        Task<Question> GetIncludeAnswers(Guid id);
        Task Insert(Question question);
        Task Update(Question question);
        Task Delete(Question question);
        Task DeleteById(Guid id);
    }
}
