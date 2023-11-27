using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IUserResearchLineRepository
    {
        Task<int> Count();
        Task<IEnumerable<UserResearchLine>> GetAll();
        Task<IEnumerable<object>> GetUserResearchLines();
        Task<DataTablesStructs.ReturnedData<object>> GetUserResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task Accept(Guid id);
        Task Deny(Guid id);
        Task DeleteById(Guid id);
        Task Insert(UserResearchLine userResearchLine);
        Task Update(UserResearchLine userResearchLine);
    }
}
