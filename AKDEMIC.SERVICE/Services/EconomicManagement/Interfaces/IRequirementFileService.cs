using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IRequirementFileService
    {
        Task<DataTablesStructs.ReturnedData<RequirementFile>> GetRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Delete(RequirementFile requirementFile);
        Task Insert(RequirementFile requirementFile);
    }
}
