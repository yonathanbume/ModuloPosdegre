using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IQuestionRepository: IRepository<Question>
    {
        Task<Question> GetIncludeAnswers(Guid id);
    }
}
