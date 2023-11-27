using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class ScoreInputScheduleRepository : Repository<ScoreInputSchedule>, IScoreInputScheduleRepository
    {
        public ScoreInputScheduleRepository(AkdemicContext context) : base(context) { }

        Task<bool> IScoreInputScheduleRepository.AnyByCourseComponentId(Guid courseComponentId)
            => _context.ScoreInputSchedules.AnyAsync(x => x.CourseComponentId == courseComponentId);

        Task<bool> IScoreInputScheduleRepository.AnyByTermIdCourseComponentId(Guid termId, Guid? courseComponentId, Guid? id)
        {
            if (id.HasValue)
            {
                return _context.ScoreInputSchedules.AnyAsync(x => x.TermId == termId && x.CourseComponentId == courseComponentId && x.Id != id);
            }
            else
            {
                return _context.ScoreInputSchedules.AnyAsync(x => x.TermId == termId && x.CourseComponentId == courseComponentId);
            }
        }

        async Task<object> IScoreInputScheduleRepository.GetAllAsModelA()
        {
            var schedules = await _context.ScoreInputSchedules
                .Select(
                    x => new
                    {
                        x.Id,
                        x.TermId,
                        TermName = x.Term.Name,
                        x.CourseComponentId,
                        CourseComponentName = x.CourseComponent.Name
                    }
                )
                .ToListAsync();

            return schedules;
        }

        async Task<object> IScoreInputScheduleRepository.GetAsModelB(Guid? id)
        {
            var query = _context.ScoreInputSchedules.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var schedule = await query
                .Select(
                    x => new
                    {
                        x.Id,
                        x.TermId,
                        x.CourseComponentId,
                        Details = x.Details.Select(y => new { y.Id, y.NumberOfUnit, InputDateTime = y.InputDate.ToLocalDateFormat() }).ToArray()
                    }
                )
                .FirstOrDefaultAsync();

            return schedule;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<ScoreInputSchedule, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.CourseComponent.Name); break;
                default:
                    orderByPredicate = ((x) => x.Term.Name); break;
            }

            var query = _context.ScoreInputSchedules.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseComponent.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()) ||
                x.Term.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT).FirstOrDefaultAsync();

            if (configuration is null)
            {
                configuration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            var evaluationsByUnit = Convert.ToBoolean(configuration.Value);

            if (evaluationsByUnit)
            {
                query = query.Where(x => x.CourseComponentId.HasValue);
            }
            else
            {
                query = query.Where(x => !x.CourseComponentId.HasValue);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.TermId,
                      TermName = x.Term.Name,
                      x.CourseComponentId,
                      CourseComponentName = x.CourseComponent.Name ?? "Sin Componentes"
                  })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid? careerId, Guid? curriculumId, int? unit, byte? status, Guid? academicDepartmentId, ClaimsPrincipal user, string searchValue, int? paramWeek)
        {
            var scoreInputSchedule = await _context.ScoreInputSchedules.Where(x => x.Id == id).FirstOrDefaultAsync();
            var scoreInputDetail = await _context.ScoreInputScheduleDetails.Where(x => x.ScoreInputScheduleId == scoreInputSchedule.Id).ToListAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == scoreInputSchedule.TermId && x.StudentSections.Any()).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
            }

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseTerm.Course.Code.ToLower().Trim().Contains(searchValue.Trim().ToLower()) || x.CourseTerm.Course.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            if (scoreInputSchedule.CourseComponentId.HasValue)
            {
                var subQuery = query
              .Where(x => x.CourseTerm.Course.CourseComponentId == scoreInputSchedule.CourseComponentId)
               .Join(
                  _context.CourseUnits.Where(x => x.CourseSyllabus.TermId == scoreInputSchedule.TermId && x.Evaluations.Any()),
                  sectionCourse => sectionCourse.CourseTerm.CourseId,
                  unitCourse => unitCourse.CourseSyllabus.CourseId,
                  (sectionCourse, unitCourse) => new
                  {
                      teachers = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                      career = sectionCourse.CourseTerm.Course.Career.Name,
                      course = $"{sectionCourse.CourseTerm.Course.Code}-{sectionCourse.CourseTerm.Course.Name}",
                      section = sectionCourse.Code,
                      component = $"UNIDAD {unitCourse.Number}",
                      unitNumber = unitCourse.Number,
                      cycle = string.Join(", ", sectionCourse.CourseTerm.Course.AcademicYearCourses.GroupBy(y=>y.AcademicYear).Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y.Key]).ToList()),
                      allEvaluationWithGrades = unitCourse.Evaluations.Where(y => y.CourseTerm.TermId == scoreInputSchedule.TermId).All(y => y.GradeRegistrations.Any(z => z.SectionId == sectionCourse.Id && z.WasPublished)),
                      totalEvaluations = unitCourse.Evaluations.Where(y=>y.CourseTerm.TermId == scoreInputSchedule.TermId).Count(),
                      evaluationsWithGrades = unitCourse.Evaluations.Where(y=>y.CourseTerm.TermId == scoreInputSchedule.TermId && y.GradeRegistrations.Any(z=>z.SectionId == sectionCourse.Id && z.WasPublished)).Count(),
                      lastGradeRegistration = _context.GradeRegistrations.Where(y => y.Evaluation.CourseTermId == sectionCourse.CourseTermId && y.Evaluation.CourseUnit.Number == unitCourse.Number && y.SectionId == sectionCourse.Id && y.WasPublished).OrderByDescending(y => y.Date).Select(y => y.Date.ToLocalDateTimeFormat()).FirstOrDefault(),
                      wasLate = _context.GradeRegistrations.Where(y => y.Evaluation.CourseTermId == sectionCourse.CourseTermId && y.Evaluation.CourseUnit.Number == unitCourse.Number && y.SectionId == sectionCourse.Id && y.WasPublished).OrderByDescending(y => y.Date).Select(y => y.WasLate).FirstOrDefault()
                  }
              ).AsNoTracking();

                if (unit.HasValue && unit != 0)
                    subQuery = subQuery.Where(x => x.unitNumber == unit);

                if (status.HasValue && status != 0)
                {
                    if (status == 1)
                        subQuery = subQuery.Where(x => x.allEvaluationWithGrades);

                    if (status == 2)
                        subQuery = subQuery.Where(x => x.wasLate);

                    if (status == 3)
                        subQuery = subQuery.Where(x => !x.allEvaluationWithGrades);
                }

                int recordsFiltered = await subQuery.CountAsync();

                var dataDB = await subQuery
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToListAsync();

                var data = dataDB
                      .Select(x => new ScoreInputScheduleDetailViewModel
                      {
                          Teachers = x.teachers,
                          Career = x.career,
                          Course = x.course,
                          Section = x.section,
                          Component = x.component,
                          UnitNumber = x.unitNumber,
                          Cycle = x.cycle,
                          AllEvaluationWithGrades = x.allEvaluationWithGrades,
                          LastGradeRegistration = x.allEvaluationWithGrades ? x.lastGradeRegistration : string.Empty,
                          TotalEvaluations = x.totalEvaluations,
                          EvaluationsWithGrades = x.evaluationsWithGrades,
                          WasLate = x.wasLate
                      })
                      .ToList();

                foreach (var item in data)
                    item.TimeIsUp = scoreInputDetail.Where(y => y.NumberOfUnit == item.UnitNumber).Select(y => y.InputDate.ToDefaultTimeZone().Date < DateTime.UtcNow.ToDefaultTimeZone().Date).FirstOrDefault();

                data = data.OrderBy(x => x.Course).ThenBy(x => x.Section).ThenBy(x => x.UnitNumber).ToList();

                int recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
            else
            {
                var subQuery = query
                 .Join(
                    _context.Evaluations.Where(x => x.CourseTerm.TermId == scoreInputSchedule.TermId),
                    sectionCourse => sectionCourse.CourseTerm.CourseId,
                    evaluation => evaluation.CourseTerm.CourseId,
                    (sectionCourse, evaluation) => new
                    {
                        teachers = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                        career = sectionCourse.CourseTerm.Course.Career.Name,
                        course = $"{sectionCourse.CourseTerm.Course.Code}-{sectionCourse.CourseTerm.Course.Name}",
                        section = sectionCourse.Code,
                        evaluation = evaluation.Name,
                        evaluationWeek = evaluation.Week,
                        cycle = string.Join(", ", sectionCourse.CourseTerm.Course.AcademicYearCourses.GroupBy(y => y.AcademicYear).Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y.Key]).ToList()),
                        lastGradeRegistration = _context.GradeRegistrations.Where(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id && y.WasPublished).OrderByDescending(y => y.Date).Select(y => y.Date).FirstOrDefault(),
                        registerOnClass = _context.Classes.Any(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id),
                        hasGradeRegistration = _context.GradeRegistrations.Any(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id && y.WasPublished)
                    }
                ).AsNoTracking();

                if (status.HasValue && status != 0)
                {
                    if (status == 1)
                        subQuery = subQuery.Where(x => x.hasGradeRegistration);

                    if (status == 2)
                        subQuery = subQuery.Where(x => x.registerOnClass);

                    if (status == 3)
                        subQuery = subQuery.Where(x => !x.hasGradeRegistration);
                }

                if (paramWeek.HasValue && paramWeek != 0)
                    subQuery = subQuery.Where(x => x.evaluationWeek == paramWeek);

                int recordsFiltered = await subQuery.CountAsync();

                var dataDB = await subQuery
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToListAsync();

                var data = dataDB
                      .Select(x => new ScoreInputScheduleDetailViewModel
                      {
                          Teachers = x.teachers,
                          Career = x.career,
                          Course = x.course,
                          Section = x.section,
                          Cycle = x.cycle,
                          Evaluation = x.evaluation,
                          EvaluationWeek = x.evaluationWeek,
                          LastGradeRegistationDateTime = x.lastGradeRegistration.ToDefaultTimeZone(),
                          LastGradeRegistration = x.lastGradeRegistration.ToLocalDateTimeFormat(),
                          RegisterOnClass = x.registerOnClass,
                          HasGradeRegistration = x.hasGradeRegistration
                      })
                      .ToList();

                var term = await _context.Terms.Where(x => x.Id == scoreInputSchedule.TermId).FirstOrDefaultAsync();
                var maxWeeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
                var tempClassDay = term.ClassStartDate.Date;
                var rangeByWeeks = new List<WeekDetailTemplate>();

                var week = 1;
                while (tempClassDay < term.ClassEndDate.Date)
                {
                    var detail = new WeekDetailTemplate();
                    detail.Week = week;
                    detail.StartDate = tempClassDay;
                    tempClassDay = tempClassDay.AddDays(7);

                    if (tempClassDay >= term.ClassEndDate.Date)
                    {
                        detail.EndDate = term.ClassEndDate.Date;
                    }
                    else
                    {
                        detail.EndDate = tempClassDay.AddDays(-1);
                    }

                    rangeByWeeks.Add(detail);

                    week++;
                }

                foreach (var item in data)
                    item.TimeIsUp = rangeByWeeks.Where(x => x.Week == item.EvaluationWeek).Select(x => x.EndDate.Date >= item.LastGradeRegistationDateTime).FirstOrDefault();

                data = data.OrderBy(x => x.Course).ThenBy(x => x.Section).ThenBy(x => x.UnitNumber).ToList();

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

        public async Task<ScoreInputScheduleViewModel> GetReportScoreInputSchedule(Guid id, Guid? careerId, Guid? curriculumId, int? unit, byte? status, Guid? academicDepartmentId, ClaimsPrincipal user, int? paramWeek)
        {
            var scoreInputSchedule = await _context.ScoreInputSchedules.Where(x => x.Id == id).FirstOrDefaultAsync();
            var courseComponent = await _context.CourseComponents.Where(x => x.Id == scoreInputSchedule.CourseComponentId).FirstOrDefaultAsync();
            var scoreInputDetail = await _context.ScoreInputScheduleDetails.Where(x => x.ScoreInputScheduleId == scoreInputSchedule.Id).ToListAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == scoreInputSchedule.TermId && x.StudentSections.Any()).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
            }

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (scoreInputSchedule.CourseComponentId.HasValue)
            {
                var subQuery = query
                    .Where(x => x.CourseTerm.Course.CourseComponentId == scoreInputSchedule.CourseComponentId)
                     .Join(
                        _context.CourseUnits.Where(x => x.CourseSyllabus.TermId == scoreInputSchedule.TermId && x.Evaluations.Any()),
                        sectionCourse => sectionCourse.CourseTerm.CourseId,
                        unitCourse => unitCourse.CourseSyllabus.CourseId,
                        (sectionCourse, unitCourse) => new
                        {
                            teachers = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                            academicDepartments = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.AcademicDepartment.Name).ToList()),
                            teacherConditions = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.WorkerLaborInformation.WorkerLaborCondition.Name).ToList()),
                            teacherDedidactions = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.TeacherDedication.Name).ToList()),
                            career = sectionCourse.CourseTerm.Course.Career.Name,
                            course = $"{sectionCourse.CourseTerm.Course.Code}-{sectionCourse.CourseTerm.Course.Name}",
                            section = sectionCourse.Code,
                            component = $"UNIDAD {unitCourse.Number}",
                            unitNumber = unitCourse.Number,
                            cycle = string.Join(", ", sectionCourse.CourseTerm.Course.AcademicYearCourses.GroupBy(y => y.AcademicYear).Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y.Key]).ToList()),
                            allEvaluationWithGrades = unitCourse.Evaluations.Where(y => y.CourseTerm.TermId == scoreInputSchedule.TermId).All(y => y.GradeRegistrations.Any(z => z.SectionId == sectionCourse.Id && z.WasPublished)),
                            totalEvaluations = unitCourse.Evaluations.Where(y => y.CourseTerm.TermId == scoreInputSchedule.TermId).Count(),
                            evaluationsWithGrades = unitCourse.Evaluations.Where(y => y.CourseTerm.TermId == scoreInputSchedule.TermId && y.GradeRegistrations.Any(z => z.SectionId == sectionCourse.Id && z.WasPublished)).Count(),
                            lastGradeRegistration = _context.GradeRegistrations.Where(y => y.Evaluation.CourseTermId == sectionCourse.CourseTermId && y.Evaluation.CourseUnit.Number == unitCourse.Number && y.SectionId == sectionCourse.Id && y.WasPublished).OrderByDescending(y => y.Date).Select(y => y.Date.ToLocalDateTimeFormat()).FirstOrDefault(),
                            wasLate = _context.GradeRegistrations.Where(y => y.Evaluation.CourseTermId == sectionCourse.CourseTermId && y.Evaluation.CourseUnit.Number == unitCourse.Number && y.SectionId == sectionCourse.Id && y.WasPublished).OrderByDescending(y => y.Date).Select(y => y.WasLate).FirstOrDefault()
                        }
                    ).AsNoTracking();

                if (unit.HasValue && unit != 0)
                    subQuery = subQuery.Where(x => x.unitNumber == unit);

                if (status.HasValue && status != 0)
                {
                    if (status == 1)
                        subQuery = subQuery.Where(x => x.allEvaluationWithGrades);

                    if (status == 2)
                        subQuery = subQuery.Where(x => x.wasLate);

                    if (status == 3)
                        subQuery = subQuery.Where(x => !x.allEvaluationWithGrades);
                }

                var data = await subQuery
            .Select(x => new ScoreInputScheduleDetailViewModel
            {
                Teachers = x.teachers,
                Career = x.career,
                Course = x.course,
                Section = x.section,
                Component = x.component,
                UnitNumber = x.unitNumber,
                AllEvaluationWithGrades = x.allEvaluationWithGrades,
                LastGradeRegistration = x.allEvaluationWithGrades ? x.lastGradeRegistration : string.Empty,
                WasLate = x.wasLate,
                Cycle = x.cycle,
                TotalEvaluations = x.totalEvaluations,
                EvaluationsWithGrades = x.evaluationsWithGrades,
                AcademicDepartments = x.academicDepartments,
                TeacherConditions = x.teacherConditions,
                TeacherDedications = x.teacherDedidactions
            })
            .ToListAsync();

                foreach (var item in data)
                    item.TimeIsUp = scoreInputDetail.Where(y => y.NumberOfUnit == item.UnitNumber).Select(y => y.InputDate.ToDefaultTimeZone().Date < DateTime.UtcNow.ToDefaultTimeZone().Date).FirstOrDefault();

                data = data.OrderBy(x => x.Course).ThenBy(x => x.Section).ThenBy(x => x.UnitNumber).ToList();

                var model = new ScoreInputScheduleViewModel
                {
                    UnitComponents = courseComponent?.Name ?? "Sin Componentes",
                    Details = data
                };


                if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                    model.AcademicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).Select(x => x.Name).FirstOrDefaultAsync();

                if (careerId.HasValue && careerId != Guid.Empty)
                    model.Career = await _context.Careers.Where(x => x.Id == careerId).Select(x => x.Name).FirstOrDefaultAsync();

                return model;
            }
            else
            {
                var subQuery = query
                .Join(
                   _context.Evaluations.Where(x => x.CourseTerm.TermId == scoreInputSchedule.TermId),
                   sectionCourse => sectionCourse.CourseTerm.CourseId,
                   evaluation => evaluation.CourseTerm.CourseId,
                   (sectionCourse, evaluation) => new
                   {
                       teachers = string.Join("; ", sectionCourse.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                       career = sectionCourse.CourseTerm.Course.Career.Name,
                       course = $"{sectionCourse.CourseTerm.Course.Code}-{sectionCourse.CourseTerm.Course.Name}",
                       section = sectionCourse.Code,
                       evaluation = evaluation.Name,
                       evaluationWeek = evaluation.Week,
                       cycle = string.Join(", ", sectionCourse.CourseTerm.Course.AcademicYearCourses.GroupBy(y => y.AcademicYear).Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y.Key]).ToList()),
                       lastGradeRegistration = _context.GradeRegistrations.Where(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id).OrderByDescending(y => y.Date).Select(y => y.Date).FirstOrDefault(),
                       registerOnClass = _context.Classes.Any(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id),
                       hasGradeRegistration = _context.GradeRegistrations.Any(y => y.EvaluationId == evaluation.Id && y.SectionId == sectionCourse.Id)
                   }
               ).AsNoTracking();

                if (status.HasValue && status != 0)
                {
                    if (status == 1)
                        subQuery = subQuery.Where(x => x.hasGradeRegistration);

                    if (status == 2)
                        subQuery = subQuery.Where(x => x.registerOnClass);

                    if (status == 3)
                        subQuery = subQuery.Where(x => !x.hasGradeRegistration);
                }

                if (paramWeek.HasValue && paramWeek != 0)
                    subQuery = subQuery.Where(x => x.evaluationWeek == paramWeek);

                var dataDB = await subQuery.ToListAsync();

                var data = dataDB
                     .Select(x => new ScoreInputScheduleDetailViewModel
                     {
                         Teachers = x.teachers,
                         Career = x.career,
                         Course = x.course,
                         Section = x.section,
                         Cycle = x.cycle,
                         Evaluation = x.evaluation,
                         EvaluationWeek = x.evaluationWeek,
                         LastGradeRegistationDateTime = x.lastGradeRegistration.ToDefaultTimeZone(),
                         LastGradeRegistration = x.lastGradeRegistration.ToLocalDateTimeFormat(),
                         RegisterOnClass = x.registerOnClass,
                         HasGradeRegistration = x.hasGradeRegistration
                     })
                     .ToList();

                var term = await _context.Terms.Where(x => x.Id == scoreInputSchedule.TermId).FirstOrDefaultAsync();
                var maxWeeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
                var tempClassDay = term.ClassStartDate.Date;
                var rangeByWeeks = new List<WeekDetailTemplate>();

                var week = 1;
                while (tempClassDay < term.ClassEndDate.Date)
                {
                    var detail = new WeekDetailTemplate();
                    detail.Week = week;
                    detail.StartDate = tempClassDay;
                    tempClassDay = tempClassDay.AddDays(7);

                    if (tempClassDay >= term.ClassEndDate.Date)
                    {
                        detail.EndDate = term.ClassEndDate.Date;
                    }
                    else
                    {
                        detail.EndDate = tempClassDay.AddDays(-1);
                    }

                    rangeByWeeks.Add(detail);

                    week++;
                }

                foreach (var item in data)
                    item.TimeIsUp = rangeByWeeks.Where(x => x.Week == item.EvaluationWeek).Select(x => x.EndDate.Date >= item.LastGradeRegistationDateTime).FirstOrDefault();

                data = data.OrderBy(x => x.Course).ThenBy(x => x.Section).ThenBy(x => x.UnitNumber).ToList();


                var model = new ScoreInputScheduleViewModel
                {
                    UnitComponents = courseComponent?.Name ?? "Sin Componentes",
                    Details = data
                };

                return model;
            }
        }

        public async Task<ScoreInputSchedule> GetByTermAndCourseComponent(Guid termId, Guid courseComponentId)
        {
            var result = await _context.ScoreInputSchedules.Where(x => x.TermId == termId && x.CourseComponentId == courseComponentId).Include(x => x.Details).FirstOrDefaultAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportConsolidatedScoreInputSchedule(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId, string searchValue, byte status)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Any()).AsNoTracking();
            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(c => c.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseTerm.Course.Code.ToLower().Contains(searchValue.Trim().ToLower()) || x.CourseTerm.Course.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            if (status == 1)
            {
                query = query.Where(x => x.CourseTerm.Evaluations.Any() && x.GradeRegistrations.Count() == x.CourseTerm.Evaluations.Count());
            }
            else if (status == 2)
            {
                query = query.Where(x => x.CourseTerm.Evaluations.Any() && x.GradeRegistrations.Count() != x.CourseTerm.Evaluations.Count());
            }
            else if (status == 3)
            {
                query = query.Where(x => !x.CourseTerm.Evaluations.Any());

            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    section = x.Code,
                    courseCode = x.CourseTerm.Course.Code,
                    courseName = x.CourseTerm.Course.Name,
                    hasEvaluations = x.CourseTerm.Evaluations.Any(),
                    evaluations = x.CourseTerm.Evaluations.Count(),
                    courseUnitsQuantity = _context.CourseUnits.Where(y => y.CourseSyllabus.CourseId == x.CourseTerm.CourseId && y.CourseSyllabus.TermId == x.CourseTerm.TermId).Count(),
                    complete = x.GradeRegistrations.Count() == x.CourseTerm.Evaluations.Count()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<List<ReportScoreInputScheduleConsolidated>> GetReportConsolidatedScoreInputSchedule(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId, byte status)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId).AsNoTracking();
            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(c => c.CurriculumId == curriculumId));

            if (status == 1)
            {
                query = query.Where(x => x.CourseTerm.Evaluations.Any() && x.GradeRegistrations.Count() == x.CourseTerm.Evaluations.Count());
            }
            else if (status == 2)
            {
                query = query.Where(x => x.CourseTerm.Evaluations.Any() && x.GradeRegistrations.Count() != x.CourseTerm.Evaluations.Count());
            }
            else if (status == 3)
            {
                query = query.Where(x => !x.CourseTerm.Evaluations.Any());

            }

            var data = await query
                .Select(x => new ReportScoreInputScheduleConsolidated
                {
                    Teacher = string.Join(", ", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()),
                    Section = x.Code,
                    CourseCode = x.CourseTerm.Course.Code,
                    CourseName = x.CourseTerm.Course.Name,
                    HasEvaluations = x.CourseTerm.Evaluations.Any(),
                    Evaluations = x.CourseTerm.Evaluations.Count(),
                    CourseUnitsQuantity = _context.CourseUnits.Where(y => y.CourseSyllabus.CourseId == x.CourseTerm.CourseId && y.CourseSyllabus.TermId == x.CourseTerm.TermId).Count(),
                    Complete = x.GradeRegistrations.Count() == x.CourseTerm.Evaluations.Count()
                })
                .ToListAsync();

            return data;
        }
    }
}