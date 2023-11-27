using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IHolidayService
    {
        Task<bool> IsHoliday(DateTime date);
        Task<bool> AnyHolidayByName(string name, Guid? id);
        Task<bool> AnyHolidayByDate(DateTime Date, Guid? id);
        Task<object> GetHoliday(Guid id);
        Task<Holiday> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetHolidaysDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task DeleteById(Guid id);
        Task Insert(Holiday holiday);
        Task Update(Holiday holiday);
        Task<List<Holiday>> GetHolidayByRange(DateTime start, DateTime end);
        Task Delete(Holiday entity);
    }
}
