using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class ShortcutService : IShortcutService
    {
        private readonly IShortcutRepository _shortcutRepository;
        private readonly ISubShortcutRepository _subShortcutRepository;

        public ShortcutService(IShortcutRepository shortcutRepository,
            ISubShortcutRepository subShortcutRepository)
        {
            _shortcutRepository = shortcutRepository;
            _subShortcutRepository = subShortcutRepository;
        }

        #region SHORTCUT
        public async Task InsertShortcut(Shortcut shortcut) =>
            await _shortcutRepository.Insert(shortcut);

        public async Task UpdateShortcut(Shortcut shortcut) =>
            await _shortcutRepository.Update(shortcut);

        public async Task DeleteShortcut(Shortcut shortcut) =>
            await _shortcutRepository.Delete(shortcut);

        public async Task<Shortcut> GetShortcutById(Guid id) =>
            await _shortcutRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<ShortcutTemplate>> GetAllShortcutDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title) =>
            await _shortcutRepository.GetAllShortcutDatatable(sentParameters, type, title);

        public async Task<EditShortcutTemplate> GetShortcutTemplateById(Guid id) =>
            await _shortcutRepository.GetShortcutById(id);

        #endregion


        #region SUBSHORTCUT
        public async Task<DataTablesStructs.ReturnedData<SubShortcutTemplate>> GetAllSubShortcutDatatable(DataTablesStructs.SentParameters sentParameters, Guid ShortcutId) =>
            await _subShortcutRepository.GetAllSubShortcutDatatable(sentParameters, ShortcutId);

        public async Task InsertSubShortcut(SubShortcut subShortcut) =>
            await _subShortcutRepository.Insert(subShortcut);

        public async Task UpdateSubShortcut(SubShortcut subShortcut) =>
            await _subShortcutRepository.Update(subShortcut);

        public async Task DeleteSubShortcut(SubShortcut subShortcut) =>
            await _subShortcutRepository.Delete(subShortcut);

        public async Task<SubShortcut> GetSubShortcutById(Guid id) =>
            await _subShortcutRepository.Get(id);

        public async Task<SubShortcut> GetSubShortcutByIdAndShortcutId(Guid ShortcutId, Guid id) =>
            await _subShortcutRepository.GetSubShortcutByIdAndShortcutId(ShortcutId, id);
        #endregion
    }
}
