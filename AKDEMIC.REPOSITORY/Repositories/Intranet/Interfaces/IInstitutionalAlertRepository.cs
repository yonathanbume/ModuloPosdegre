using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IInstitutionalAlertRepository : IRepository<InstitutionalAlert>
    {
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsCountGroupByStatus(Guid dependecyId);
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertGroupByDependency(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsByDependecyId(Guid dependencyId);
        Task<List<InstitutionalAlertTemplate>> GetInstitutionalAlerts();
        Task<InstitutionalAlertDetailTemplate> GetAlertReponse(Guid id);
    }
}
