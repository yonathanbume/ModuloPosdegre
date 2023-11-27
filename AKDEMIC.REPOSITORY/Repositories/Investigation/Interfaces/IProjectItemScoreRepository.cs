using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectItemScoreRepository : IRepository<ProjectItemScore>
    {
        Task<IEnumerable<ProjectItemScore>> GetAllByProjectAdvanceId(Guid projectAdvanceId);
    }
}
