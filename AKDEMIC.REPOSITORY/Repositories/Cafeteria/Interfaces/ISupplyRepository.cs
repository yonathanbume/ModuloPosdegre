using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ISupplyRepository : IRepository<Supply>
    {
        Task<DataTablesStructs.ReturnedData<Supply>> GetSuppliesDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task<Select2Structs.ResponseParameters> GetSuppliesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<object> GetSupplySelect(Guid providerId);
    }
}
