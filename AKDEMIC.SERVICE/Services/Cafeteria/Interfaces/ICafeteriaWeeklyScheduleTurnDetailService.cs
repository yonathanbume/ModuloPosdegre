using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ICafeteriaWeeklyScheduleTurnDetailService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaWeeklyScheduleTurnDetailDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid cafeteriaWeeklyScheduleId);
        Task Insert(CafeteriaWeeklyScheduleTurnDetail entity);
        Task Update(CafeteriaWeeklyScheduleTurnDetail entity);
        Task<CafeteriaWeeklyScheduleTurnDetail> Get(Guid id);
        Task Delete (CafeteriaWeeklyScheduleTurnDetail model);
        Task<Tuple<bool, string>> ValidateTurnAndHour(int Type, Guid CafeteriaWeeklyScheduleIdParameter, TimeSpan StartTimeParameter, TimeSpan EndTimeParameter, Guid? Id = null);
    }
}
