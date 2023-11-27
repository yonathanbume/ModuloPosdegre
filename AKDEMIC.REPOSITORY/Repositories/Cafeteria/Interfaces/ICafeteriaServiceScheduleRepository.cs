using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ICafeteriaServiceScheduleRepository : IRepository<CafeteriaServiceTermSchedule>
    {
        Task<DataTablesStructs.ReturnedData<CafeteriaServiceTermSchedule>> GetCafeteriaServiceTermScheduleDataTable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<DataTablesStructs.ReturnedData<CafeteriaWeeklySchedule>> GetDaysDataTable(DataTablesStructs.SentParameters sentParameters, Guid weekId, string search);
        Task UpdateDays(CafeteriaWeeklySchedule dayofweek);
        Task InsertDays(CafeteriaWeeklySchedule dayofweek);
        Task<CafeteriaServiceTermSchedule> InsertWeek(CafeteriaServiceTermSchedule cafeteriaServiceTermSchedule);
        Task DeleteDayById(Guid id);
        Task<CafeteriaServiceTermSchedule> GetDates();
        Task<bool> ValidateDayOfWeek(Guid weekId, int day);
        Task InsertAssistance(UserCafeteriaDailyAssistance newassistance);
        Task<CafeteriaServiceTermSchedule> GetActiveWeek();
        Task<CafeteriaWeeklyScheduleTurnDetail> GetActiveDayAndTurn(Guid CafeteriaTermScheduleId);
        Task<Select2Structs.ResponseParameters> GetWeeksSelect2ClientSide(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<CafeteriaWeeklySchedule> GetDayData(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDayByWeekGroupBy(DataTablesStructs.SentParameters sentParameters, Guid weekId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTurnsByWeeklyScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid cafeteriaWeeklyScheduleId, int dayOfWeek, string searchValue = null);
        Task<List<MenuPlateSupply>> GetMenuPlateByWeeklySchedule(Guid MenuPlateId);
        Task<bool> ValidateWeekByTerm(DateTime dateBegin, DateTime dateEnd);
        Task<byte> GetTurnByMenuPlate(Guid MenuPlateId);

        //NUEVO
        Task<bool> ValidateDayByWeek(Guid cafeteriaServiceTermScheduleId, int dayOfWeek, Guid? cafeteriaWeeklyScheduleId = null);
    }
}
