using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ICafeteriaWeeklyScheduleTurnDetailRepository : IRepository<CafeteriaWeeklyScheduleTurnDetail>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaWeeklyScheduleTurnDetailDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid cafeteriaWeeklyScheduleId);
        Task<Tuple<bool, string>> ValidateTurnAndHour(int Type, Guid CafeteriaWeeklyScheduleIdParameter, TimeSpan StartTimeParameter, TimeSpan EndTimeParameter, Guid? Id = null);

    }
}
