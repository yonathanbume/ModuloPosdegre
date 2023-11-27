using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.WeeklyAttendanceReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class WeeklyAttendanceReportRepository : Repository<WeeklyAttendanceReport> , IWeeklyAttendanceReportRepository
    {
        public WeeklyAttendanceReportRepository(AkdemicContext context) : base(context) { }

        public async Task SaveWeeklyAttendanceReport(Guid sectionId, int week)
        {
            try
            {
                var weeklyAttendanceReport = await _context.WeeklyAttendanceReports.Where(x => x.SectionId == sectionId && x.Week == week).FirstOrDefaultAsync();
                var countClasses = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId && x.WeekNumber == week && x.IsDictated).CountAsync();
                var classStudents = await _context.ClassStudents.Where(x => x.Class.ClassSchedule.SectionId == sectionId && x.Class.WeekNumber == week && x.Class.IsDictated).ToListAsync();

                if (weeklyAttendanceReport is null)
                {
                    weeklyAttendanceReport = new WeeklyAttendanceReport
                    {
                        Week = week,
                        SectionId = sectionId,
                        Absences = classStudents.Where(x => x.IsAbsent).Count(),
                        Attendances = classStudents.Where(x => !x.IsAbsent).Count(),
                        AverageAttendances = Math.Round((classStudents.Where(x=>!x.IsAbsent).Count() / (decimal)countClasses),2,MidpointRounding.AwayFromZero),
                        AverageAbsences = Math.Round((classStudents.Where(x => x.IsAbsent).Count() / (decimal)countClasses),2,MidpointRounding.AwayFromZero),
                        AttendancePercentage = Math.Round((classStudents.Where(x => !x.IsAbsent).Count() * 100M / classStudents.Count()),2,MidpointRounding.AwayFromZero)
                    };

                    await _context.WeeklyAttendanceReports.AddAsync(weeklyAttendanceReport);
                }
                else
                {
                    weeklyAttendanceReport.Absences = classStudents.Where(x => x.IsAbsent).Count();
                    weeklyAttendanceReport.Attendances = classStudents.Where(x => !x.IsAbsent).Count();
                    weeklyAttendanceReport.AverageAttendances = Math.Round((classStudents.Where(x => !x.IsAbsent).Count() / (decimal)countClasses),2,MidpointRounding.AwayFromZero);
                    weeklyAttendanceReport.AverageAbsences = Math.Round((classStudents.Where(x => x.IsAbsent).Count() / (decimal)countClasses),2,MidpointRounding.AwayFromZero);
                    weeklyAttendanceReport.AttendancePercentage = Math.Round((classStudents.Where(x => !x.IsAbsent).Count() * 100M / classStudents.Count()),2,MidpointRounding.AwayFromZero);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }

        public async Task<List<SectionTemplate>> GetSectionReportData(Guid? facultyId , Guid? careerId, ClaimsPrincipal user = null)
        {
            var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var query = _context.Sections.Where(x=>x.CourseTerm.TermId == termId && x.StudentSections.Any()).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x => x.CourseTerm.Course.Career.CareerDirectorId == userId || x.CourseTerm.Course.Career.AcademicCoordinatorId == userId || x.CourseTerm.Course.Career.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.CourseTerm.Course.Career.Faculty.DeanId== userId || x.CourseTerm.Course.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            var model = await
                query.Select(x => new SectionTemplate
                {
                    Course = x.CourseTerm.Course.Name,
                    Section = x.Code,
                    EnrolledStudents = x.StudentSections.Count(),
                    AcademicYear = string.Join(", ", x.CourseTerm.Course.AcademicYearCourses.Select(x=>x.AcademicYear)),
                    WeekReport = x.WeeklyAttendanceReports
                    .Select(y=> new WeekReportTemplate
                    {
                        Week = y.Week,
                        AverageAttendances = y.AverageAttendances,
                        AttendancePercentage = y.AttendancePercentage,
                        AverageAbsences = y.AverageAbsences
                    })
                    .OrderBy(y=>y.Week)
                    .ToList(),
                    AttendanceAverage = Math.Round((x.WeeklyAttendanceReports.Sum(y=>y.AverageAttendances) / x.WeeklyAttendanceReports.Count()),2,MidpointRounding.AwayFromZero),
                    AttendancePercentage = Math.Round((x.WeeklyAttendanceReports.Sum(y=>y.AttendancePercentage) / x.WeeklyAttendanceReports.Count()),2,MidpointRounding.AwayFromZero)
                })
                .ToListAsync();

            return model;
        }
    }
}
