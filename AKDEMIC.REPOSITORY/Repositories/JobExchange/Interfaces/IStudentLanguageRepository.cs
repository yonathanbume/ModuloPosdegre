using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentLanguageRepository:IRepository<StudentLanguage>
    {
        Task<IEnumerable<StudentLanguage>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<StudentLanguage>> GetAllWithIncludesByStudent(Guid studentId);
        Task<List<LanguageDate>> GetAllByStudentTemplate(Guid studentId);
        Task<bool> ExistByLanguage(Guid id);
    }
}
