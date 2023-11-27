using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayService(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<bool> IsHoliday(DateTime date)
        {
            return await _holidayRepository.IsHoliday(date);
        }

        public async Task<bool> AnyHolidayByName(string name, Guid? id)
        {
            return await _holidayRepository.AnyHolidayByName(name, id);
        }

        public async Task<bool> AnyHolidayByDate(DateTime date, Guid? id)
        {
            return await _holidayRepository.AnyHolidayByDate(date, id);
        }

        public async Task<Holiday> Get(Guid id)
        {
            return await _holidayRepository.Get(id);
        }

        public async Task<object> GetHoliday(Guid id)
        {
            return await _holidayRepository.GetHoliday(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHolidaysDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            return await _holidayRepository.GetHolidaysDatatable(sentParameters, searchValue);
        }

        public async Task DeleteById(Guid id)
        {
            await _holidayRepository.DeleteById(id);
        }

        public async Task Insert(Holiday holiday)
        {
            await _holidayRepository.Insert(holiday);
        }

        public async Task Update(Holiday holiday)
        {
            await _holidayRepository.Update(holiday);
        }

        public async Task<List<Holiday>> GetHolidayByRange(DateTime start, DateTime end)
            => await _holidayRepository.GetHolidayByRange(start, end);

        public async Task Delete(Holiday entity)
            => await _holidayRepository.Delete(entity);
    }
}
