using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleReportProfileDetailRepository: IRepository<ScaleReportProfileDetail>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDetailDataTable(DataTablesStructs.SentParameters sentParameters,Guid profileId, string searchValue = null);
        Task<bool> AnyBySectionNumber(int sectionNumber, Guid reportProfileId, Guid? id = null);
        Task<List<int>> GetSectionsByProfile(Guid profileId);
    }
}
