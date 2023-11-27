using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IUserRequirementItemRepository : IRepository<UserRequirementItem>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetUserRequirementItmDatatable(DataTablesStructs.SentParameters sentParameters, Guid itemId, string search = null);
        Task<object> GetAllUserRequerimentItemsByUserRequerimentId(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<UserRequirementItem> GetByUserRequrimentId(Guid userRequirementId);
        Task<object> GetByUserRequrimentDetailId(Guid userRequirementId);
        Task SaveChanges();
    }
}
