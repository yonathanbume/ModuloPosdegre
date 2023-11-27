using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface IShortcutService
    {
        Task InsertShortcut(Shortcut shortcut);
        Task UpdateShortcut(Shortcut shortcut);
        Task DeleteShortcut(Shortcut shortcut);
        Task<Shortcut> GetShortcutById(Guid id);
        Task<DataTablesStructs.ReturnedData<ShortcutTemplate>> GetAllShortcutDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title);
        Task<EditShortcutTemplate> GetShortcutTemplateById(Guid id);

        
        Task<DataTablesStructs.ReturnedData<SubShortcutTemplate>> GetAllSubShortcutDatatable(DataTablesStructs.SentParameters sentParameters, Guid ShortcutId);
        Task InsertSubShortcut(SubShortcut subshortcut);
        Task UpdateSubShortcut(SubShortcut subshortcut);
        Task DeleteSubShortcut(SubShortcut subshortcut);
        Task<SubShortcut> GetSubShortcutById(Guid id);
        Task<SubShortcut> GetSubShortcutByIdAndShortcutId(Guid ShortcutId, Guid id);
    }
}
