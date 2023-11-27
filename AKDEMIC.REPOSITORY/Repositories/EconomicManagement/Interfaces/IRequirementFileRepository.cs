using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IRequirementFileRepository : IRepository<RequirementFile>
    {
        Task<DataTablesStructs.ReturnedData<RequirementFile>> GetRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
