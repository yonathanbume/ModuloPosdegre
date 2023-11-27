using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareProductRepository : IRepository<InstitutionalWelfareProduct>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> InstitutionalWelfareProductSelect2();
    }
}
