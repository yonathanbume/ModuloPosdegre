using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentLanguageService: IStudentLanguageService
    {
        private readonly IStudentLanguageRepository _studentLanguageRepository;

        public StudentLanguageService(IStudentLanguageRepository studentLanguageRepository)
        {
            _studentLanguageRepository = studentLanguageRepository;
        }

        public async Task DeleteRange(IEnumerable<StudentLanguage> studentLanguages)
        {
            await _studentLanguageRepository.DeleteRange(studentLanguages);
        }

        public async Task<bool> ExistByLanguage(Guid id)
        {
            return await _studentLanguageRepository.ExistByLanguage(id);
        }

        public async Task<IEnumerable<StudentLanguage>> GetAllByStudent(Guid studentId)
        {
            return await _studentLanguageRepository.GetAllByStudent(studentId);
        }

        public async Task<List<ProfileDetailTemplate.LanguageDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _studentLanguageRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task<IEnumerable<StudentLanguage>> GetAllWithIncludesByStudent(Guid studentId)
        {
            return await _studentLanguageRepository.GetAllWithIncludesByStudent(studentId);
        }

        public async Task InsertRange(IEnumerable<StudentLanguage> studentLanguages)
        {
            await _studentLanguageRepository.InsertRange(studentLanguages);
        }
    }
}
