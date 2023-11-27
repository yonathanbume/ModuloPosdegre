using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptGroupRepository : IRepository<ConceptGroup>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConceptGroupsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null);
    }
}
