using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface IShortcutRepository : IRepository<Shortcut>
    {
        Task<DataTablesStructs.ReturnedData<ShortcutTemplate>> GetAllShortcutDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null);
        Task<EditShortcutTemplate> GetShortcutById(Guid id);
    }
}
