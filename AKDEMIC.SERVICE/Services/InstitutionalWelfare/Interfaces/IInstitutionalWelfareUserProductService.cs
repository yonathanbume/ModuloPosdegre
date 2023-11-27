using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareUserProductService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetUserProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistByProduct(Guid productId);
        Task<IEnumerable<InstitutionalWelfareUserProduct>> GetAll();
        Task Insert(InstitutionalWelfareUserProduct entity);
        Task Update(InstitutionalWelfareUserProduct entity);
        Task Delete(InstitutionalWelfareUserProduct entity);
        Task<InstitutionalWelfareUserProduct> Get(Guid id);
        Task<InstitutionalWelfareUserProduct> GetWithIncludes(Guid id);
        Task<object> GetReport();


    }
}
