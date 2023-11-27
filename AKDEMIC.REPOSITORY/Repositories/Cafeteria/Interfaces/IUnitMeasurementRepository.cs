using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IUnitMeasurementRepository : IRepository<UnitMeasurement>
    {
        Task<object> GetUnitMeasurementSelect();
        Task<DataTablesStructs.ReturnedData<object>> GetUnitMeasurementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
