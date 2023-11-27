using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IConceptGroupService
    {
        Task<ConceptGroup> Get(Guid id);
        Task Insert(ConceptGroup group);
        Task Update(ConceptGroup group);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptGroupsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null);

    }
}
