using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareDisabilityRepository : IRepository<Disability>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllDisabilitiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<bool> AnyByCode(string Code, Guid? id = null);
    }
}
