using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareUserProductRepository: IRepository<InstitutionalWelfareUserProduct>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetUserProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> ExistByProduct(Guid productId);
        Task<InstitutionalWelfareUserProduct> GetWithIncludes(Guid id);
        Task<object> GetReport();
        //Task<DataTablesStructs.ReturnedData<object>> GetUserProductReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
