using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.InstitutionalAlert;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class InstitutionalAlertRepository : Repository<InstitutionalAlert> , IInstitutionalAlertRepository
    {
        public InstitutionalAlertRepository(AkdemicContext context):base(context) { }

        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsCountGroupByStatus(Guid dependecyId)
        {
            var result = await _context.InstitutionalAlerts.Where(x => x.DependencyId == dependecyId)
                .GroupBy(x => x.Status)
                .Select(x => new InstitutionalAlert
                {
                    AlertStatus = x.Key ? ConstantHelpers.INSTITUTIONAL_ALERT.ALERT_STATUS.ACTIVE : ConstantHelpers.INSTITUTIONAL_ALERT.ALERT_STATUS.INACTIVE,
                    AlertCount = x.Count()
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertGroupByDependency(DateTime? startDate=null, DateTime? endDate=null)
        {
            var query = _context.InstitutionalAlerts.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.RegisterDate <= endDate && x.RegisterDate >= startDate);

            var result =  query
                .AsEnumerable()
                .GroupBy(x => x.Dependency)
                .Select(x => new InstitutionalAlert
                {
                    Dependency = new Dependency
                    {
                        Name = x.Key.Name
                    },
                    AlertCount = x.Count()
                })
                .ToArray();

            return result;
        }

        public async Task<IEnumerable<InstitutionalAlert>> GetInstitutionalAlertsByDependecyId(Guid dependencyId)
            => await _context.InstitutionalAlerts.Where(x => x.DependencyId == dependencyId).ToArrayAsync();

        public async Task<List<InstitutionalAlertTemplate>> GetInstitutionalAlerts()
        {
            var result = await _context.InstitutionalAlerts
                .Include(x => x.Applicant)
                .Include(x => x.Dependency)
                .Include(x => x.Applicant.UserDependencies)
                .Select(x => new InstitutionalAlertTemplate
                {
                    Id = x.Id,
                    Applicant = x.Applicant.FullName,
                    Dependency = x.Dependency.Name,
                    Register = x.RegisterDate.ToLocalTime().ToString("dd/MM/yyyy hh:mm"),
                    RegisterDateTime = x.RegisterDate.ToLocalTime(),
                    Status = x.Status ? ConstantHelpers.INSTITUTIONAL_ALERT.ALERT_STATUS.ACTIVE : ConstantHelpers.INSTITUTIONAL_ALERT.ALERT_STATUS.INACTIVE
                }).OrderByDescending(x => x.RegisterDateTime)
                .ToListAsync();

            return result;
        }

        public async Task<InstitutionalAlertDetailTemplate> GetAlertReponse(Guid id)
        {
            var result = await _context.InstitutionalAlerts
                        .Include(x => x.Assistant)
                        .Include(x => x.Dependency)
                         .Select(x => new InstitutionalAlertDetailTemplate
                         {
                             Id = x.Id,
                             Applicant = x.Applicant.FullName,
                             Assistant = x.AssistantId != null ? x.Assistant.FullName : "",
                             Dependency = x.Dependency.Name,
                             RegisterDate = x.RegisterDate.ToString("dd/MM/yyyy"),
                             Status = x.Status,
                             Type = x.Type ?? 0,
                             Description = x.Description
                         }).FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
