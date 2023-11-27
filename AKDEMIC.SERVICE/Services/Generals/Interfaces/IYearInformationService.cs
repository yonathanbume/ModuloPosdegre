using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IYearInformationService
    {
        Task<YearInformation> Get(Guid id);
        Task<string> GetNameByYear(int year);
        Task<bool> AnyByYear(int year, Guid? id = null);
        Task<IEnumerable<YearInformation>> GetAll();
        Task Insert(YearInformation yearInformation);
        Task Update(YearInformation yearInformation);
        Task Delete(YearInformation yearInformation);
        Task<DataTablesStructs.ReturnedData<object>> GetAllYearInformationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
