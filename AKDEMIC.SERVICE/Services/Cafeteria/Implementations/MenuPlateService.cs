using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class MenuPlateService : IMenuPlateService
    {
        private readonly IMenuPlateRepository _menuPlateRepository;
        public MenuPlateService(IMenuPlateRepository menuPlateRepository)
        {
            _menuPlateRepository = menuPlateRepository;
        }

        public async Task<Tuple<bool, string>> AddMenuPlateSupply(Guid MenuPlateId, Guid ProviderSupplyId, Guid CafeteriaWeeklyScheduleId, decimal Quantity, byte TurnId)
        {
            return await _menuPlateRepository.AddMenuPlateSupply(MenuPlateId, ProviderSupplyId, CafeteriaWeeklyScheduleId, Quantity, TurnId);
        }

        public async Task AddSupply(MenuPlateSupply menuPlateSupply)
        {
            await _menuPlateRepository.AddSupply(menuPlateSupply);
        }

        public async Task DeleteMenuSupplyById(Guid menuSupplyId)
        {
            await _menuPlateRepository.DeleteMenuSupplyById(menuSupplyId);
        }

        public async Task DeleteSupplyById(Guid supplyId)
        {
            await _menuPlateRepository.DeleteSupplyById(supplyId);
        }

        public async Task<MenuPlate> Get(Guid id)
        {
            return await _menuPlateRepository.Get(id);
        }

        public async Task<IEnumerable<MenuPlate>> GetAll()
        {
            return await _menuPlateRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<MenuPlate>> GetMenuDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            return await _menuPlateRepository.GetMenuDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetMenuPlatesDatatable(DataTablesStructs.SentParameters sentParameters, Guid MenuPlateId, string searchValue)
        {
            return await _menuPlateRepository.GetMenuPlatesDatatable(sentParameters, MenuPlateId, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetMenusSelect2ClientSide(Guid? menuId = null)
        {
            return await _menuPlateRepository.GetMenusSelect2ClientSide(menuId);
        }

        public async Task<DataTablesStructs.ReturnedData<MenuPlateSupply>> GetMenuSuppliesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue)
        {
            return await _menuPlateRepository.GetMenuSuppliesDatatable(sentParameters, id, searchValue);
        }

        public async Task Insert(MenuPlate menuPlate)
        {
            await _menuPlateRepository.Insert(menuPlate);
        }

        public async Task<Guid> InsertAndReturnId(MenuPlate menu)
        {
            return await _menuPlateRepository.InsertAndReturnId(menu);
        }

        public async Task Update(MenuPlate model)
        {
            await _menuPlateRepository.Update(model);
        }
    }
}
