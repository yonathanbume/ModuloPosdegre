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
    public class InstitutionalWelfareUserProductService : IInstitutionalWelfareUserProductService
    {
        private readonly IInstitutionalWelfareUserProductRepository _institutionalWelfareUserProductRepository;

        public InstitutionalWelfareUserProductService(IInstitutionalWelfareUserProductRepository institutionalWelfareUserProductRepository)
        {
            _institutionalWelfareUserProductRepository = institutionalWelfareUserProductRepository;
        }

        public async Task Delete(InstitutionalWelfareUserProduct entity)
        {
            await _institutionalWelfareUserProductRepository.Delete(entity);
        }

        public async Task<bool> ExistByProduct(Guid productId)
        {
            return await _institutionalWelfareUserProductRepository.ExistByProduct(productId);
        }

        public async Task<InstitutionalWelfareUserProduct> Get(Guid id)
        {
            return await _institutionalWelfareUserProductRepository.Get(id);
        }

        public async Task<InstitutionalWelfareUserProduct> GetWithIncludes(Guid id)
        {
            return await _institutionalWelfareUserProductRepository.GetWithIncludes(id);
        }

        public async Task<IEnumerable<InstitutionalWelfareUserProduct>> GetAll()
        {
            return await _institutionalWelfareUserProductRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _institutionalWelfareUserProductRepository.GetUserProductDatatable(sentParameters, searchValue);
        }

        public async Task Insert(InstitutionalWelfareUserProduct entity)
        {
             await _institutionalWelfareUserProductRepository.Insert(entity);
        }

        public async Task Update(InstitutionalWelfareUserProduct entity)
        {
            await _institutionalWelfareUserProductRepository.Update(entity);
        }

        public async Task<object> GetReport()
        {
            return await _institutionalWelfareUserProductRepository.GetReport();
        }
    }
}
