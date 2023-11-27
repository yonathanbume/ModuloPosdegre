using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        Task<bool> IsHoliday(DateTime date);
        Task<bool> AnyHolidayByName(string name, Guid? id);
        Task<bool> AnyHolidayByDate(DateTime date, Guid? id);
        Task<object> GetHoliday(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetHolidaysDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<List<Holiday>> GetHolidayByRange(DateTime start, DateTime end);
    }
}
