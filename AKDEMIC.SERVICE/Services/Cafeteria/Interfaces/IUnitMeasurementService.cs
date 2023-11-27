using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IUnitMeasurementService
    {
        Task<object> GetUnitMeasurementSelect();
        Task<DataTablesStructs.ReturnedData<object>> GetUnitMeasurementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(UnitMeasurement unitMeasurement);
        Task Update(UnitMeasurement unitMeasurement);
        Task Delete(UnitMeasurement unitMeasurement);
        Task<UnitMeasurement> Get(Guid id);
    }
}
