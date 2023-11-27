using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CurriculumArea;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CurriculumAreaService : ICurriculumAreaService
    {
        private readonly ICurriculumAreaRepository _curriculumRepository;

        public CurriculumAreaService(ICurriculumAreaRepository curriculumAreaRepository)
        {
            _curriculumRepository = curriculumAreaRepository;
        }

        public async Task<IEnumerable<CurriculumArea>> GetAll() => await _curriculumRepository.GetAll();

        public async Task<CurriculumArea> Get(Guid id) => await _curriculumRepository.Get(id);

        public async Task<CurriculumArea> GetWithFacultiesId(Guid id) =>
            await _curriculumRepository.GetWithFacultiesId(id);

        public async Task Insert(CurriculumArea curriculumArea) => await _curriculumRepository.Insert(curriculumArea);

        public async Task Update(CurriculumArea curriculumArea) => await _curriculumRepository.Update(curriculumArea);

        public async Task DeleteById(Guid id) => await _curriculumRepository.DeleteById(id);

        public async Task<object> GetCurriculumAreasJson(string q)
            => await _curriculumRepository.GetCurriculumAreasJson(q);

        public async Task<List<CurriculumAreaTemplate>> GetCurriculumsForTemary()
        {
            return await _curriculumRepository.GetCurriculumsForTemary();
        }
    }
}
