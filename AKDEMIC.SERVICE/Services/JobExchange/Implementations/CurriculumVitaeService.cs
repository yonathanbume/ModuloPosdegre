using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class CurriculumVitaeService: ICurriculumVitaeService
    {
        private readonly ICurriculumVitaeRepository _curriculumVitaeRepository;

        public CurriculumVitaeService(ICurriculumVitaeRepository curriculumVitaeRepository)
        {
            _curriculumVitaeRepository = curriculumVitaeRepository;
        }

        public async Task<CurriculumVitae> Get(Guid id)
        {
            return await _curriculumVitaeRepository.Get(id);
        }

        public async Task<CurriculumVitae> GetByStudent(Guid studentId)
        {
            return await _curriculumVitaeRepository.GetByStudent(studentId);
        }

        public Task<CurriculumVitaeTemplate> GetCurriculumVitae(Guid id)
            => _curriculumVitaeRepository.GetCurriculumVitae(id);

        public Task<CurriculumVitaeTemplate> GetCurriculumVitaeByStudentId(Guid studentId)
            => _curriculumVitaeRepository.GetCurriculumVitaeByStudentId(studentId);

        public async Task Insert(CurriculumVitae curriculumVitae)
        {
            await _curriculumVitaeRepository.Insert(curriculumVitae);
        }

        public async Task Update(CurriculumVitae curriculumVitae)
        {
            await _curriculumVitaeRepository.Update(curriculumVitae);
        }
    }
}
