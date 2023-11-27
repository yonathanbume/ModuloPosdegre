using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleReportHistoryRepository:IRepository<ScaleReportHistory>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetScaleReportHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
