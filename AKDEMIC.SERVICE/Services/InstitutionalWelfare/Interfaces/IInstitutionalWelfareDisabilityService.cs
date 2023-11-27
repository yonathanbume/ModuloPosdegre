using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareDisabilityService
    {
        Task<Disability> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<IEnumerable<Disability>> GetAll();
        Task Insert(Disability disability);
        Task Update(Disability disability);
        Task Delete(Disability disability);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDisabilitiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
