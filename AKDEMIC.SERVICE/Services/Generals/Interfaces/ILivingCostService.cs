using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ILivingCostService
    {
        Task Insert(LivingCost entity);
        Task Update(LivingCost entity);
        Task<LivingCost> Get(Guid id);
        Task GetAll();
        Task<LivingCost> First();
        Task<DataTablesStructs.ReturnedData<LivingCost>> GetLivingCostDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task DeleteById(Guid id);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
