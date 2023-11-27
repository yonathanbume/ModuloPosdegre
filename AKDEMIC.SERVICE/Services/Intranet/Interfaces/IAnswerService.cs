using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> Get(Guid id);
        Task<Answer> Add(Answer answer);
        Task AddRange(IEnumerable<Answer> answers);
        Task DeleteRange(IEnumerable<Answer> answers);
        Task Delete(Answer answer);
        Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid id);
    }
}
