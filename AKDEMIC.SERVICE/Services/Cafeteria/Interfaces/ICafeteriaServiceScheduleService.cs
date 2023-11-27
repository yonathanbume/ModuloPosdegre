using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ICafeteriaServiceScheduleService
    {
        Task<DataTablesStructs.ReturnedData<CafeteriaServiceTermSchedule>> GetCafeteriaServiceTermScheduleDataTable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<DataTablesStructs.ReturnedData<CafeteriaWeeklySchedule>> GetDaysDataTable(DataTablesStructs.SentParameters sentParameters, Guid weekId, string search = null);
        Task<CafeteriaServiceTermSchedule> Get(Guid id);
        Task Update(CafeteriaServiceTermSchedule week);
        Task InsertDays(CafeteriaWeeklySchedule dayofweek);
        Task<CafeteriaServiceTermSchedule> InsertWeek(CafeteriaServiceTermSchedule cafeteriaServiceTermSchedule);
        Task<CafeteriaServiceTermSchedule> GetDates();
        Task DeleteDayById(Guid id);
        Task<bool> ValidateDayOfWeek(Guid weekId, int day);
        Task InsertAssistance(UserCafeteriaDailyAssistance newassistance);
        Task<CafeteriaServiceTermSchedule> GetActiveWeek();
        Task<Select2Structs.ResponseParameters> GetWeeksSelect2ClientSide(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<CafeteriaWeeklyScheduleTurnDetail> GetActiveDayAndTurn(Guid CafeteriaTermScheduleId);
        Task<CafeteriaWeeklySchedule> GetDayData(Guid id);
        Task UpdateDays(CafeteriaWeeklySchedule dayofweek);
        Task<DataTablesStructs.ReturnedData<object>> GetDayByWeekGroupBy(DataTablesStructs.SentParameters sentParameters, Guid weekId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTurnsByWeeklyScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid cafeteriaWeeklyScheduleId, int dayOfWeek, string searchValue = null);
        Task<List<MenuPlateSupply>> GetMenuPlateByWeeklySchedule(Guid MenuPlateId);
        Task<bool> ValidateWeekByTerm(DateTime dateBegin, DateTime dateEnd);
        Task<byte> GetTurnByMenuPlate(Guid MenuPlateId);
        Task<bool> ValidateDayByWeek(Guid cafeteriaServiceTermScheduleId, int dayOfWeek, Guid? cafeteriaWeeklyScheduleId = null);

    }
}
