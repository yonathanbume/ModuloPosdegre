using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeacherDedicationRepository : Repository<TeacherDedication>, ITeacherDedicationRepository
    {
        public TeacherDedicationRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetTeacherDedicationChart(Guid? id = null)
        {
            var teacherDedication = _context.TeacherDedication
                        .AsQueryable();

            if (id != null)
            {
                teacherDedication = teacherDedication.Where(x => x.Id == id);
            }

            var data = await teacherDedication
                .Select(x => new
                {
                    Dedication = x.Name,
                    Accepted = x.Teachers.Count()
                }).ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Dedication).ToList(),
                data = data.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<IEnumerable<TeacherDedication>> GetAllWithIncludes(int? status = null)
        {
            var query = _context.TeacherDedication           
                .Include(x => x.WorkerLaborRegime)
                .AsQueryable();

            if (status != null)
                query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }

        public async Task<object> GetAllAsSelect2ClientSideAsync(bool includeTitle = false)
        {
            var result = await _context.TeacherDedication.Select(x => new
            {
                Id = $"{x.Id}",
                Text = x.Name
            }).ToListAsync();

            if (includeTitle)
                result.Insert(0, new { Id = Guid.Empty.ToString(), Text = "Todos" });

            return result;
        }

        async Task<double> ITeacherDedicationRepository.GetTeacherLessonsHours(string teacherId, Guid termId, TimeSpan startTime, TimeSpan endTime)
        {
            var termm = await _context.Terms.FindAsync(termId);

            var lessons = await _context.TeacherSchedules.Include(x => x.ClassSchedule.Section.CourseTerm.Course)
                        .Where(x => x.TeacherId == teacherId)
                        .Where(x => termm.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.ClassSchedule).SumAsync(x => (x.EndTime - x.StartTime).TotalHours) + (endTime - startTime).TotalHours;

            return lessons;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDedicationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<TeacherDedication, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.MaxLessonHours);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.MinLessonHours);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.MaxNoLessonHours);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.MinNoLessonHours);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.WorkerLaborRegime.Name);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Status);
                    break;
            }

            var query = _context.TeacherDedication.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    WorkerLaborRegime = x.WorkerLaborRegime.Name,
                    status = ConstantHelpers.STATES.VALUES.ContainsKey(x.Status) ? ConstantHelpers.STATES.VALUES[x.Status] : "Desconocido",
                    x.MaxLessonHours,
                    x.MaxNoLessonHours,
                    x.MinLessonHours,
                    x.MinNoLessonHours
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<TeacherDedication>> GetAll(string search, bool? onlyActive = false)
        {
            var query = _context.TeacherDedication.AsQueryable();
            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            return await query.ToListAsync();
        }

        public async Task<object> GetTeacherDedicationSelect()
        {
            var result = await _context.TeacherDedication
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDedicationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId ,string searchValue = null)
        {
            var teacherDedications = await _context.TeacherDedication.ToListAsync();

            var query = _context.Teachers.AsNoTracking();
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var pedagogical_hour_time_configuration = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME);

            int.TryParse(pedagogical_hour_time_configuration, out var pedagogical_hour_time);

            var tecSchedules = await _context.TeacherSchedules.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .Select(
                    x => new
                    {
                        x.TeacherId,
                        ClassSchedule = new
                        {
                            x.ClassSchedule.SessionType,
                            x.ClassSchedule.EndTimeText,
                            x.ClassSchedule.StartTimeText
                        }
                    }
                )
                .ToListAsync();

            var nonSchedules = await _context.NonTeachingLoadSchedules
                .Where(x => x.NonTeachingLoad.TermId == termId)
                .Select(x => new
                {
                    x.NonTeachingLoad.TeacherId,
                    x.StartTime,
                    x.EndTime
                })
                .ToListAsync();


            var dataDB = await query
                .Select(x => new
                {
                    x.UserId,
                    x.User.FullName,
                    AcademicDepartment = x.AcademicDepartment.Name,
                    teacherDedication = x.TeacherDedication.Name,
                    x.TeacherDedication.MinLessonHours,
                    x.TeacherDedication.MaxLessonHours,
                    x.TeacherDedication.MinNoLessonHours,
                    x.TeacherDedication.MaxNoLessonHours,
                    directedCourseSum = x.TeacherSections.Where(y => y.Section.IsDirectedCourse && y.Section.CourseTerm.TermId == termId).Sum(y => (y.Section.CourseTerm.Course.TheoreticalHours + y.Section.CourseTerm.Course.PracticalHours)) / 2M
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.FullName,
                    x.AcademicDepartment,
                    x.MinNoLessonHours,
                    x.MaxNoLessonHours,
                    x.MinLessonHours,
                    x.MaxLessonHours,
                    x.teacherDedication,
                    hours = (((tecSchedules
                                    .Where(y => y.TeacherId == x.UserId)
                                    .Select(s => new
                                    {
                                        teoricHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                        practicalHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                        seminarHours = s.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR ? DateTime.ParseExact(s.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(s.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalMinutes : 0,
                                    }).Sum(y => y.teoricHours + y.practicalHours + y.seminarHours)) / pedagogical_hour_time) + Convert.ToDouble(x.directedCourseSum)).ToString("0.0"),
                    nonTeaching = nonSchedules.Where(y=>y.TeacherId == x.UserId).Select(s=> s.EndTime.ToLocalTimeSpanUtc().Subtract(s.StartTime.ToLocalTimeSpanUtc()).TotalHours).Sum().ToString("0.0")
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
    }
}