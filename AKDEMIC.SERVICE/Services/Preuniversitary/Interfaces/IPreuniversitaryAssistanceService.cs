using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryAssistanceService
    {
        Task<PreuniversitaryAssistance> GetByScheduleAndCurrentDate(Guid scheduleId);
        Task<List<PreuniversitaryUserGroupTemplate>> GetAssistancesStudentByAssistanceId(PreuniversitaryAssistance entity, string userId, Guid scheduleId, Guid assistanceId);
        Task Insert(PreuniversitaryAssistance entity);
        Task InsertRangeStudentAssistance(IEnumerable<PreuniversitaryAssistanceStudent> entities);
        Task Update(PreuniversitaryAssistance entity);
    }
}
