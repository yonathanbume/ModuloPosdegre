using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IUserResearchLineService
    {
        Task<IEnumerable<object>> GetUserResearchLines();
        Task<DataTablesStructs.ReturnedData<object>> GetUserResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task Accept(Guid id);
        Task Deny(Guid id);
        Task DeleteById(Guid id);
        Task Insert(UserResearchLine userResearchLine);
    }
}
