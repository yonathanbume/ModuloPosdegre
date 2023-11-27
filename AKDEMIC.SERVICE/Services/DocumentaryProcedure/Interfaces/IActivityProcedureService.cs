using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IActivityProcedureService
    {
        Task InsertActivityProcedure(ActivityProcedure activityProcedure);
        Task UpdateActivityProcedure(ActivityProcedure activityProcedure);
        Task DeleteActivityProcedure(ActivityProcedure activityProcedure);
        Task<ActivityProcedure> GetActivityProcedureById(Guid id);
        Task<IEnumerable<ActivityProcedure>> GetAllActivityProcedures();
        Task<DataTablesStructs.ReturnedData<object>> GetActivityProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
