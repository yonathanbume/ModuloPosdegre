using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IHeritageService
    {
        Task<Heritage> Get(Guid catalogItemId, Guid dependencyId);
        Task Update(Heritage entity);
        Task<DataTablesStructs.ReturnedData<object>> GetHeritageDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string search);
        Task<List<Heritage>> GetHeritages(ClaimsPrincipal user);
    }
}
