using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraDisplacementFieldRepository : IRepository<ScaleExtraDisplacementField>
    {
        Task<ScaleExtraDisplacementField> GetScaleExtraDisplacementFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraDisplacementField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);
        Task<DataTablesStructs.ReturnedData<object>> GetDisplacementRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<string> GetScalefieldByUserId(string id);
    }
}
