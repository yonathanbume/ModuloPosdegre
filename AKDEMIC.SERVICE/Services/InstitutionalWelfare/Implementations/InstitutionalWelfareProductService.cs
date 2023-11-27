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
    public class InstitutionalWelfareProductService : IInstitutionalWelfareProductService
    {
        private readonly IInstitutionalWelfareProductRepository _institutionalWelfareProductRepository;

        public InstitutionalWelfareProductService(IInstitutionalWelfareProductRepository institutionalWelfareProductRepository)
        {
            _institutionalWelfareProductRepository = institutionalWelfareProductRepository;
        }

        public async Task Delete(InstitutionalWelfareProduct entity)
        {
            await _institutionalWelfareProductRepository.Delete(entity);
        }

        public async Task<InstitutionalWelfareProduct> Get(Guid id)
        {
            return await _institutionalWelfareProductRepository.Get(id);
        }

        public async Task<IEnumerable<InstitutionalWelfareProduct>> GetAll()
        {
            return await _institutionalWelfareProductRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _institutionalWelfareProductRepository.GetInstitutionalWelfareProductDatatable(sentParameters, searchValue);
        }

        public async Task Insert(InstitutionalWelfareProduct entity)
        {
            await _institutionalWelfareProductRepository.Insert(entity);
        }

        public async Task<object> InstitutionalWelfareProductSelect2()
        {
            return await _institutionalWelfareProductRepository.InstitutionalWelfareProductSelect2();
        }

        public async Task Update(InstitutionalWelfareProduct entity)
        {
            await _institutionalWelfareProductRepository.Update(entity);
        }
    }
}
