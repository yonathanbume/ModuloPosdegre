using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.WeeklyAttendanceReport;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IWeeklyAttendanceReportRepository : IRepository<WeeklyAttendanceReport>
    {
        Task SaveWeeklyAttendanceReport(Guid sectionId, int week);
        Task<List<SectionTemplate>> GetSectionReportData(Guid? facultyId, Guid? careerId, ClaimsPrincipal user = null);
    }
}
