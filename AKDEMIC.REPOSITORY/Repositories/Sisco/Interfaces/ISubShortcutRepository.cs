using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface ISubShortcutRepository : IRepository<SubShortcut>
    {
        Task<DataTablesStructs.ReturnedData<SubShortcutTemplate>> GetAllSubShortcutDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<SubShortcut> GetSubShortcutByIdAndShortcutId(Guid ShortcutId, Guid id);
    }
}
