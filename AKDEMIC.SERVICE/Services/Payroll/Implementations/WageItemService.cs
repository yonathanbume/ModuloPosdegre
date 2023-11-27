using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WageItemService : IWageItemService
    {
        private readonly IWageItemRepository _wageItemRepository;

        public WageItemService(IWageItemRepository wageItemRepository)
        {
            _wageItemRepository = wageItemRepository;
        }

        public async Task DeleteById(Guid id)
            => await _wageItemRepository.DeleteById(id);

        public async Task<WageItem> Get(Guid id)
            => await _wageItemRepository.Get(id);

        public async Task<IEnumerable<WageItem>> GetAll()
            => await _wageItemRepository.GetAll();


        public async Task Insert(WageItem wageItem)
            => await _wageItemRepository.Insert(wageItem);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                    => _wageItemRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task Update(WageItem wageItem)
            => await _wageItemRepository.Update(wageItem);
    }
}
