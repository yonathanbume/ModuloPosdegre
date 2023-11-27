using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleReportProfileRepository: IRepository<ScaleReportProfile>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDataTable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? id = null);
    }
}
