using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class InstitutionalAlertService : IInstitutionalAlertService
    {
        private readonly IInstitutionalAlertRepository _institutionalAlertRepository;

        public InstitutionalAlertService(IInstitutionalAlertRepository institutionalAlertRepository)
        {
            _institutionalAlertRepository = institutionalAlertRepository;
        }

        public async Task InsertInstitutionalAlert(InstitutionalAlert institutionalAlert) =>
            await _institutionalAlertRepository.Insert(institutionalAlert);

        public async Task UpdateInstitutionalAlert(InstitutionalAlert institutionalAlert) =>
            await _institutionalAlertRepository.Update(institutionalAlert);

        public async Task DeleteInstitutionalAlert(InstitutionalAlert institutionalAlert) =>
            await _institutionalAlertRepository.Delete(institutionalAlert);

        public async Task<InstitutionalAlert> GetInstitutionalAlertById(Guid id) =>
            await _institutionalAlertRepository.Get(id);

        public async Task<IEnumerable<InstitutionalAlert>> GetAllInstitutionalAlerts() =>
            await _institutionalAlertRepository.GetAll();
        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertGroupByDependency(DateTime? startDate = null, DateTime? endDate = null)
            => await _institutionalAlertRepository.GetInstitutionalAlertGroupByDependency(startDate, endDate);

        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsByDependecyId(Guid dependencyId)
            => await _institutionalAlertRepository.GetInstitutionalAlertsByDependecyId(dependencyId);

        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsCountGroupByStatus(Guid dependecyId)
            => await _institutionalAlertRepository.GetInstitutionalAlertsCountGroupByStatus(dependecyId);

        public async Task<List<InstitutionalAlertTemplate>> GetInstitutionalAlerts()
            => await _institutionalAlertRepository.GetInstitutionalAlerts();

        public async Task<InstitutionalAlertDetailTemplate> GetAlertReponse(Guid id)
            => await _institutionalAlertRepository.GetAlertReponse(id);
    }
}
