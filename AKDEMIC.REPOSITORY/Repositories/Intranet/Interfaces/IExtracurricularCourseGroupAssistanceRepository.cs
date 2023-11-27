using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtracurricularCourseGroupAssistanceRepository : IRepository<ExtracurricularCourseGroupAssistance>
    {
        Task<IEnumerable<ExtracurricularCourseGroupAssistance>> GetAllByGroup(Guid groupId);
    }
}
