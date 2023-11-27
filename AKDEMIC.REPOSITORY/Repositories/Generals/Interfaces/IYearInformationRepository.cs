using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IYearInformationRepository: IRepository<YearInformation>
    {
        Task<bool> AnyByYear(int year, Guid? id = null);
        Task<string> GetNameByYear(int year);
        Task<DataTablesStructs.ReturnedData<object>> GetAllYearInformationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
