using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IUserRequirementFileService
    {
        Task<DataTablesStructs.ReturnedData<UserRequirementFile>> GetUserRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Delete(UserRequirementFile userRequirementFile);
        Task Insert(UserRequirementFile userRequirementFile);
        IQueryable<UserRequirementFile> GetQueryable(Guid id);
        Task<object> GetUserRequirementFilesDetail(Guid id);
        Task<UserRequirementFile> Get(Guid id);
        Task AddRangeAsync(List<UserRequirementFile> userRequirementFiles);
        Task<List<UserRequirementFile>> GetLstDeleteds(List<Guid> LstDeleteds);
        void RemoveRange(List<UserRequirementFile> userRequirementFiles);
    }
}
