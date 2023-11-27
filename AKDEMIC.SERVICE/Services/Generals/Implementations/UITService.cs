using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class UITService : IUITService
    {
        private readonly IUITRepository _uitRepository;

        public UITService(IUITRepository uitRepository)
        {
            _uitRepository = uitRepository;
        }

        public async Task<bool> AnyUITByYear(int year)
        {
            return await _uitRepository.AnyUITByYear(year);
        }

        public async Task<UIT> Get(Guid id)
        {
            return await _uitRepository.Get(id);
        }

        public async Task<UIT> GetCurrentUIT()
        {
            return await _uitRepository.GetCurrentUIT();
        }

        public async Task<IEnumerable<UIT>> GetAll()
        {
            return await _uitRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<UIT>> GetUITsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _uitRepository.GetUITsDatatable(sentParameters, searchValue);
        }

        public async Task Delete(UIT uit) =>
            await _uitRepository.Delete(uit);

        public async Task Insert(UIT uit) =>
            await _uitRepository.Insert(uit);

        public async Task Update(UIT uit) =>
            await _uitRepository.Update(uit);
        public async Task<UIT> LastOrDefaultAsync()
            => await _uitRepository.Last();
    }
}
