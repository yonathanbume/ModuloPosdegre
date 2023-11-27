using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ILivingCostRepository : IRepository<LivingCost>
    {
        Task<DataTablesStructs.ReturnedData<LivingCost>> GetLivingCostDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
