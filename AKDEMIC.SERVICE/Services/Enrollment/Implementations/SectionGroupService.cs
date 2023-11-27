using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class SectionGroupService : ISectionGroupService
    {
        private readonly ISectionGroupRepository _sectionGroupRepository;

        public SectionGroupService(ISectionGroupRepository sectionGroupRepository)
        {
            _sectionGroupRepository = sectionGroupRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _sectionGroupRepository.AnyByCode(code, ignoredId);

        public async Task Delete(SectionGroup entity)
            => await _sectionGroupRepository.Delete(entity);

        public async Task<SectionGroup> Get(Guid id)
            => await _sectionGroupRepository.Get(id);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionGroupBySectionSelect2lientSide(Guid sectionId)
            => await _sectionGroupRepository.GetSectionGroupBySectionSelect2lientSide(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionGroupDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _sectionGroupRepository.GetSectionGroupDatatable(parameters, searchValue);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionGroupsSelect2ClientSide()
            => await _sectionGroupRepository.GetSectionGroupsSelect2ClientSide();

        public async Task Insert(SectionGroup entity)
            => await _sectionGroupRepository.Insert(entity);

        public async Task Update(SectionGroup entity)
            => await _sectionGroupRepository.Update(entity);
    }
}
