using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipTypeService : IInstitutionalWelfareScholarshipTypeService
    {
        private readonly IInstitutionalWelfareScholarshipTypeRepository _institutionalWelfareScholarshipTypeRepository;

        public InstitutionalWelfareScholarshipTypeService(IInstitutionalWelfareScholarshipTypeRepository institutionalWelfareScholarshipTypeRepository)
        {
            _institutionalWelfareScholarshipTypeRepository = institutionalWelfareScholarshipTypeRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _institutionalWelfareScholarshipTypeRepository.AnyByName(name, ignoredId);

        public async Task<bool> AnyScholarship(Guid id)
            => await _institutionalWelfareScholarshipTypeRepository.AnyScholarship(id);

        public async Task Delete(InstitutionalWelfareScholarshipType entity)
            => await _institutionalWelfareScholarshipTypeRepository.Delete(entity);

        public async Task<InstitutionalWelfareScholarshipType> Get(Guid id)
            => await _institutionalWelfareScholarshipTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipTypesDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _institutionalWelfareScholarshipTypeRepository.GetScholarshipTypesDatatable(parameters, searchValue);

        public async Task<object> GetScholarshipTypeSelect2()
            => await _institutionalWelfareScholarshipTypeRepository.GetScholarshipTypeSelect2();

        public async Task Insert(InstitutionalWelfareScholarshipType entity)
            => await _institutionalWelfareScholarshipTypeRepository.Insert(entity);

        public async Task Update(InstitutionalWelfareScholarshipType entity)
            => await _institutionalWelfareScholarshipTypeRepository.Update(entity);
    }
}
