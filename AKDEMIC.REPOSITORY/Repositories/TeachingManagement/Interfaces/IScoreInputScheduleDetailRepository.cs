using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IScoreInputScheduleDetailRepository : IRepository<ScoreInputScheduleDetail>
    {
        Task<IEnumerable<ScoreInputScheduleDetail>> GetAllByFilter(Guid? scoreInputScheduleId = null);
    }
}