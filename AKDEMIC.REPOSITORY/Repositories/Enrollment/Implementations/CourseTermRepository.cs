using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseTerm;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class CourseTermRepository : Repository<CourseTerm>, ICourseTermRepository
    {
        public CourseTermRepository(AkdemicContext context) : base(context) { }
        #region PRIVATE
        private async Task<Select2Structs.ResponseParameters> GetCourseTermsByTermSelect2(Select2Structs.RequestParameters requestParameters, Guid termId, string userId, Expression<Func<CourseTerm, Select2Structs.Result>> selectPredicate, string searchValue = null)
        {
            var query = _context.CourseTerms
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.Course.Career.AcademicCoordinatorId == userId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Course.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        private Expression<Func<CourseTermDataTemplate, dynamic>> GetCourseTermDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                default:
                    return ((x) => x.Code);
            }
        }

        private async Task<DataTablesStructs.ReturnedData<CourseTermDataTemplate>> GetCourseTermDataTable(
        DataTablesStructs.SentParameters sentParameters, Guid careerId, Guid curriculumId, Guid cid, Guid pid, string search,
        Expression<Func<CourseTermDataTemplate, CourseTermDataTemplate>> selectPredicate = null,
        Expression<Func<CourseTermDataTemplate, dynamic>> orderByPredicate = null)
        {
            var query = _context.CourseTerms
                .Include(x => x.Course.Career.Curriculums)
                .Where(x => x.CourseId == cid && x.TermId == pid && x.Course.CareerId == careerId)
                .AsQueryable();

            var sections = await _context.Sections
                .Include(x => x.CourseTerm.Course.Career)
                .Where(x => x.CourseTerm.CourseId == cid && x.CourseTerm.TermId == pid && x.CourseTerm.Course.CareerId == careerId)
                .ToListAsync();

            if (cid != Guid.Empty)
            {
                query = query.Where(x => x.CourseId == cid);
                sections = sections.Where(x => x.CourseTerm.CourseId == cid).ToList();
            }

            var queryclient = await query
                .Select(s => new
                {
                    Idcourseterm = s.Id,
                    Idterm = s.TermId,
                    Idcourse = s.Course.Id,
                    Code = s.Course.Code,
                    Name = s.Course.Name,
                    Seciton = s.Sections,
                    s.Course.Career.Curriculums
                }).ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Course.Code.ToUpper().Contains(search.ToUpper()) || x.Course.Name.ToUpper().Contains(search.ToUpper()));
            }
            var allitems = queryclient.Where(x => x.Curriculums.Any(y => y.Id == curriculumId));
            int recordsFiltered = allitems.Count();
            var result = allitems
                .Select(s => new CourseTermDataTemplate
                {
                    Idcourseterm = s.Idcourseterm,
                    Idterm = s.Idterm,
                    Idcourse = s.Idcourse,
                    Code = s.Code,
                    Name = s.Name,
                    SectionCode = sections?.FirstOrDefault(y => y.CourseTermId == s.Idcourseterm)?.Code
                })
                //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .ToList();

            int recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<CourseTermDataTemplate>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            //return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC
        public async Task<Select2Structs.ResponseParameters> GetCourseTermsByTermSelect2(Select2Structs.RequestParameters requestParameters, Guid termId, string userId)
        {
            return await GetCourseTermsByTermSelect2(requestParameters, termId, userId, (x) => new Select2Structs.Result
            {
                Id = x.Course.Id,
                Text = x.Course.Name
            });
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetCourseTermByTermSelect2ClientSide(Guid termId, string academicCoordinatorId = null, Guid? careerId = null)
        {
            var query = _context.CourseTerms
             .Where(x => x.TermId == termId)
             .AsQueryable();

            if (!string.IsNullOrEmpty(academicCoordinatorId))
            {
                query = query.Where(x => x.Course.Career.AcademicCoordinatorId == academicCoordinatorId || x.Course.Career.CareerDirectorId == academicCoordinatorId);
            }

            if (careerId.HasValue)
                query = query.Where(x => x.Course.CareerId == careerId || x.Course.AreaId.HasValue);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Course.Id,
                    Text = x.Course.Name
                })
                .Distinct()
                .ToArrayAsync();

            return result;
        }

        Task<CourseTermATemplate> ICourseTermRepository.GetCourseTermATemplateById(Guid id)
        {
            var courseTerm = _context.CourseTerms.Where(x => x.Id == id)
                .Select(
                    x => new CourseTermATemplate
                    {
                        CourseId = x.CourseId,
                        TermId = x.TermId
                    }
                ).FirstOrDefaultAsync();

            return courseTerm;
        }

        public async Task<CourseTerm> GetByFilters(Guid? courseId = null, Guid? termId = null)
        {
            var query = _context.CourseTerms.AsQueryable();

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId.Value);

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<CourseTerm> GetCourseTermWithCourse(Guid id)
        {
            return await _context.CourseTerms
                .Include(x => x.Course)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CourseTerm> GetCourseTermWithTermAndSections(Guid id)
        {
            return await _context.CourseTerms
                 .Include(x => x.Course.Career.Faculty)
                .Include(x => x.Sections)
                .Include(x => x.Term)
                .Include(x => x.Evaluations)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<CourseTermDataTemplate>> GetCourseTermDataTable(DataTablesStructs.SentParameters parameters, Guid careerId, Guid curriculumId, Guid cid, Guid pid, string search)
        {
            return await GetCourseTermDataTable(parameters, careerId, curriculumId, cid, pid, search, null, GetCourseTermDatatableOrderByPredicate(parameters));
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseTermSectionsDataTable(DataTablesStructs.SentParameters parameters, Guid careerId, Guid curriculumId, Guid cid, Guid pid, string search)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == pid && x.CourseTerm.Course.CareerId == careerId).AsNoTracking();

            if (curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (cid != Guid.Empty)
                query = query.Where(x => x.CourseTerm.CourseId == cid);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Trim().Contains(search.ToLower().Trim()) || x.CourseTerm.Course.Code.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var result = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.CourseTermId,
                   x.Id,
                   section = x.Code,
                   courseName = x.CourseTerm.Course.Name,
                   courseCode = x.CourseTerm.Course.Code,
                   teachers = string.Join(", ", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList())
               })
               .ToListAsync();

            int recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<bool> AnyByCourseIdAndTermStatus(Guid courseId, int termStatus)
        {
            var result = await _context.CourseTerms.AnyAsync(x => x.CourseId == courseId && x.Term.Status == termStatus);
            return result;
        }
        public async Task<bool> AnyByIdAndTermStatus(Guid id, int termStatus)
        {
            var result = await _context.CourseTerms.AnyAsync(x => x.Id == id && x.Term.Status == termStatus);
            return result;
        }

        public async Task<object> GetAsModelA(Guid? termId = null, Guid? courseId = null)
        {
            var result = await (from ct in _context.CourseTerms
                                where ct.TermId.Equals(termId) && ct.CourseId.Equals(courseId)
                                select new
                                {
                                    course = ct.Course.Name,
                                    term = ct.Term.Name,
                                    credits = ct.Course.Credits,
                                    weekhours = ct.WeekHours
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<CourseTerm> GetcourseTermEnrollemtn(Guid courseId, Guid? termId = null)
        {
            var courseTerm = await _context.CourseTerms
                .Include(x => x.Course)
                .Include(x => x.Term)
                .Where(ct => ct.CourseId == courseId && ct.TermId == termId && ct.Sections.Any())
                .FirstOrDefaultAsync();

            return courseTerm;
        }

        public async Task<object> GetCourseTerms(Guid cid, string q)
        {
            var result = _context.CourseTerms
            .Where(c => c.CourseId.Equals(cid))
            .Select(c => new
            {
                id = c.TermId,
                text = c.Term.Name
            }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(x => x.text.Contains(q));

            var qry = await result.ToListAsync();

            return qry;
        }

        public async Task<IEnumerable<CourseTerm>> GetAllByTermAndCareer(Guid? careerId = null, Guid? termId = null)
        {
            var query = _context.CourseTerms
                .Include(x => x.Course)
                .Include(x => x.Term)
                .AsQueryable();

            if (careerId != null)
                query = query.Where(x => x.Course.CareerId == careerId || x.Course.AcademicYearCourses.Any(y=>y.Curriculum.CareerId == careerId));

            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            return await query.OrderBy(x => x.Course.Name).ToListAsync();
        }

        public async Task<CourseTerm> GetFirstByCourseAndeTermId(Guid courseId, Guid termId)
        {
            return await _context.CourseTerms.FirstOrDefaultAsync(x => x.CourseId == courseId && x.TermId == termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseGradeStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<CourseTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Course.Career.Name);
                    break;
                default:
                    break;
            }

            var query = _context.CourseTerms
                .Where(x => x.TermId == termId && x.Course.AcademicHistories.Any(y => y.TermId == x.TermId && !y.Withdraw && y.Type != ConstantHelpers.AcademicHistory.Types.CONVALIDATION))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Course.CareerId == careerId);
            }

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Course.Name.ToUpper().Contains(search.ToUpper()) || x.Course.Code.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Career = x.Course.CareerId.HasValue ? x.Course.Career.Name : "-",
                    Grades = x.Course.AcademicHistories.Where(y => y.TermId == x.TermId && !y.Withdraw && y.Type != ConstantHelpers.AcademicHistory.Types.CONVALIDATION).OrderBy(y => y.Grade).Select(y => y.Grade).ToList()
                })
                .ToListAsync();

            var result = new List<CourseStatisticTemplate>();

            foreach (var item in data)
            {
                //Media
                var average = Math.Round(item.Grades.Average(), 2, MidpointRounding.AwayFromZero);

                //Mediana
                var median = item.Grades.Count == 0 ? 0 : item.Grades.Count % 2 != 0 ? item.Grades[item.Grades.Count / 2] : (item.Grades[item.Grades.Count / 2] + item.Grades[(item.Grades.Count / 2) - 1]) / (double)2;

                //desviacion estandar
                var deviation = Math.Round(Math.Sqrt(item.Grades.Average(v => Math.Pow(v - average, 2))), 2, MidpointRounding.AwayFromZero);

                //percentiles
                var p25 = Math.Round(CalculatePercentile(item.Grades, 0.25), 2, MidpointRounding.AwayFromZero);
                var p50 = Math.Round(CalculatePercentile(item.Grades, 0.5), 2, MidpointRounding.AwayFromZero);
                var p75 = Math.Round(CalculatePercentile(item.Grades, 0.75), 2, MidpointRounding.AwayFromZero);

                result.Add(new CourseStatisticTemplate
                {
                    Code = item.Code,
                    Course = item.Course,
                    Career = item.Career,
                    GradeCount = item.Grades.Count,
                    Mean = average,
                    Median = median,
                    StandardDeviation = deviation,
                    Percentile25 = p25,
                    Percentile50 = p50,
                    Percentile75 = p75
                });
            }

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<CourseStatisticTemplate>> GetCourseGradeStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null)
        {
            var query = _context.CourseTerms
                .Where(x => x.TermId == termId && x.Course.AcademicHistories.Any(y => y.TermId == x.TermId))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Course.CareerId == careerId);
            }

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Career = x.Course.CareerId.HasValue ? x.Course.Career.Name : "-",
                    Grades = x.Course.AcademicHistories.Where(y => y.TermId == x.TermId && !y.Withdraw).OrderBy(y => y.Grade).Select(y => y.Grade).ToList()
                })
                .ToListAsync();

            var result = new List<CourseStatisticTemplate>();

            foreach (var item in data)
            {
                //Media
                var average = Math.Round(item.Grades.Average(), 2, MidpointRounding.AwayFromZero);

                //Mediana
                var median = item.Grades.Count == 0 ? 0 : item.Grades.Count % 2 != 0 ? item.Grades[item.Grades.Count / 2] : (item.Grades[item.Grades.Count / 2] + item.Grades[(item.Grades.Count / 2) - 1]) / (double)2;

                //desviacion estandar
                var deviation = Math.Round(Math.Sqrt(item.Grades.Average(v => Math.Pow(v - average, 2))), 2, MidpointRounding.AwayFromZero);

                //percentiles
                var p25 = Math.Round(CalculatePercentile(item.Grades, 0.25), 2, MidpointRounding.AwayFromZero);
                var p50 = Math.Round(CalculatePercentile(item.Grades, 0.5), 2, MidpointRounding.AwayFromZero);
                var p75 = Math.Round(CalculatePercentile(item.Grades, 0.75), 2, MidpointRounding.AwayFromZero);

                result.Add(new CourseStatisticTemplate
                {
                    Code = item.Code,
                    Course = item.Course,
                    Career = item.Career,
                    GradeCount = item.Grades.Count,
                    Mean = average,
                    Median = median,
                    StandardDeviation = deviation,
                    Percentile25 = p25,
                    Percentile50 = p50,
                    Percentile75 = p75
                });
            }

            return result;

        }


        private double CalculatePercentile(List<int> list, double percentile)
        {
            if (list.Count == 0) return 0;

            //position = (n + 1) * P
            var p = (list.Count + 1) * percentile;
            if (p <= 1d) return list[0];
            else if (p >= list.Count) return list[list.Count - 1];
            else
            {
                //Pk = Plow + (Lp - Llow) * (Phigh - Plow)
                var k = (int)p;
                var pLow = list[k - 1];
                var pHigh = list[k];
                var result = pLow + (p - k) * (pHigh - pLow);
                return result;
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseAttendanceStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<CourseTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Course.Career.Name);
                    break;
                default:
                    break;
            }

            var query = _context.CourseTerms
                .Where(x => x.TermId == termId /*&& x.Sections.Any(y => y.Classes.Any(z => z.IsDictated))*/)
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Course.CareerId == careerId);
            }

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Course.Name.ToUpper().Contains(search.ToUpper()) || x.Course.Code.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Career = x.Course.CareerId.HasValue ? x.Course.Career.Name : "-",
                    //Grades = x.Course.AcademicHistories.Where(y => y.TermId == x.TermId && !y.Withdraw).OrderBy(y => y.Grade).Select(y => y.Grade).ToList(),
                    ClassStudents = x.Sections
                        .Select(y => y.Classes.Select(z => z.ClassStudents))
                        .ToList()
                })
                .ToListAsync();

            var result = new List<CourseStatisticTemplate>();

            foreach (var item in data)
            {
                var classStudents = item.ClassStudents.SelectMany(x => x).ToList().SelectMany(x => x).ToList();
                var studentsAttendance = classStudents
                    .Where(x => !x.IsAbsent)
                    .GroupBy(x => x.StudentId)
                    .Select(x => x.Count())
                    .OrderBy(x => x)
                    .ToList();

                if (studentsAttendance.Count == 0)
                {
                    result.Add(new CourseStatisticTemplate
                    {
                        Code = item.Code,
                        Course = item.Course,
                        Career = item.Career,
                        //GradeCount = studentsAttendance.Count,
                        Mean = 0,
                        Median = 0,
                        StandardDeviation = 0,
                        Percentile25 = 0,
                        Percentile50 = 0,
                        Percentile75 = 0
                    });

                    continue;
                }

                //Media
                var average = Math.Round(studentsAttendance.Average(), 2, MidpointRounding.AwayFromZero);

                //Mediana
                var median = studentsAttendance.Count == 0 ? 0 : studentsAttendance.Count % 2 != 0 ? studentsAttendance[studentsAttendance.Count / 2] : (studentsAttendance[studentsAttendance.Count / 2] + studentsAttendance[(studentsAttendance.Count / 2) - 1]) / (double)2;

                //desviacion estandar
                var deviation = Math.Round(Math.Sqrt(studentsAttendance.Average(v => Math.Pow(v - average, 2))), 2, MidpointRounding.AwayFromZero);

                //percentiles
                var p25 = Math.Round(CalculatePercentile(studentsAttendance, 0.25), 2, MidpointRounding.AwayFromZero);
                var p50 = Math.Round(CalculatePercentile(studentsAttendance, 0.5), 2, MidpointRounding.AwayFromZero);
                var p75 = Math.Round(CalculatePercentile(studentsAttendance, 0.75), 2, MidpointRounding.AwayFromZero);

                result.Add(new CourseStatisticTemplate
                {
                    Code = item.Code,
                    Course = item.Course,
                    Career = item.Career,
                    //GradeCount = studentsAttendance.Count,
                    Mean = average,
                    Median = median,
                    StandardDeviation = deviation,
                    Percentile25 = p25,
                    Percentile50 = p50,
                    Percentile75 = p75
                });
            }

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<CourseStatisticTemplate>> GetCourseAttendanceStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null)
        {
            var query = _context.CourseTerms
               .Where(x => x.TermId == termId /*&& x.Sections.Any(y => y.Classes.Any(z => z.IsDictated))*/)
               .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.AcademicSecretaryId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Course.CareerId == careerId);
            }

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Career = x.Course.CareerId.HasValue ? x.Course.Career.Name : "-",
                    //Grades = x.Course.AcademicHistories.Where(y => y.TermId == x.TermId && !y.Withdraw).OrderBy(y => y.Grade).Select(y => y.Grade).ToList(),
                    ClassStudents = x.Sections
                        .Select(y => y.Classes.Select(z => z.ClassStudents))
                        .ToList()
                })
                .ToListAsync();

            var result = new List<CourseStatisticTemplate>();

            foreach (var item in data)
            {
                var classStudents = item.ClassStudents.SelectMany(x => x).ToList().SelectMany(x => x).ToList();
                var studentsAttendance = classStudents
                    .Where(x => !x.IsAbsent)
                    .GroupBy(x => x.StudentId)
                    .Select(x => x.Count())
                    .OrderBy(x => x)
                    .ToList();

                if (studentsAttendance.Count == 0)
                {
                    result.Add(new CourseStatisticTemplate
                    {
                        Code = item.Code,
                        Course = item.Course,
                        Career = item.Career,
                        //GradeCount = studentsAttendance.Count,
                        Mean = 0,
                        Median = 0,
                        StandardDeviation = 0,
                        Percentile25 = 0,
                        Percentile50 = 0,
                        Percentile75 = 0
                    });

                    continue;
                }

                //Media
                var average = Math.Round(studentsAttendance.Average(), 2, MidpointRounding.AwayFromZero);

                //Mediana
                var median = studentsAttendance.Count == 0 ? 0 : studentsAttendance.Count % 2 != 0 ? studentsAttendance[studentsAttendance.Count / 2] : (studentsAttendance[studentsAttendance.Count / 2] + studentsAttendance[(studentsAttendance.Count / 2) - 1]) / (double)2;

                //desviacion estandar
                var deviation = Math.Round(Math.Sqrt(studentsAttendance.Average(v => Math.Pow(v - average, 2))), 2, MidpointRounding.AwayFromZero);

                //percentiles
                var p25 = Math.Round(CalculatePercentile(studentsAttendance, 0.25), 2, MidpointRounding.AwayFromZero);
                var p50 = Math.Round(CalculatePercentile(studentsAttendance, 0.5), 2, MidpointRounding.AwayFromZero);
                var p75 = Math.Round(CalculatePercentile(studentsAttendance, 0.75), 2, MidpointRounding.AwayFromZero);

                result.Add(new CourseStatisticTemplate
                {
                    Code = item.Code,
                    Course = item.Course,
                    Career = item.Career,
                    //GradeCount = studentsAttendance.Count,
                    Mean = average,
                    Median = median,
                    StandardDeviation = deviation,
                    Percentile25 = p25,
                    Percentile50 = p50,
                    Percentile75 = p75
                });
            }

            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid curriculumId, int academicYear)
        {
            Expression<Func<CourseTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Course.Career.Name);
                    break;
                default:
                    break;
            }

            var curriculum = await _context.Curriculums.FindAsync(curriculumId);

            var query = _context.CourseTerms
                .Where(x => x.TermId == termId && x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId && y.AcademicYear == academicYear))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Course.AcademicYearCourses.FirstOrDefault(y => y.CurriculumId == curriculumId).Id,
                    Curriculum = curriculum.Code,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Sections = x.Sections.Count
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetCoursesWithSectionsSelect(Guid termId, Guid? careerId, Guid? curriculumId, int? academicYear)
        {
            var query = _context.Courses.Where(x => x.CourseTerms.Any(y => y.TermId == termId && y.Sections.Any()));

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            var result = await query
                .Select(x => new
                {
                    x.Id,
                    Text = $"{x.Code} - {x.Name}"
                })
                .ToListAsync();

            return result;
        }

        #endregion

    }
}