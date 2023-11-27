using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectAdvanceHistoricService
    {
        Task<IEnumerable<ProjectAdvanceHistoric>> GetAllbyProjectAdvanceId(Guid projectAdvanceId);
        Task Insert(ProjectAdvanceHistoric entity);
        Task<ProjectAdvanceHistoric> Get(Guid id);
        Task Update(ProjectAdvanceHistoric entity);
    }
}
