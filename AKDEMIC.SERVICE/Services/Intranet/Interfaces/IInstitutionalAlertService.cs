using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IInstitutionalAlertService
    {
        Task InsertInstitutionalAlert(InstitutionalAlert institutionalAlert);
        Task UpdateInstitutionalAlert(InstitutionalAlert institutionalAlert);
        Task DeleteInstitutionalAlert(InstitutionalAlert institutionalAlert);
        Task<InstitutionalAlert> GetInstitutionalAlertById(Guid id);
        Task<IEnumerable<InstitutionalAlert>> GetAllInstitutionalAlerts();
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsCountGroupByStatus(Guid dependecyId);
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertGroupByDependency(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsByDependecyId(Guid dependencyId);
        Task<List<InstitutionalAlertTemplate>> GetInstitutionalAlerts();
        Task<InstitutionalAlertDetailTemplate> GetAlertReponse(Guid id);
    }
}
