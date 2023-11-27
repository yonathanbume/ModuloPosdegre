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
    public interface IHeritageRepository : IRepository<Heritage>
    {
        Task<Heritage> Get(Guid catalogItemId, Guid dependencyId);
        Task<DataTablesStructs.ReturnedData<object>> GetHeritageDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string search);
        Task<List<Heritage>> GetHeritages(ClaimsPrincipal user);
    }
}
