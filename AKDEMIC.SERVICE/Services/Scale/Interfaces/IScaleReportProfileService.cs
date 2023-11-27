using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleReportProfileService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDataTable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<ScaleReportProfile> Get(Guid id);
        Task<IEnumerable<ScaleReportProfile>> GetAll();
        Task<bool> AnyByName(string name, Guid? id = null);
        Task Insert(ScaleReportProfile scaleReportProfile);
        Task Delete(ScaleReportProfile scaleReportProfile);
        Task Update(ScaleReportProfile scaleReportProfile);
        Task DeleteById(Guid id);
    }
}
