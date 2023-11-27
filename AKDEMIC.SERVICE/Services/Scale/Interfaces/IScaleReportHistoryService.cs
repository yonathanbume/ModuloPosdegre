using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleReportHistoryService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetScaleReportHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task<ScaleReportHistory> Add(ScaleReportHistory scaleReportHistory);
        Task Insert(ScaleReportHistory scaleReportHistory);
        Task<int> Count();
    }
}
