using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IScoreInputScheduleDetailService
    {
        Task DeleteRange(IEnumerable<ScoreInputScheduleDetail> scoreInputScheduleDetails);
        Task UpdateRange(IEnumerable<ScoreInputScheduleDetail> scoreInputScheduleDetails);
        Task<IEnumerable<ScoreInputScheduleDetail>> GetAllByFilter(Guid? scoreInputScheduleId = null);
    }
}