using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class SectionCodeService : ISectionCodeService
    {
        private readonly ISectionCodeRepository _sectionCodeRepository;

        public SectionCodeService(ISectionCodeRepository sectionCodeRepository)
        {
            _sectionCodeRepository = sectionCodeRepository;
        }

        public async Task Delete(SectionCode sectionCode)
        {
            await _sectionCodeRepository.Delete(sectionCode);
        }

        public async Task<SectionCode> Get(Guid id)
        {
            return await _sectionCodeRepository.Get(id);
        }

        public async Task<IEnumerable<SectionCode>> GetAll()
        {
            return await _sectionCodeRepository.GetAll();
        }

        public Task<object> GetFakeSelect2()
            => _sectionCodeRepository.GetFakeSelect2();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionCodeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _sectionCodeRepository.GetSectionCodeDatatable(sentParameters, searchValue);
        }

        public async Task Insert(SectionCode sectionCode)
        {
            await _sectionCodeRepository.Insert(sectionCode);
        }

        public async Task Update(SectionCode sectionCode)
        {
            await _sectionCodeRepository.Update(sectionCode);
        }
    }
}