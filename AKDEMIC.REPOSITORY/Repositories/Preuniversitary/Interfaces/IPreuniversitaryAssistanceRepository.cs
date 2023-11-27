using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryAssistanceRepository : IRepository<PreuniversitaryAssistance>
    {
        Task<PreuniversitaryAssistance> GetByScheduleAndCurrentDate(Guid scheduleId);
        Task<List<PreuniversitaryUserGroupTemplate>> GetAssistancesStudentByAssistanceId(PreuniversitaryAssistance entity, string userId, Guid scheduleId, Guid assistanceId);
        Task InsertRangeStudentAssistance(IEnumerable<PreuniversitaryAssistanceStudent> entities);
    }
}
