using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleReportProfileDetailService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDetailDataTable(DataTablesStructs.SentParameters sentParameters,Guid profileId, string searchValue = null);
        Task<bool> AnyBySectionNumber(int sectionNumber, Guid reportProfileId, Guid? id = null);
        Task<ScaleReportProfileDetail> Get(Guid id);
        Task<List<int>> GetSectionsByProfile(Guid profileId);
        Task Insert(ScaleReportProfileDetail scaleReportProfileDetail);
        Task Delete(ScaleReportProfileDetail scaleReportProfileDetail);
        Task Update(ScaleReportProfileDetail scaleReportProfileDetail);        
    }
}
