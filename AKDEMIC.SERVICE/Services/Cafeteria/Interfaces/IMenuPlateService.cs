using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IMenuPlateService
    {
        Task<DataTablesStructs.ReturnedData<MenuPlate>> GetMenuDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<Guid> InsertAndReturnId(MenuPlate menu);
        Task<DataTablesStructs.ReturnedData<MenuPlateSupply>> GetMenuSuppliesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue);
        Task AddSupply(MenuPlateSupply menuPlateSupply);
        Task DeleteSupplyById(Guid supplyId);
        Task<IEnumerable<MenuPlate>> GetAll();
        Task<IEnumerable<Select2Structs.Result>> GetMenusSelect2ClientSide(Guid? menuId=null);
        Task Insert(MenuPlate menuPlate);
        Task<DataTablesStructs.ReturnedData<object>> GetMenuPlatesDatatable(DataTablesStructs.SentParameters sentParameters, Guid MenuPlateId, string searchValue);
        Task<Tuple<bool, string>> AddMenuPlateSupply(Guid MenuPlateId, Guid ProviderSupplyId, Guid CafeteriaWeeklyScheduleId, decimal Quantity, byte TurnId);
        Task DeleteMenuSupplyById(Guid menuSupplyId);
        Task<MenuPlate> Get(Guid id);
        Task Update(MenuPlate model);
    }
}
