using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private readonly ICompanyTypeRepository _companyTypeRepository;

        public CompanyTypeService(ICompanyTypeRepository companyTypeRepository)
        {
            _companyTypeRepository = companyTypeRepository;
        }

        public async Task Delete(CompanyType model)
        {
            await _companyTypeRepository.Delete(model);
        }

        public async Task<CompanyType> Get(Guid id)
        {
            return await _companyTypeRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompanyTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _companyTypeRepository.GetCompanyTypeDatatable(sentParameters,searchValue);
        }

        public async Task<object> GetCompanyTypeSelect2()
        {
            return await _companyTypeRepository.GetCompanyTypeSelect2();
        }

        public async Task Insert(CompanyType model)
        {
            await _companyTypeRepository.Insert(model);
        }

        public async Task Update(CompanyType model)
        {
            await _companyTypeRepository.Update(model);
        }
    }
}
