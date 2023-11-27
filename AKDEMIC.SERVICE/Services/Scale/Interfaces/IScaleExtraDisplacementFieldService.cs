using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraDisplacementFieldService
    {
        Task<ScaleExtraDisplacementField> Get(Guid id);
        Task Insert(ScaleExtraDisplacementField entity);
        Task Update(ScaleExtraDisplacementField entity);
        Task Delete(ScaleExtraDisplacementField entity);
        Task<ScaleExtraDisplacementField> GetScaleExtraDisplacementFieldByResolutionId(Guid resolutionId);
        Task<IEnumerable<ScaleExtraDisplacementField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);

        Task<DataTablesStructs.ReturnedData<object>> GetDisplacementRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<string> GetScalefieldByUserId(string id);
    }
}
