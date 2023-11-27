using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.WeeklyAttendanceReport;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class WeeklyAttendanceReportService : IWeeklyAttendanceReportService
    {
        private readonly IWeeklyAttendanceReportRepository _weeklyAttendanceReportRepository;

        public WeeklyAttendanceReportService(IWeeklyAttendanceReportRepository weeklyAttendanceReportRepository)
        {
            _weeklyAttendanceReportRepository = weeklyAttendanceReportRepository;
        }

        public async Task<List<SectionTemplate>> GetSectionReportData(Guid? facultyId, Guid? careerId, ClaimsPrincipal user = null)
            => await _weeklyAttendanceReportRepository.GetSectionReportData(facultyId, careerId, user);

        public async Task SaveWeeklyAttendanceReport(Guid sectionId, int week)
            => await _weeklyAttendanceReportRepository.SaveWeeklyAttendanceReport(sectionId, week);
    }
}
