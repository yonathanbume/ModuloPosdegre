using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareProductService
    {
        public Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<InstitutionalWelfareProduct>> GetAll();
        Task Insert(InstitutionalWelfareProduct entity);
        Task Update(InstitutionalWelfareProduct entity);
        Task Delete(InstitutionalWelfareProduct entity);
        Task<InstitutionalWelfareProduct> Get(Guid id);
        Task<object> InstitutionalWelfareProductSelect2();
    }
}
