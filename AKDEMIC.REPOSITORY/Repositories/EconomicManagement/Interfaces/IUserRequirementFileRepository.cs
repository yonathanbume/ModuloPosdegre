using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IUserRequirementFileRepository : IRepository<UserRequirementFile>
    {
        Task<DataTablesStructs.ReturnedData<UserRequirementFile>> GetUserRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        IQueryable<UserRequirementFile> GetQueryable(Guid id);
        Task<object> GetUserRequirementFilesDetail(Guid id);
        Task<List<UserRequirementFile>> GetLstDeleteds(List<Guid> LstDeleteds);
    }
}
