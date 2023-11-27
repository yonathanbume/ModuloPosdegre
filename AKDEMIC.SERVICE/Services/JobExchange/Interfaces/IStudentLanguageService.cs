using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IStudentLanguageService
    {
        Task<IEnumerable<StudentLanguage>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<StudentLanguage>> GetAllWithIncludesByStudent(Guid studentId);
        Task DeleteRange(IEnumerable<StudentLanguage> studentLanguages);
        Task InsertRange(IEnumerable<StudentLanguage> studentLanguages);
        Task<List<LanguageDate>> GetAllByStudentTemplate(Guid studentId);
        Task<bool> ExistByLanguage(Guid id);
    }
}
