using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ISupplyService
    {
        Task Insert(Supply newSupply);
        Task<Supply> Get(Guid id);
        Task Update(Supply editing);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<Supply>> GetSuppliesDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task<object> GetSupplySelect(Guid providerId);
        Task<Select2Structs.ResponseParameters> GetSuppliesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
    }
}
