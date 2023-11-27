using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class CompanySizeService: ICompanySizeService
    {
        private readonly ICompanySizeRepository _companySizeRepository;

        public CompanySizeService(ICompanySizeRepository companySizeRepository)
        {
            _companySizeRepository = companySizeRepository;
        }

        public async Task Delete(CompanySize model)
        {
            await _companySizeRepository.Delete(model);
        }

        public async Task<CompanySize> Get(Guid id)
        {
            return await _companySizeRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompanySizeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _companySizeRepository.GetCompanySizeDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetCompanySizeSelect2()
        {
            return await _companySizeRepository.GetCompanySizeSelect2();
        }

        public async Task Insert(CompanySize model)
        {
            await _companySizeRepository.Insert(model);
        }

        public async Task Update(CompanySize model)
        {
            await _companySizeRepository.Update(model);
        }
    }
}
