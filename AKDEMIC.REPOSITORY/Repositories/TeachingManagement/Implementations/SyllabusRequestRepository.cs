using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusRequest;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class SyllabusRequestRepository : Repository<SyllabusRequest>, ISyllabusRequestRepository
    {
        public SyllabusRequestRepository(AkdemicContext context) : base(context) { }

        Task<bool> ISyllabusRequestRepository.AnyByTermId(Guid termId)
            => _context.SyllabusRequests.AnyAsync(x => x.TermId == termId);

        async Task<object> ISyllabusRequestRepository.GetAllAsModelA()
        {
            var query = await _context.SyllabusRequests.Select(x => new
            {

                id = x.Id,
                name = x.Name,
                start = x.Start.ToLocalDateFormat(),
                end = x.End.ToLocalDateFormat(),
                termid = x.Term.Name
            }).ToListAsync();

            return query;
        }

        async Task<object> ISyllabusRequestRepository.GetAllAsModelB(string coordinatorId, string teacherId)
        {
            var coursesQuery = _context.CourseTerms.AsQueryable();

            if (!string.IsNullOrEmpty(coordinatorId))
                coursesQuery = coursesQuery.Where(x => x.CoordinatorId == coordinatorId);

            if (!string.IsNullOrEmpty(teacherId))
                coursesQuery = coursesQuery.Where(x => x.Sections.Any(y => y.TeacherSections.Any(z => z.TeacherId == teacherId)));

            var courses = await coursesQuery
                                .Select(x => new
                                {
                                    coursetermid = x.Id,
                                    coursename = x.Course.Name,
                                    termid = x.TermId
                                }).ToListAsync();

            var query = _context.SyllabusRequests.AsQueryable();

            //if (!string.IsNullOrEmpty(teacherId))
            //{
            //    query = query.Where(x => x.SyllabusTeachers.Any(y => y.TeacherId == teacherId));
            //}

            var syllabusRequests = await query.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                start = x.Start.ToLocalDateFormat(),
                end = x.End.ToLocalDateFormat(),
                term = x.Term.Name,
                termid = x.TermId,
                files = x.SyllabusTeachers.Select(y => y.Url).ToList()
            }).ToListAsync();

            var oquery = _context.SyllabusTeachers.AsQueryable();

            if (!string.IsNullOrEmpty(teacherId))
            {
                oquery = oquery.Where(x => x.TeacherId == teacherId);
            }

            var rrrr = await oquery.Select(x => new { x.SyllabusRequestId, x.CourseTermId }).ToListAsync();
            var result = new List<object>();

            foreach (var syllabusRequest in syllabusRequests)
            {
                foreach (var course in courses)
                {
                    if (!rrrr.Any(x => x.SyllabusRequestId == syllabusRequest.id && x.CourseTermId == course.coursetermid)
                         && syllabusRequest.termid == course.termid)
                    {

                        var syllabusViewModel = new
                        {
                            SyllabusRequestId = syllabusRequest.id,
                            Term = syllabusRequest.term,
                            Name = syllabusRequest.name,
                            CourseTermId = course.coursetermid,
                            CourseName = course.coursename,
                            Start = syllabusRequest.start,
                            End = syllabusRequest.end,
                            Files = syllabusRequest.files
                        };
                        result.Add(syllabusViewModel);
                    }
                }
            }

            return result;
        }

        async Task<object> ISyllabusRequestRepository.GetAsModelA(Guid? id)
        {
            var query = _context.SyllabusRequests.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var result = await query.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                start = x.Start.ToLocalDateFormat(),
                end = x.End.ToLocalDateFormat(),
                termid = x.TermId,
                type = ConstantHelpers.SYLLABUS_REQUEST.TYPE.VALUES[x.Type]
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<SyllabusRequest, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Start); break;
                case "2":
                    orderByPredicate = ((x) => x.End); break;
                case "3":
                    orderByPredicate = ((x) => x.Term.Name); break;
                default:
                    orderByPredicate = ((x) => x.Term.Name); break;
            }

            var query = _context.SyllabusRequests.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      start = x.Start.ToLocalDateFormat(),
                      end = x.End.ToLocalDateFormat(),
                      term = x.Term.Name,
                      termId = x.Term.Id,
                      type = ConstantHelpers.SYLLABUS_REQUEST.TYPE.VALUES[x.Type]
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestToTeachersDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid? termId, string searchValue = null)
        {
            if (!termId.HasValue || termId == Guid.Empty)
            {
                var activeTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
                termId = activeTermId;
            }

            var requestByTerm = await _context.SyllabusRequests.Where(x => x.TermId == termId).FirstOrDefaultAsync();

            //var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && y.Sections.Any(z => z.TeacherSections.Any(h => h.TeacherId == teacherId))));
            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId && y.CoordinatorId == teacherId));

            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            //Si no hay solicitud de silabo no se debe mostrar nada.
            if (requestByTerm == null)
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = new List<object>(),
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.SyllabusRequestId == (requestByTerm == null ? Guid.Empty : requestByTerm.Id));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Course.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    courseName = x.Course.Name,
                    courseCode = x.Course.Code,
                    curriculum = x.Curriculum.Name,
                    curriculumId = x.CurriculumId,
                    courseId = x.CourseId,
                    termId = term.Id,
                    term = term.Name,
                    onlyDigital = requestByTerm.Type == ConstantHelpers.SYLLABUS_REQUEST.TYPE.DIGITAL,
                    requestId = requestByTerm == null ? Guid.Empty : requestByTerm.Id,
                    requestName = requestByTerm == null ? string.Empty : requestByTerm.Name,
                    syllabus = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == term.Id).Select(y => new { y.IsDigital, y.Id }).FirstOrDefault(),
                    syllabusStatus = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.Status).FirstOrDefault(),
                    presentationDate = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id && y.CourseTerm.TermId == termId).Select(y => y.PresentationDate.ToLocalDateTimeFormat()).FirstOrDefault(),
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

        public async Task<SyllabusRequest> GetByTerm(Guid termId)
            => await _context.SyllabusRequests.Where(x => x.TermId == termId).FirstOrDefaultAsync();

        public async Task<ChartJSTemplate> GetChartJsReport(Guid termId, Guid? facultyId, ClaimsPrincipal user)
        {
            //var 
            var query = _context.SyllabusTeachers.Where(x => x.SyllabusRequest.TermId == termId).AsQueryable();
            var allCourses = _context.CourseTerms.Where(x => x.TermId == termId).AsQueryable();

            var careers = _context.Careers.AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                careers = careers.Where(x => x.FacultyId == facultyId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careersFilter = await _context.Careers.Where(x =>
                    x.CareerDirectorId == userId ||
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicSecretaryId == userId)
                        .Select(x => x.Id).ToListAsync();

                    careers = careers.Where(x => careersFilter.Contains(x.Id));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var careersFilter = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();
                    careers = careers.Where(x => careersFilter.Contains(x.Id));
                }
            }

            var careersData = await careers.Select(x => new { x.Id, x.Name }).ToArrayAsync();

            var result = new ChartJSTemplate
            {
                Categories = careersData.Select(x => x.Name).ToArray(),
                CategoriesId = careersData.Select(x => x.Id).ToArray(),
                //Data = new List<DataTemplate>
                //{
                //    new DataTemplate
                //    {
                //        Name = "Pendientes"
                //    },
                //    new DataTemplate
                //    {
                //        Name = "En Proceso"
                //    },
                //    new DataTemplate
                //    {
                //        Name = "Entregados"
                //    }
                //}
            };

            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.ENABLED_SYLLABUS_VALIDATION).FirstOrDefaultAsync();

            if (confi is null)
            {
                confi = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.ENABLED_SYLLABUS_VALIDATION,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.ENABLED_SYLLABUS_VALIDATION]
                };
            }

            var confiValidation = bool.Parse(confi.Value);

            if (confiValidation)
            {
                result.Data = new List<DataTemplate>
                {
                        new DataTemplate
                        {
                            Name = "Pendientes"
                        },
                        new DataTemplate
                        {
                            Name = "En Proceso"
                        },
                        new DataTemplate
                        {
                            Name = "Con Observaciones"
                        },
                        new DataTemplate
                        {
                            Name = "En validación"
                        },
                    new DataTemplate
                        {
                            Name = "Presentados"
                        },
                };

                var pendings = new List<int>();
                var inProcess = new List<int>();
                var observeds = new List<int>();
                var in_validations = new List<int>();
                var confirmed = new List<int>();

                foreach (var item in result.CategoriesId)
                {
                    //pendings
                    var total = await allCourses.Where(x => x.Course.CareerId == item).CountAsync();
                    var inProcessTpm = await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS).CountAsync();
                    var observedsTpm= await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.OBSERVED).CountAsync();
                    var in_vlaidationsTpm = await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_VALIDATION).CountAsync();
                    var confirmedTpm = await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED).CountAsync();
                    var pendingsTpm = total - (inProcessTpm + confirmedTpm);
                    inProcess.Add(inProcessTpm);
                    confirmed.Add(confirmedTpm);
                    pendings.Add(pendingsTpm);

                    observeds.Add(observedsTpm);
                    in_validations.Add(in_vlaidationsTpm);
                }

                result.Data[0].Data = pendings.ToArray();
                result.Data[1].Data = inProcess.ToArray();
                result.Data[2].Data = observeds.ToArray();
                result.Data[3].Data = in_validations.ToArray();
                result.Data[4].Data = confirmed.ToArray();

            }
            else
            {
                result.Data = new List<DataTemplate>
                {
                        new DataTemplate
                        {
                            Name = "Pendientes"
                        },
                        new DataTemplate
                        {
                            Name = "En Proceso"
                        },
                        new DataTemplate
                        {
                            Name = "Presentados"
                        }
                };

                var pendings = new List<int>();
                var inProcess = new List<int>();
                var confirmed = new List<int>();

                foreach (var item in result.CategoriesId)
                {
                    //pendings
                    var total = await allCourses.Where(x => x.Course.CareerId == item).CountAsync();
                    var inProcessTpm = await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS).CountAsync();
                    var confirmedTpm = await query.Where(x => x.CourseTerm.Course.CareerId == item && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED).CountAsync();
                    var pendingsTpm = total - (inProcessTpm + confirmedTpm);
                    inProcess.Add(inProcessTpm);
                    confirmed.Add(confirmedTpm);
                    pendings.Add(pendingsTpm);
                }

                result.Data[0].Data = pendings.ToArray();
                result.Data[1].Data = inProcess.ToArray();
                result.Data[2].Data = confirmed.ToArray();
            }

            return result;

        }

        public async Task<SyllabusRequest> GetLastSyllabusRequestOpened()
        {
            var result = await _context.SyllabusRequests.OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Select2Structs.Result>> GetSyllabusRequestTermSelect2()
        {
            var result = await _context.SyllabusRequests.OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Term.Id,
                    Text = x.Term.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<ChartJSTemplate> GetChartJsReportByAcademicDepartment(Guid termId, Guid? academicDepartmentId, ClaimsPrincipal user)
        {
            var queryAcademicDepartment = _context.AcademicDepartments.AsNoTracking();

            var query = _context.CourseTerms.Where(x => x.TermId == termId).AsNoTracking();

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
            {
                query = query.Where(x => x.Coordinator.Teachers.Any(y => y.AcademicDepartmentId == academicDepartmentId));
                queryAcademicDepartment = queryAcademicDepartment.Where(x => x.Id == academicDepartmentId);
            }

            if (user != null)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    queryAcademicDepartment = queryAcademicDepartment.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId);
                    query = query.Where(x => x.Coordinator.Teachers.Any(y => y.AcademicDepartment.AcademicDepartmentDirectorId == userId || y.AcademicDepartment.AcademicDepartmentSecretaryId == userId));
                }
            }

            var academicDepartment = await queryAcademicDepartment.ToListAsync();

            var result = new ChartJSTemplate
            {
                Categories = academicDepartment.Select(x => x.Name).ToArray(),
                CategoriesId = academicDepartment.Select(x => x.Id).ToArray(),
                Data = new List<DataTemplate>
                {
                    new DataTemplate
                    {
                        Name = "Pendientes"
                    },
                    new DataTemplate
                    {
                        Name = "En Proceso"
                    },
                    new DataTemplate
                    {
                        Name = "Entregados"
                    }
                }
            };

            var pendings = new List<int>();
            var inProcess = new List<int>();
            var confirmed = new List<int>();

            foreach (var item in result.CategoriesId)
            {
                //pendings
                var total = await query.Where(x => x.Coordinator.Teachers.Any(y => y.AcademicDepartmentId == item)).CountAsync();
                var inProcessTpm = await query.Where(x => x.Coordinator.Teachers.Any(y => y.AcademicDepartmentId == item) && x.SyllabusTeachers.Any(y => y.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS)).CountAsync();
                var confirmedTpm = await query.Where(x => x.Coordinator.Teachers.Any(y => y.AcademicDepartmentId == item) && x.SyllabusTeachers.Any(y => y.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED)).CountAsync();
                var pendingsTpm = total - (inProcessTpm + confirmedTpm);
                inProcess.Add(inProcessTpm);
                confirmed.Add(confirmedTpm);
                pendings.Add(pendingsTpm);
            }

            result.Data[0].Data = pendings.ToArray();
            result.Data[1].Data = inProcess.ToArray();
            result.Data[2].Data = confirmed.ToArray();

            return result;

        }


    }
}