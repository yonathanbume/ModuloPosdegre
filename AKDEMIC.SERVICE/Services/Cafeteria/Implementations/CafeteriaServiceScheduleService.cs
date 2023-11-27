using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class CafeteriaServiceScheduleService : ICafeteriaServiceScheduleService
    {
        private ICafeteriaServiceScheduleRepository _cafeteriaServiceScheduleRepository;
        public CafeteriaServiceScheduleService(ICafeteriaServiceScheduleRepository cafeteriaServiceScheduleRepository)
        {
            _cafeteriaServiceScheduleRepository = cafeteriaServiceScheduleRepository;
        }

        public async Task DeleteDayById(Guid id)
        {
            await _cafeteriaServiceScheduleRepository.DeleteDayById(id);
        }

        public async Task<CafeteriaServiceTermSchedule> Get(Guid id)
        {
            return await _cafeteriaServiceScheduleRepository.Get(id);
        }

        public async Task<CafeteriaWeeklyScheduleTurnDetail> GetActiveDayAndTurn(Guid CafeteriaTermScheduleId)
        {
            return await _cafeteriaServiceScheduleRepository.GetActiveDayAndTurn(CafeteriaTermScheduleId);
        }

        public async Task<CafeteriaServiceTermSchedule> GetActiveWeek()
        {
            return await _cafeteriaServiceScheduleRepository.GetActiveWeek();
        }

        public async Task<DataTablesStructs.ReturnedData<CafeteriaServiceTermSchedule>> GetCafeteriaServiceTermScheduleDataTable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            return await _cafeteriaServiceScheduleRepository.GetCafeteriaServiceTermScheduleDataTable(sentParameters, search);
        }

        public async Task<CafeteriaServiceTermSchedule> GetDates()
        {
            return await _cafeteriaServiceScheduleRepository.GetDates();
        }

        public async Task<CafeteriaWeeklySchedule> GetDayData(Guid id)
        {
            return await _cafeteriaServiceScheduleRepository.GetDayData(id);
        }

        public async Task<DataTablesStructs.ReturnedData<CafeteriaWeeklySchedule>> GetDaysDataTable(DataTablesStructs.SentParameters sentParameters, Guid weekId, string search = null)
        {
            return await _cafeteriaServiceScheduleRepository.GetDaysDataTable(sentParameters, weekId, search);
        }

        public async Task<Select2Structs.ResponseParameters> GetWeeksSelect2ClientSide(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _cafeteriaServiceScheduleRepository.GetWeeksSelect2ClientSide(requestParameters, searchValue);
        }

        public async Task InsertAssistance(UserCafeteriaDailyAssistance newassistance)
        {
            await _cafeteriaServiceScheduleRepository.InsertAssistance(newassistance);
        }

        public async Task InsertDays(CafeteriaWeeklySchedule dayofweek)
        {
            await _cafeteriaServiceScheduleRepository.InsertDays(dayofweek);
        }
        public async Task UpdateDays(CafeteriaWeeklySchedule dayofweek)
        {
            await _cafeteriaServiceScheduleRepository.UpdateDays(dayofweek);
        }

        public async Task<CafeteriaServiceTermSchedule> InsertWeek(CafeteriaServiceTermSchedule cafeteriaServiceTermSchedule)
        {
            return await _cafeteriaServiceScheduleRepository.InsertWeek(cafeteriaServiceTermSchedule);
        }

        public async Task Update(CafeteriaServiceTermSchedule week)
        {
            await _cafeteriaServiceScheduleRepository.Update(week);
        }

        public async Task<bool> ValidateDayOfWeek(Guid weekId, int day)
        {
            return await _cafeteriaServiceScheduleRepository.ValidateDayOfWeek(weekId, day);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDayByWeekGroupBy(DataTablesStructs.SentParameters sentParameters, Guid weekId, string searchValue = null)
        {
            return await _cafeteriaServiceScheduleRepository.GetDayByWeekGroupBy(sentParameters, weekId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTurnsByWeeklyScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid cafeteriaWeeklyScheduleId, int dayOfWeek, string searchValue = null)
        {
            return await _cafeteriaServiceScheduleRepository.GetTurnsByWeeklyScheduleDatatable(sentParameters, cafeteriaWeeklyScheduleId, dayOfWeek, searchValue);
        }

        public async Task<List<MenuPlateSupply>> GetMenuPlateByWeeklySchedule(Guid MenuPlateId)
        {
            return await _cafeteriaServiceScheduleRepository.GetMenuPlateByWeeklySchedule(MenuPlateId);
        }
         
        public async Task<bool> ValidateWeekByTerm(DateTime dateBegin, DateTime dateEnd)
        {
            return await _cafeteriaServiceScheduleRepository.ValidateWeekByTerm(dateBegin, dateEnd);
        }

        public async Task<bool> ValidateDayByWeek(Guid cafeteriaServiceTermScheduleId, int dayOfWeek, Guid? cafeteriaWeeklyScheduleId = null)
        {
            return await _cafeteriaServiceScheduleRepository.ValidateDayByWeek(cafeteriaServiceTermScheduleId, dayOfWeek, cafeteriaWeeklyScheduleId);
        }

        public async Task<byte> GetTurnByMenuPlate(Guid MenuPlateId)
        {
            return await _cafeteriaServiceScheduleRepository.GetTurnByMenuPlate(MenuPlateId);
        }
    }
}
