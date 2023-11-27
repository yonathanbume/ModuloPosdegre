using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectItemScoreService
    {
        Task<IEnumerable<ProjectItemScore>> GetAllByProjectAdvanceId(Guid projectAdvanceId);
        Task InsertRange(IEnumerable<ProjectItemScore> entities);
    }
}
