using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicSummary;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AcademicSummariesRepository : Repository<AcademicSummary>, IAcademicSummariesRepository
    {
        public AcademicSummariesRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private Expression<Func<AcademicSummaryTemplate, dynamic>> GetAcademicSummaryDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.Code);
                case "2":
                    return ((x) => x.Career);
                case "3":
                    return ((x) => x.AcademicProgram);
                case "4":
                    return ((x) => x.Finalgrade);
                case "5":
                    return ((x) => x.Approbed);
                case "6":
                    return ((x) => x.Finalgrade);
                case "7":
                    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<AcademicSummaryTemplate, string[]> GetAcademicSummaryDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name+"",
                x.Code+"",
                x.Career+"",
                x.AcademicProgram+"",
                x.Observations+"",
                x.Order+"",
                x.Finalgrade+"",
                x.Approbed+"",
        };
        }
        private async Task<DataTablesStructs.ReturnedData<AcademicSummaryTemplate>> GetAcademicSummaryDatatable(
          DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid termId, string name,
          Expression<Func<AcademicSummaryTemplate, AcademicSummaryTemplate>> selectPredicate = null,
          Expression<Func<AcademicSummaryTemplate, dynamic>> orderByPredicate = null,
          Func<AcademicSummaryTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.AcademicSummaries
               .Include(x => x.Student.Career)
               .Include(x => x.Student.AcademicProgram)
              .Include(x => x.Term)
              .Where(x => x.TermId == termId)
              .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(name.Trim().ToLower()));

            var result = query
                .Select(x => new AcademicSummaryTemplate
                {
                    Name = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    AcademicProgram = x.Student.AcademicProgram.Name,
                    Observations = x.MeritType,
                    Finalgrade = x.WeightedAverageGrade < 0 ? 0 : x.WeightedAverageGrade,
                    Approbed = x.WeightedAverageGrade <= x.Term.MinGrade ? false : true,
                    Order = x.MeritOrder
                })
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        #endregion
        #region PUBLIC

        public async Task<List<AcademicSummaryTemplate>> GetAcademicSummaryTemplate(Guid? careerId, Guid? termId)
        {
            var query = _context.AcademicSummaries
              .Include(x => x.Student.Career)
              .Include(x => x.Student.AcademicProgram)
             .Include(x => x.Term)
             .Where(x => x.TermId == termId)
             .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            var result = await query
                .Select(x => new AcademicSummaryTemplate
                {
                    Name = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    AcademicProgram = x.Student.AcademicProgram.Name,
                    Observations = x.MeritType,
                    Finalgrade = x.WeightedAverageGrade < 0 ? 0 : x.WeightedAverageGrade,
                    Approbed = x.WeightedAverageGrade <= x.Term.MinGrade ? false : true,
                    Order = x.MeritOrder
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryTemplate>> GetAcademicSummaryDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid termId, string name)
        {
            return await GetAcademicSummaryDatatable(parameters, careerId, termId, name, null, GetAcademicSummaryDatatableOrderByPredicate(parameters), GetAcademicSummaryDatatableSearchValuePredicate());
        }

        public async Task<IEnumerable<AcademicSummaryReportTemplate>> GetAcademicSummariesReportAsDataAsync(Guid? careerId, Guid termId)
        {
            var query = _context.AcademicSummaries
                .Include(x => x.Student)
                .Include(x => x.Term)
                .Where(x => x.TermId == termId)
                .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            var result = new List<AcademicSummaryReportTemplate>
            {
                new AcademicSummaryReportTemplate
                {
                    approbedname = "Desaprobados", // solo para el color del chart :u
                    approbeds = query.Where(x=>x.WeightedAverageGrade <= x.Term.MinGrade).Count(),
                    disapprobedname = "Aprobados",
                    disapprobeds = query.Where(x=>x.WeightedAverageGrade > x.Term.MinGrade).Count()
                }
            };

            //var result = query
            //    .AsEnumerable()
            //    .GroupBy(x => x.CareerId)
            //    .Select(x => new AcademicSummaryReportTemplate
            //    {
            //        approbedname = "Desaprobados",
            //        approbeds = x.Count(c => c.WeightedAverageGrade <= c.Term.MinGrade),
            //        disapprobedname = "Aprobados",
            //        disapprobeds = x.Count(c => c.WeightedAverageGrade > c.Term.MinGrade)
            //    }).ToList();

            return result;
        }

        public async Task<AcademicSummary> GetByStudentAndTerm(Guid studentId, Guid termId)
        {
            return _context.AcademicSummaries.Include(acs => acs.Term).Include(acs => acs.Career)
                           .FirstOrDefault(acs => acs.TermId == termId && acs.StudentId == studentId);
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllByStudent(Guid studentId)
        {
            return await _context.AcademicSummaries.Include(x => x.Term).Where(x => x.StudentId == studentId).ToListAsync();
        }

        public async Task<List<MeritChartDetailTemplate>> GetDetailMeritChart(Guid studentId)
        {
            var details = await _context.AcademicSummaries.Where(x => x.StudentId == studentId &&
                (x.Term.Status == 1 || x.Term.Status == 2))
                    .Select(x => new MeritChartDetailTemplate
                    {
                        Term = x.Term.Name,
                        MeritOrder = x.MeritOrder,
                        Average = x.WeightedAverageGrade,
                        ApprovedCredits = x.ApprovedCredits,
                        TotalStudents = _context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault(),
                        Observations = "-"

                    })
                    .OrderBy(x => x.Term)
                    .ToListAsync();

            return details;
        }

        public async Task<List<AcademicPerformanceSummaryTemplate>> GetAcademicPerformanceSummary(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var details = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId && x.CurriculumId == student.CurriculumId && !x.WasWithdrawn)
                .OrderBy(x => x.Term.Year).ThenBy(x => x.Term.Number)
                .Select(x => new AcademicPerformanceSummaryTemplate
                {
                    Term = x.Term.Name,
                    Average = x.WeightedAverageGrade,
                    ApprovedCredits = x.ApprovedCredits,
                    TotalCredits = x.TotalCredits,
                    Year = x.Term.Year,
                    Number = x.Term.Number,
                    TermScore = x.TermScore,
                    CumulativeWeightedAverage = x.WeightedAverageCumulative,
                    CumulativeScore = x.CumulativeScore,
                    WeightedAverageGrade = x.WeightedAverageGrade,
                    TermStartDate = x.Term.StartDate,
                    ExtraCredits = x.ExtraCredits,
                    ExtraScore = x.ExtraScore
                }).ToListAsync();


            var curriculumCourses = await _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .Select(x => x.CourseId)
                    .ToListAsync();

            var qryAcademicHistories = _context.AcademicHistories
                .Where(x => x.StudentId == studentId && x.Approved
                && !((x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated)
                || x.Type == ConstantHelpers.AcademicHistory.Types.DIRECTED
                || x.Type == ConstantHelpers.AcademicHistory.Types.LEVELING
                || x.Type == ConstantHelpers.AcademicHistory.Types.HOLIDAY
                || x.Type == ConstantHelpers.AcademicHistory.Types.SUMMER)
                && curriculumCourses.Contains(x.CourseId)
                && !x.Student.AcademicSummaries.Any(y => y.TermId == x.TermId))
                .AsNoTracking();

            if (details.Count > 0)
            {
                var lastTermDate = details.OrderByDescending(x => x.TermStartDate).FirstOrDefault().TermStartDate;
                qryAcademicHistories = qryAcademicHistories.Where(x => x.Term.StartDate > lastTermDate);
            }

            var academicHistories = await qryAcademicHistories
                .Select(x => new
                {
                    x.TermId,
                    x.Term.StartDate,
                    x.Course.Credits,
                    x.Grade,
                    x.Course.AcademicProgramId,
                    AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : ""
                }).ToListAsync();

            if (academicHistories.Count != 0)
            {
                var extraScore = Math.Round(academicHistories.Where(y => y.Grade > 0).Sum(y => y.Grade * y.Credits) * 2, MidpointRounding.AwayFromZero) / 2;
                var extraCredits = academicHistories.Where(y => y.Grade > 0).Sum(y => y.Credits);

                var cumulativeScore = details.Sum(x => x.TermScore + x.ExtraScore) + extraScore;
                var cumulative = 0.00M;
                var totalCredits = details.Sum(x => x.TotalCredits + x.ExtraCredits) + extraCredits;
                if (totalCredits > 0) cumulative = Math.Round(cumulativeScore / totalCredits, 2, MidpointRounding.AwayFromZero);

                var summary = new AcademicPerformanceSummaryTemplate
                {
                    Term = null,
                    ApprovedCredits = 0,
                    TotalCredits = 0,
                    Average = 0.0M,
                    WeightedAverageGrade = 0.0M,
                    Year = 0,
                    Number = null,
                    TermScore = 0,
                    TermStartDate = DateTime.UtcNow,

                    CumulativeWeightedAverage = cumulative,
                    CumulativeScore = cumulativeScore,

                    ExtraCredits = academicHistories.Sum(x => x.Credits)
                };

                details.Add(summary);
            }

            //if (details.Count == 0)
            //{
            //    var curriculumCourses = await _context.AcademicYearCourses
            //        .Where(x => x.CurriculumId == student.CurriculumId)
            //        .Select(x => x.CourseId)
            //        .ToListAsync();

            //    var qryAcademicHistories = _context.AcademicHistories
            //        .Where(x => x.StudentId == studentId && x.Approved
            //        && !((x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated)
            //        || x.Type == ConstantHelpers.AcademicHistory.Types.DIRECTED
            //        || x.Type == ConstantHelpers.AcademicHistory.Types.LEVELING
            //        || x.Type == ConstantHelpers.AcademicHistory.Types.HOLIDAY
            //        || x.Type == ConstantHelpers.AcademicHistory.Types.SUMMER)
            //        && curriculumCourses.Contains(x.CourseId))
            //        .AsNoTracking();

            //    var academicHistories = await qryAcademicHistories
            //        .Select(x => new
            //        {
            //            x.TermId,
            //            x.Term.StartDate,
            //            x.Course.Credits,
            //            x.Grade
            //        }).ToListAsync();

            //    if (academicHistories.Count != 0)
            //    {
            //        var summary = new AcademicPerformanceSummaryTemplate
            //        {
            //            Term = null,
            //            ApprovedCredits = 0,
            //            TotalCredits = 0,
            //            Average = 0.0M,
            //            WeightedAverageGrade = 0.0M,
            //            Year = 0,
            //            Number = null,
            //            TermScore = 0,
            //            TermStartDate = DateTime.UtcNow,

            //            CumulativeWeightedAverage = academicHistories.Where(x => x.Grade > 0).Sum(y => y.Credits) > 0 ? Math.Round(academicHistories.Where(x => x.Grade > 0).Sum(y => y.Grade * y.Credits) / academicHistories.Where(x => x.Grade > 0).Sum(y => y.Credits), 2, MidpointRounding.AwayFromZero) : 0.00M,
            //            CumulativeScore = Math.Round(academicHistories.Where(x => x.Grade > 0).Sum(y => y.Grade * y.Credits) * 2, MidpointRounding.AwayFromZero) / 2,
            //            ExtraCredits = academicHistories.Sum(x => x.Credits)
            //        };

            //        details.Add(summary);
            //    }
            //}

            return details;
        }

        public async Task<decimal> GetTotalCreditsApproved(Guid studentId)
        {
            var query = _context.AcademicYearCourses.AsNoTracking();
            var student = await _context.Students.Include(x => x.Career).Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var result = await query.Where(x => x.CurriculumId == student.CurriculumId && x.Course.AcademicHistories.Any(y => y.StudentId == studentId && y.Approved)).SumAsync(y => y.Course.Credits);
            return result;
        }

        public async Task<decimal> GetAverage(Guid? graduationTermid = null)
        {
            var average = await _context.AcademicSummaries.Where(x => x.TermId == graduationTermid).Select(x => x.WeightedAverageCumulative).FirstOrDefaultAsync();

            return average;
        }

        public async Task<decimal> GetCurrent(Guid studentId, Guid? graduationTermid = null)
        {
            var current = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.TermId == graduationTermid).Select(x => x.WeightedAverageCumulative).FirstOrDefaultAsync();
            return current;
        }

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailUpperThird(Guid studentId)
        {
            var details = await _context.AcademicSummaries.Where(x => x.StudentId == studentId &&
            (x.Term.Status == 1 || x.Term.Status == 2) && x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD)
                .Select(x => new UpperFifthDetailsTemplate
                {
                    Term = x.Term.Name,
                    MeritOrder = x.MeritOrder,
                    Average = x.WeightedAverageGrade,
                    ApprovedCredits = x.ApprovedCredits,
                    TotalStudents = _context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault(),
                    TotalStudentsInUpperFifth = (_context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault()) / 5,
                    Observations = "-"
                }).ToListAsync();

            return details;
        }

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailUpperFith(Guid studentId)
        {
            var details = await _context.AcademicSummaries.Where(x => x.StudentId == studentId &&
            (x.Term.Status == 1 || x.Term.Status == 2) && x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH)
                .Select(x => new UpperFifthDetailsTemplate
                {
                    Term = x.Term.Name,
                    MeritOrder = x.MeritOrder,
                    Average = x.WeightedAverageGrade,
                    ApprovedCredits = x.ApprovedCredits,
                    TotalStudents = _context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault(),
                    TotalStudentsInUpperFifth = (_context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault()) / 5,
                    Observations = "-"
                }).ToListAsync();

            return details;
        }

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailTenthFith(Guid studentId)
        {
            var details = await _context.AcademicSummaries.Where(x => x.StudentId == studentId &&
            (x.Term.Status == 1 || x.Term.Status == 2) && x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH)
                .Select(x => new UpperFifthDetailsTemplate
                {
                    Term = x.Term.Name,
                    MeritOrder = x.MeritOrder,
                    Average = x.WeightedAverageGrade,
                    ApprovedCredits = x.ApprovedCredits,
                    TotalStudents = _context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault(),
                    TotalStudentsInUpperFifth = (_context.AcademicSummaries.Where(y => y.Term.Name == x.Term.Name).GroupBy(y => y.TermId).Select(y => y.Count()).FirstOrDefault()) / 5,
                    Observations = "-"
                }).ToListAsync();

            return details;
        }

        public async Task<decimal> GetAverageBachelorsDegree(Guid studentId, Guid? graduationTermid = null)
        {
            var average = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.TermId == graduationTermid)
                .Select(x => x.WeightedAverageCumulative).FirstOrDefaultAsync();

            return average;
        }

        public async Task<int> GetacademicSemestersCount(Guid studentId)
        {
            var academicSemesters = await _context.AcademicSummaries.Where(x => x.StudentId == studentId).CountAsync();

            return academicSemesters;
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllByStudentOrderedByTermDesc(Guid studentId)
        {
            var query = _context.AcademicSummaries
           .Include(x => x.Term)
           .Include(x => x.Career)
           .Include(x => x.Student)
           .Where(x => x.StudentId == studentId)
           .OrderByDescending(x => x.Term.EndDate);
            return await query.ToListAsync();
        }

        public async Task<decimal> GetCurrentWeightedGrade(Guid studentId, Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);
            var current = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && term != null && x.TermId != termId)
                        .OrderByDescending(x => x.Term.StartDate).Select(x => x.WeightedAverageGrade).FirstOrDefaultAsync();

            return current;
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllWithIncludesByStudent(Guid studentId)
        {
            var query = _context.AcademicSummaries
                    .Include(x => x.Term)
                    .Include(x => x.Student)
                        .ThenInclude(x => x.User)
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<AcademicSummary> GetLastTermSanctionedByStudentId(Guid studentId)
        {
            var lastTerm = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Term.Year)
                .ThenByDescending(x => x.Term.Number)
                .Include(x => x.Term)
                .FirstOrDefaultAsync();

            return lastTerm;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            Expression<Func<Career, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Career = x.Name,
                    TotalGraduated = x.Students.Count(y => y.Status == ConstantHelpers.Student.States.GRADUATED),
                    GraduatedInTime = x.Students.Count(y => y.Status == ConstantHelpers.Student.States.GRADUATED
                            && y.AcademicSummaries.Count() == _context.AcademicYearCourses.Where(z => z.CurriculumId == y.CurriculumId).DefaultIfEmpty().Max(z => (int)z.AcademicYear))
                })
                .OrderBy(x => x.TotalGraduated)
                .ThenBy(x => x.Career)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<object> GetGraduatedInTimeChart(ClaimsPrincipal user = null)
        {
            var query = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    Career = x.Name,
                    TotalGraduated = x.Students.Count(y => y.Status == ConstantHelpers.Student.States.GRADUATED),
                    GraduatedInTime = x.Students.Count(y => y.Status == ConstantHelpers.Student.States.GRADUATED
                            && y.AcademicSummaries.Count() == _context.AcademicYearCourses.Where(z => z.CurriculumId == y.CurriculumId).DefaultIfEmpty().Max(z => (int)z.AcademicYear))
                })
                .OrderBy(x => x.TotalGraduated)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.TotalGraduated == 0 ? 0.0 :
                        Math.Round((x.GraduatedInTime * 100.0) / x.TotalGraduated * 1.0, 2, MidpointRounding.AwayFromZero)).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null)
        {
            var currenTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var academicYears = 15;
            int recordsFiltered = 0;
            int recordsTotal = 0;

            var queryCareers = _context.Careers.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                queryCareers = queryCareers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId);
            }

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                queryCareers = queryCareers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
            }

            var model = await queryCareers
                .Select(x => new AcademicSummaryByCareerTemplate
                {
                    CareerId = x.Id,
                    Career = x.Name,
                    //AcademicYears = new List<AcademicSummaryByCareerDetailTemplate>(),
                    Total = 0
                })
                .ToListAsync();

            var careerHashSet = model.Select(x => x.CareerId).ToHashSet();

            if (currenTermId == termId)
            {
                var students = await _context.Students
                    .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                    .Where(x => careerHashSet.Contains(x.CareerId))
                    .Select(x => new
                    {
                        x.CurrentAcademicYear,
                        x.CareerId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    if (item.Total > 0)
                    {

                    }

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.CurrentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }
            else
            {
                var students = await _context.AcademicSummaries
                    .Where(x => x.TermId == termId)
                    .Where(x => careerHashSet.Contains(x.CareerId))
                    .Select(x => new
                    {
                        x.StudentAcademicYear,
                        x.CareerId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.StudentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }

            recordsFiltered = model.Count();

            model = model
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            recordsTotal = model.Count;

            return new DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>
            {
                Data = model,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null, Guid? facultyId = null)
        {
            var currenTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var recordsFiltered = 0;
            var recordsTotal = 0;

            var model = await GetAcademicSummariesByCycleData(termId, user, facultyId);

            recordsFiltered = model.Count();

            model = model
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            recordsTotal = model.Count;

            return new DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>
            {
                Data = model,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleData(Guid termId, ClaimsPrincipal user, Guid? facultyId = null)
        {
            var academicYears = 15;
            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                careersQuery = careersQuery
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId);
            }

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                careersQuery = careersQuery.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                careersQuery = careersQuery.Where(x => x.FacultyId == facultyId);

            var model = await careersQuery
                .Select(x => new AcademicSummaryByCareerTemplate
                {
                    //Faculty = "&emsp;<strong>Facultad:</strong>&emsp;" + x.Career.Faculty.Name,
                    //CareerId = x.Id,
                    //Career = "&emsp;&emsp;&emsp;<strong>Escuela:</strong>&emsp;" + x.Career.Name,
                    //AcademicProgram = "&emsp;&emsp;&emsp;&emsp;&emsp;Programa:&emsp;" + x.Name,
                    Faculty = x.Faculty.Name,
                    CareerId = x.Id,
                    Career = x.Name,
                    //AcademicProgram = x.Name,
                    //AcademicYears = new List<AcademicSummaryByCareerDetailTemplate>(),
                    Total = 0
                })
                .OrderBy(x => x.Faculty).ThenBy(x => x.Career)/*.ThenBy(x => x.AcademicProgram)*/
                .ToListAsync();

            var careersHashSet = model.Select(x => x.CareerId).ToHashSet();

            var currentTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            if (currentTermId == termId)
            {
                var query = _context.Students.AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(x => x.Career.FacultyId == facultyId);

                var students = await query
                    .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                    .Where(x => /*x.AcademicProgramId.HasValue && */careersHashSet.Contains(x.CareerId))
                    .Select(x => new
                    {
                        x.CurrentAcademicYear,
                        x.CareerId,
                        //x.AcademicProgramId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.CurrentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }
            else
            {
                var summariesQry = _context.AcademicSummaries.AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    summariesQry = summariesQry.Where(x => x.Career.FacultyId == facultyId);

                var students = await summariesQry
                    .Where(x => x.TermId == termId)
                    .Where(x => /*x.Student.AcademicProgramId.HasValue &&*/ careersHashSet.Contains(x.Student.CareerId))
                    .Select(x => new
                    {
                        x.StudentAcademicYear,
                        x.CareerId,
                        //x.Student.AcademicProgramId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.StudentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }

            return model;
        }

        public async Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdData(Guid termId, ClaimsPrincipal user = null)
        {
            var currenTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var academicYears = 15;

            var queryCareers = _context.Careers.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                queryCareers = queryCareers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId);
            }

            var model = await queryCareers
                .Select(x => new AcademicSummaryByCareerTemplate
                {
                    CareerId = x.Id,
                    Career = x.Name,
                    //AcademicYears = new List<AcademicSummaryByCareerDetailTemplate>(),
                    Total = 0
                })
                .ToListAsync();

            var careerHashSet = model.Select(x => x.CareerId).ToHashSet();

            if (currenTermId == termId)
            {
                var students = await _context.Students
                    .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                    .Where(x => careerHashSet.Contains(x.CareerId))
                    .Select(x => new
                    {
                        x.CurrentAcademicYear,
                        x.CareerId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    if (item.Total > 0)
                    {

                    }

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.CurrentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }
            else
            {
                var students = await _context.AcademicSummaries
                    .Where(x => x.TermId == termId)
                    .Where(x => careerHashSet.Contains(x.CareerId))
                    .Select(x => new
                    {
                        x.StudentAcademicYear,
                        x.CareerId
                    })
                    .ToListAsync();

                foreach (var item in model)
                {
                    var careerStudents = students.Where(x => x.CareerId == item.CareerId).ToList();

                    item.Total = careerStudents.Count;

                    for (int i = 1; i <= academicYears; i++)
                    {
                        var detail = new AcademicSummaryByCareerDetailTemplate
                        {
                            AcademicYear = i,
                            Quantity = careerStudents.Count(x => x.StudentAcademicYear == i)
                        };

                        item.AcademicYears.Add(detail);
                    }
                }
            }

            return model = model
                 .ToList();
        }

        public async Task<AcademicSummary> GetAcademicSumariesByStudentTermAndCareer(Guid studentId, Guid careerId, Guid termId)
        {
            return await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.CareerId == careerId && x.TermId == termId).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountByCareerIdAndTermId(Guid careerId, Guid termId)
        {
            return await _context.AcademicSummaries.Where(x => x.CareerId == careerId && x.TermId == termId).CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAverageGradesByConditionsDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.AcademicSummaries.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            if (sex != 0)
                query = query.Where(x => x.Student.User.Sex == sex);

            var recordsFiltered = await query
                .Select(x => new { x.Student.User.Sex, x.TermId, x.Student.CareerId })
                .Distinct()
                .CountAsync();

            var dbData = await query
                .Select(x => new
                {
                    x.CareerId,
                    Career = x.Career.Name,
                    x.Student.User.Sex,
                    x.TermId,
                    Term = x.Term.Name,
                    x.WeightedAverageGrade
                })
                .ToListAsync();

            var preData = dbData
                .Select(x => new
                {
                    x.CareerId,
                    x.Career,
                    x.Sex,
                    x.TermId,
                    x.Term
                })
                .Distinct()
                .ToList();


            var data = preData
                .OrderBy(x => x.Career)
                .ThenByDescending(x => x.Term)
                .Select(x => new
                {
                    x.Career,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Sex) ? ConstantHelpers.SEX.VALUES[x.Sex] : "",
                    x.Term,
                    Low = dbData.Count(y => y.WeightedAverageGrade <= 11.0m && y.CareerId == x.CareerId && y.Sex == x.Sex && y.TermId == x.TermId),
                    Medium = dbData.Count(y => y.WeightedAverageGrade > 11.0m && y.WeightedAverageGrade <= 15.0m && y.CareerId == x.CareerId && y.Sex == x.Sex && y.TermId == x.TermId),
                    High = dbData.Count(y => y.WeightedAverageGrade > 15.0m && y.CareerId == x.CareerId && y.Sex == x.Sex && y.TermId == x.TermId),
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetAverageGradesByConditionsChart(int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.AcademicSummaries
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }


            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            if (sex != 0)
                query = query.Where(x => x.Student.User.Sex == sex);

            var categories = new List<string>();
            var data = new List<int>();

            categories.Add("[0 - 11]");
            data.Add(await query.CountAsync(y => y.WeightedAverageGrade <= 11));

            categories.Add("[12 - 15]");
            data.Add(await query.CountAsync(y => y.WeightedAverageGrade > 11 && y.WeightedAverageGrade <= 15));

            categories.Add("[16 - 20]");
            data.Add(await query.CountAsync(y => y.WeightedAverageGrade > 15));

            var result = new
            {
                categories,
                data
            };

            return result;
        }

        public async Task<decimal> GetStudentAverageAcumulative(Guid studentId, Guid? termId = null)
        {
            var query = _context.AcademicSummaries
                .Where(x => x.StudentId == studentId)
                .AsQueryable();

            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            var academicSummary = await query.OrderByDescending(x => x.Term.ClassStartDate).FirstOrDefaultAsync();

            decimal result = 0;

            if (academicSummary != null)
            {
                result = academicSummary.WeightedAverageGrade;
            }

            return result;
        }

        public async Task UpdateAcademicSummariesJob()
        {
            var academicSummaries = new List<AcademicSummary>();
            var academicHistories = await _context.AcademicHistories
                .Where(x => !x.Validated)
                .OrderBy(x => x.CourseId)
                .OrderBy(x => x.StudentId)
                .OrderBy(x => x.TermId)
                .ToListAsync();
            var courses = await _context.Courses
                .OrderBy(x => x.Id)
                .ToListAsync();
            var students = await _context.Students
                .OrderBy(x => x.Id)
                .OrderBy(x => x.CareerId)
                .ToListAsync();
            var terms = await _context.Terms
                .OrderBy(x => x.Id)
                .ToListAsync();

            foreach (var student in students)
            {
                foreach (var term in terms)
                {
                    var academicSummaryAny = false;

                    academicSummaries.OrderBy(x => x.CareerId)
                        .OrderBy(x => x.StudentId)
                        .OrderBy(x => x.TermId);

                    foreach (var academicSummary in academicSummaries)
                    {
                        if (academicSummary.CareerId == student.CareerId && academicSummary.StudentId == student.Id && academicSummary.TermId == term.Id)
                        {
                            academicSummaryAny = true;

                            break;
                        }
                    }

                    if (academicSummaryAny)
                    {
                        continue;
                    }

                    var academicHistoryAny = false;
                    var approvedCredits = 0.0M;
                    var grade = 0;
                    var gradeIndex = 0;
                    var totalCredits = 0.0M;

                    foreach (var academicHistory in academicHistories)
                    {
                        if (academicHistory.StudentId == student.Id && academicHistory.TermId == term.Id)
                        {
                            foreach (var course in courses)
                            {
                                if (course.Id == academicHistory.CourseId)
                                {
                                    var courseCredits = course.Credits;

                                    if (academicHistory.Grade >= term.MinGrade)
                                    {
                                        approvedCredits += courseCredits;
                                    }

                                    academicHistoryAny = true;
                                    grade += academicHistory.Grade;
                                    totalCredits += courseCredits;
                                    gradeIndex++;

                                    break;
                                }
                            }
                        }
                    }

                    if (!academicHistoryAny)
                    {
                        continue;
                    }

                    academicSummaries.Add(new AcademicSummary
                    {
                        CareerId = student.CareerId,
                        StudentId = student.Id,
                        TermId = term.Id,
                        WeightedAverageGrade = grade / gradeIndex,
                        Observations = "",
                        MeritOrder = 1,
                        MeritType = CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE,
                        TotalCredits = totalCredits,
                        ApprovedCredits = approvedCredits,
                        StudentAcademicYear = 1,
                        TermHasFinished = term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED ? true : false
                    });
                }
            }

            await _context.AcademicSummaries.AddRangeAsync(academicSummaries);
            await _context.SaveChangesAsync();

            var academicSummaries2 = await _context.AcademicSummaries.OrderByDescending(x => x.WeightedAverageGrade).ToListAsync();
            var careers = await _context.Careers.ToListAsync();

            foreach (var term in terms)
            {
                foreach (var career in careers)
                {
                    var academicSummary2Index = 0;

                    foreach (var academicSummary2 in academicSummaries2)
                    {
                        if (academicSummary2.CareerId == career.Id && academicSummary2.TermId == term.Id)
                        {
                            academicSummary2.MeritOrder = ++academicSummary2Index;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateAcademicSummariesJob(string connectionString, string careerCode)
        {
            Guid? careerId = null;

            if (careerCode != null)
            {
                var career = await _context.Careers.FirstOrDefaultAsync(x => x.Code.ToUpper() == careerCode.ToUpper());
                careerId = career.Id;
            }

            var academicSummaries = _context.AcademicSummaries.AsQueryable();

            if (careerId != null)
            {
                academicSummaries = academicSummaries.Where(x => x.Student.CareerId == careerId);
            }

            var academicSummaries2 = await academicSummaries.ToListAsync();

            _context.AcademicSummaries.RemoveRange(academicSummaries2);
            await _context.SaveChangesAsync();

            var academicSummaryDataTable = new DataTable();

            academicSummaryDataTable.Columns.Add("Id", typeof(Guid));
            academicSummaryDataTable.Columns.Add("CareerId", typeof(Guid));
            academicSummaryDataTable.Columns.Add("StudentId", typeof(Guid));
            academicSummaryDataTable.Columns.Add("TermId", typeof(Guid));
            academicSummaryDataTable.Columns.Add("ApprovedCredits", typeof(int));
            academicSummaryDataTable.Columns.Add("MeritOrder", typeof(int));
            academicSummaryDataTable.Columns.Add("MeritType", typeof(int));
            academicSummaryDataTable.Columns.Add("Observations", typeof(string));
            academicSummaryDataTable.Columns.Add("StudentAcademicYear", typeof(int));
            academicSummaryDataTable.Columns.Add("StudentStatus", typeof(int));
            academicSummaryDataTable.Columns.Add("TermHasFinished", typeof(bool));
            academicSummaryDataTable.Columns.Add("TotalCredits", typeof(int));
            academicSummaryDataTable.Columns.Add("TotalOrder", typeof(int));
            academicSummaryDataTable.Columns.Add("WasWithdrawn", typeof(bool));
            academicSummaryDataTable.Columns.Add("WeightedAverageCumulative", typeof(decimal));
            academicSummaryDataTable.Columns.Add("WeightedAverageGrade", typeof(decimal));

            var students = await _context.Students
                .Where(x => careerId != null ? x.CareerId == careerId : x.CareerId != null)
                .Select(x => new Student
                {
                    Id = x.Id,
                    CareerId = x.CareerId
                })
                .ToListAsync();

            for (var i = 0; i < students.Count; i++)
            {
                academicSummaries2 = new List<AcademicSummary>();
                var student = students[i];
                var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == student.Id)
                    .Select(x => new AcademicHistory
                    {
                        CourseId = x.CourseId,
                        TermId = x.TermId,
                        Approved = x.Approved,
                        Grade = x.Grade,
                        Course = new Course
                        {
                            Credits = x.Course.Credits
                        },
                        Term = new Term
                        {
                            Status = x.Term.Status
                        }
                    })
                    .ToListAsync();

                var terms = academicHistories
                    .GroupBy(x => new { x.TermId, x.Term.Status })
                    .Select(x => new Term
                    {
                        Id = x.Key.TermId,
                        Status = x.Key.Status
                    })
                    .ToList();

                for (var j = 0; j < terms.Count; j++)
                {
                    var term = terms[j];
                    var academicHistories2 = academicHistories
                        .Where(x => x.TermId == term.Id)
                        .GroupBy(x => new { x.CourseId, x.Approved, x.Grade, x.Course.Credits })
                        .Select(x => new AcademicHistory
                        {
                            Approved = x.Key.Approved,
                            Grade = x.Key.Grade,
                            Course = new Course
                            {
                                Credits = x.Key.Credits
                            }
                        })
                        .ToList();

                    if (academicHistories2.Count == 0)
                    {
                        continue;
                    }

                    var academicSummariesCount = 0;
                    var approvedCredits = 0.0M;
                    var totalCredits = 0.0M;
                    var weightedAverageGrade = 0.0M;
                    var weightedAverageCumulative = 0.0M;

                    for (var k = 0; k < academicHistories2.Count; k++)
                    {
                        var academicHistory2 = academicHistories2[k];
                        totalCredits += academicHistory2.Course.Credits;
                        weightedAverageGrade += academicHistory2.Grade * academicHistory2.Course.Credits;

                        if (academicHistory2.Approved)
                        {
                            approvedCredits += academicHistory2.Course.Credits;
                        }

                        for (var l = 0; l < academicSummaries2.Count; l++)
                        {
                            var academicSummary = academicSummaries2[l];

                            if (academicSummary.StudentId == student.Id)
                            {
                                academicSummariesCount++;
                                weightedAverageCumulative += academicSummary.WeightedAverageGrade;
                            }
                        }
                    }

                    if (totalCredits > 0)
                    {
                        weightedAverageGrade /= (1.0M * totalCredits);
                    }
                    else
                    {
                        weightedAverageGrade = 0;
                    }

                    weightedAverageCumulative += weightedAverageGrade / (academicSummariesCount + 1);

                    academicSummaries2.Add(new AcademicSummary
                    {
                        Id = Guid.NewGuid(),
                        CareerId = student.CareerId,
                        StudentId = student.Id,
                        TermId = term.Id,
                        ApprovedCredits = approvedCredits,
                        MeritOrder = 1,
                        MeritType = CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE,
                        Observations = null,
                        StudentAcademicYear = 1,
                        StudentStatus = CORE.Helpers.ConstantHelpers.Student.States.REGULAR,
                        TermHasFinished = term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED,
                        TotalCredits = totalCredits,
                        TotalOrder = 1,
                        WasWithdrawn = false,
                        WeightedAverageCumulative = weightedAverageCumulative,
                        WeightedAverageGrade = weightedAverageGrade
                    });
                }

                for (var j = 0; j < academicSummaries2.Count; j++)
                {
                    var academicSummary = academicSummaries2[j];
                    var academicSummaryRow = academicSummaryDataTable.NewRow();
                    academicSummaryRow[0] = academicSummary.Id;
                    academicSummaryRow[1] = academicSummary.CareerId;
                    academicSummaryRow[2] = academicSummary.StudentId;
                    academicSummaryRow[3] = academicSummary.TermId;
                    academicSummaryRow[4] = academicSummary.ApprovedCredits;
                    academicSummaryRow[5] = academicSummary.MeritOrder;
                    academicSummaryRow[6] = academicSummary.MeritType;
                    academicSummaryRow[7] = academicSummary.Observations;
                    academicSummaryRow[8] = academicSummary.StudentAcademicYear;
                    academicSummaryRow[9] = academicSummary.StudentStatus;
                    academicSummaryRow[10] = academicSummary.TermHasFinished;
                    academicSummaryRow[11] = academicSummary.TotalCredits;
                    academicSummaryRow[12] = academicSummary.TotalOrder;
                    academicSummaryRow[13] = academicSummary.WasWithdrawn;
                    academicSummaryRow[14] = academicSummary.WeightedAverageCumulative;
                    academicSummaryRow[15] = academicSummary.WeightedAverageGrade;

                    academicSummaryDataTable.Rows.Add(academicSummaryRow);
                }
            }


            using (var sqlBulkCopy = new SqlBulkCopy(connectionString))
            {
                sqlBulkCopy.DestinationTableName = ConstantHelpers.ENTITY_MODELS.INTRANET.ACADEMIC_SUMMARY;
                await sqlBulkCopy.WriteToServerAsync(academicSummaryDataTable);
            }
        }

        public async Task ReCreateStudentAcademicSummaries(Guid studentId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Include(x => x.Career)
                .FirstOrDefaultAsync();

            var qryAcademicHistories = _context.AcademicHistories
                .Where(x => x.StudentId == studentId /*&& !x.Withdraw*/
                && ((x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated)
                || x.Type == ConstantHelpers.AcademicHistory.Types.DIRECTED
                || x.Type == ConstantHelpers.AcademicHistory.Types.LEVELING
                || x.Type == ConstantHelpers.AcademicHistory.Types.HOLIDAY
                || x.Type == ConstantHelpers.AcademicHistory.Types.SUMMER)
                )
                .AsNoTracking();

            var academicHistories = await qryAcademicHistories
                .Select(x => new
                {
                    x.TermId,
                    TermYear = x.Term.Year,
                    TermNumber = x.Term.Number,
                    x.CourseId,
                    x.Grade,
                    x.Course.Credits,
                    x.Approved,
                    AcademicYear = x.CurriculumId.HasValue ? x.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault() : x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    CurriculumId = x.CurriculumId.HasValue ? x.CurriculumId.Value : x.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),
                    x.Course.AcademicProgramId,
                    x.SectionId,
                    x.Term.StartDate,
                    x.Withdraw
                })
                .ToListAsync();

            var additionalHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && (x.Type != ConstantHelpers.AcademicHistory.Types.DIRECTED
                && x.Type != ConstantHelpers.AcademicHistory.Types.LEVELING
                && x.Type != ConstantHelpers.AcademicHistory.Types.HOLIDAY
                && x.Type != ConstantHelpers.AcademicHistory.Types.SUMMER
                && !(x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated)))
                .Select(x => new
                {
                    x.Approved,
                    x.TermId,
                    x.Term.StartDate,
                    x.CourseId,
                    x.Course.Credits,
                    x.Grade,
                    Curriculums = x.Course.AcademicYearCourses.Select(y => y.CurriculumId).ToList(),
                    x.Course.AcademicProgramId,
                    AcademicProgram = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : null,
                    x.Type,
                    x.Validated
                })
                .ToListAsync();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
            {
                var deferredHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == studentId && (x.Type == ConstantHelpers.AcademicHistory.Types.DEFERRED || x.Validated))
                    .Select(x => new
                    {
                        x.TermId,
                        TermYear = x.Term.Year,
                        TermNumber = x.Term.Number,
                        x.CourseId,
                        x.Grade,
                        x.Course.Credits,
                        x.Approved,
                        AcademicYear = x.CurriculumId.HasValue ? x.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault() : x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        CurriculumId = x.CurriculumId.HasValue ? x.CurriculumId.Value : x.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),
                        x.Course.AcademicProgramId,
                        x.SectionId,
                        x.Term.StartDate,
                        x.Withdraw
                    })
                    .ToListAsync();

                academicHistories = academicHistories.Where(x => !deferredHistories.Any(y => y.CourseId == x.CourseId && x.TermId == y.TermId)).ToList();
                academicHistories.AddRange(deferredHistories);

                additionalHistories = additionalHistories.Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED || x.Validated).ToList();
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
            {
                academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == studentId)
                    .Select(x => new
                    {
                        x.TermId,
                        TermYear = x.Term.Year,
                        TermNumber = x.Term.Number,
                        x.CourseId,
                        x.Grade,
                        x.Course.Credits,
                        x.Approved,
                        AcademicYear = x.CurriculumId.HasValue ? x.Course.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).Select(y => y.AcademicYear).FirstOrDefault() : x.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                        CurriculumId = x.CurriculumId.HasValue ? x.CurriculumId.Value : x.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),
                        x.Course.AcademicProgramId,
                        x.SectionId,
                        x.Term.StartDate,
                        x.Withdraw
                    })
                    .ToListAsync();

                additionalHistories.Clear();
            }

            var curriculums = await _context.Curriculums
                .Select(x => new
                {
                    x.Id,
                    x.CareerId
                })
                .ToListAsync();

            var enrolledTerms = academicHistories
                .GroupBy(x => new { x.TermId, x.TermYear, x.TermNumber, x.CurriculumId, x.StartDate })
                .Select(x => new
                {
                    x.Key.TermId,
                    x.Key.TermYear,
                    x.Key.TermNumber,
                    x.Key.CurriculumId,
                    x.Key.StartDate,
                    CareerId = curriculums.Any(y => y.Id == x.Key.CurriculumId) ? curriculums.FirstOrDefault(y => y.Id == x.Key.CurriculumId).CareerId : student.CareerId,
                    ApprovedCredits = x.Where(y => y.Approved).Sum(y => y.Credits),
                    TotalCredits = x.Where(y => !y.Withdraw).Sum(y => y.Credits),
                    WeightedAverageGrade = x.Where(y => !y.Withdraw).Sum(y => y.Credits) > 0 ? Math.Round(x.Where(y => !y.Withdraw).Sum(y => y.Grade * y.Credits) / x.Where(y => !y.Withdraw).Sum(y => y.Credits), 2, MidpointRounding.AwayFromZero) : 0.00M,
                    TermScore = Math.Round(x.Where(y => !y.Withdraw).Sum(y => y.Grade * y.Credits) * 2, MidpointRounding.AwayFromZero) / 2,
                    StudentAcademicYear = x.GroupBy(y => y.AcademicYear).OrderByDescending(y => y.Count()).Select(y => y.Key).FirstOrDefault(),
                    ApprovedCourses = x.Where(y => !y.Withdraw && y.Approved).Select(x => x.CourseId).ToList(),
                    Withdraw = x.All(y => y.Withdraw)
                })
                .OrderBy(x => x.TermYear)
                .ThenBy(x => x.TermNumber)
                .ToList();

            var academicYearCredits = await _context.AcademicYearCredits
                .Select(x => new
                {
                    x.CurriculumId,
                    x.AcademicYear,
                    x.StartCredits,
                    x.EndCredits
                }).ToListAsync();

            var usedCourses = new List<Tuple<Guid, Guid>>();

            var studentSections = await _context.StudentSections
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    x.Section.CourseTerm.TermId
                }).ToListAsync();
            //var term2019 = await _context.Terms.FirstOrDefaultAsync(x => x.RelationId == "201902");

            var processedSummaries = new List<AcademicSummary>();

            foreach (var item in enrolledTerms)
            {
                //if (activeTerm != null && activeTerm.Id == item.TermId) continue;
                var hasPendingGrades = studentSections.Any(x => x.TermId == item.TermId && x.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS);

                usedCourses.AddRange(item.ApprovedCourses.Select(x => new Tuple<Guid, Guid>(item.CurriculumId, x)).ToList());
                var extraPrevCourses = additionalHistories
                    .Where(x => x.Curriculums.Contains(item.CurriculumId)
                    && x.Approved && x.StartDate <= item.StartDate && x.Grade > 0
                    && !usedCourses.Any(y => y.Item1 == item.CurriculumId && y.Item2 == x.CourseId))
                    .ToList();

                var extraScore = Math.Round(extraPrevCourses.Sum(y => y.Grade * y.Credits) * 2, MidpointRounding.AwayFromZero) / 2;
                var extraCredits = extraPrevCourses.Sum(y => y.Credits);
                usedCourses.AddRange(extraPrevCourses.Select(x => new Tuple<Guid, Guid>(item.CurriculumId, x.CourseId)).ToList());

                var cumulativeScore = processedSummaries.Where(x => x.CurriculumId == item.CurriculumId).Sum(x => x.TermScore + x.ExtraScore) + item.TermScore + extraScore;
                var cumulative = 0.00M;
                var totalCredits = processedSummaries.Where(x => x.CurriculumId == item.CurriculumId).Sum(x => x.TotalCredits + x.ExtraCredits) + item.TotalCredits + extraCredits;
                if (totalCredits > 0) cumulative = Math.Round(cumulativeScore / totalCredits, 2, MidpointRounding.AwayFromZero);

                var summary = new AcademicSummary
                {
                    Id = Guid.NewGuid(),
                    CareerId = item.CareerId,
                    StudentId = student.Id,
                    TermId = item.TermId,
                    ApprovedCredits = item.ApprovedCredits,
                    Observations = item.Withdraw ? "Retiro de ciclo" : "",
                    StudentAcademicYear = item.StudentAcademicYear,
                    StudentStatus = ConstantHelpers.Student.States.REGULAR,
                    StudentCondition = ConstantHelpers.Student.Condition.REGULAR,
                    TermHasFinished = !hasPendingGrades,
                    TotalCredits = item.TotalCredits,
                    WasWithdrawn = item.Withdraw,
                    WeightedAverageCumulative = cumulative,
                    WeightedAverageGrade = item.WeightedAverageGrade,
                    TermScore = item.TermScore,
                    CumulativeScore = cumulativeScore,
                    ExtraScore = extraScore,
                    ExtraCredits = extraCredits,

                    MeritOrder = 0,
                    MeritType = ConstantHelpers.ACADEMIC_ORDER.NONE,
                    TotalOrder = 0,
                    TotalMeritType = ConstantHelpers.ACADEMIC_ORDER.NONE,
                };

                if (item.CurriculumId != Guid.Empty)
                    summary.CurriculumId = item.CurriculumId;

                //creditos aprobados de periodos previoc + creditos totales matriculados del ciclo
                var approvedCredits = processedSummaries.Where(x => x.CurriculumId == item.CurriculumId).Sum(x => x.ApprovedCredits + x.ExtraCredits)
                    + item.TotalCredits + extraPrevCourses.Where(x => x.StartDate < item.StartDate).Sum(x => x.Credits);

                if (academicYearCredits.Any(x => x.CurriculumId == item.CurriculumId))
                {
                    var currentYear = academicYearCredits
                        .Where(x => x.CurriculumId == item.CurriculumId && x.StartCredits <= approvedCredits && approvedCredits <= x.EndCredits)
                        .FirstOrDefault();

                    if (currentYear == null)
                    {
                        var lastYearCredits = academicYearCredits
                        .Where(x => x.CurriculumId == item.CurriculumId)
                        .OrderByDescending(x => x.AcademicYear)
                        .FirstOrDefault();

                        if (lastYearCredits != null && approvedCredits > lastYearCredits.EndCredits) summary.StudentAcademicYear = lastYearCredits.AcademicYear;
                    }
                    else summary.StudentAcademicYear = currentYear.AcademicYear;
                }

                processedSummaries.Add(summary);
            }

            var currentSummaries = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId)
                .ToListAsync();

            var summariesToRemove = currentSummaries.Where(x => !processedSummaries.Any(y => y.TermId == x.TermId && y.CurriculumId == x.CurriculumId)).ToList();
            _context.AcademicSummaries.RemoveRange(summariesToRemove);

            var summariesToUpdate = currentSummaries.Where(x => processedSummaries.Any(y => y.TermId == x.TermId && y.CurriculumId == x.CurriculumId)).ToList();
            foreach (var item in summariesToUpdate)
            {
                var data = processedSummaries.FirstOrDefault(x => x.TermId == item.TermId && x.CurriculumId == item.CurriculumId);
                //if (data.TermId == term2019.Id)
                //{
                //    var entro = true;
                //}
                item.CareerId = data.CareerId;
                item.CurriculumId = data.CurriculumId;

                if (!item.TermHasFinished || item.MeritOrder <= 0)
                {
                    item.StudentAcademicYear = data.StudentAcademicYear;
                    item.TermHasFinished = data.TermHasFinished;
                    item.WeightedAverageGrade = data.WeightedAverageGrade;
                    item.TermScore = data.TermScore;
                }

                item.ApprovedCredits = data.ApprovedCredits;
                item.TotalCredits = data.TotalCredits;
                item.WasWithdrawn = data.WasWithdrawn;

                item.WeightedAverageCumulative = data.WeightedAverageCumulative;

                item.CumulativeScore = data.CumulativeScore;
                item.ExtraScore = data.ExtraScore;
                item.ExtraCredits = data.ExtraCredits;
            }

            var newSummaries = processedSummaries
                .Where(x => !currentSummaries.Any(y => y.TermId == x.TermId && y.CurriculumId == x.CurriculumId))
                .ToList();

            await _context.AcademicSummaries.AddRangeAsync(newSummaries);

            if (student.CurrentMeritOrder == 0 && processedSummaries.All(x => x.TermHasFinished))
                student.CurrentMeritOrder = -1;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAcademicSummariesMeritOrderJob(string connectionString)
        {
            var careerId = new Guid();
            var termId = new Guid();
            var meritOrder = 0;
            var academicSummaries = await _context.AcademicSummaries
                .Select(x => new AcademicSummary
                {
                    Id = x.Id,
                    CareerId = x.CareerId,
                    TermId = x.TermId
                })
                .OrderByDescending(x => x.WeightedAverageGrade)
                .OrderByDescending(x => x.TermId)
                .OrderByDescending(x => x.CareerId)
                .AsNoTracking()
                .ToListAsync();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"UPDATE {ConstantHelpers.ENTITY_MODELS.INTRANET.ACADEMIC_SUMMARY} SET MeritOrder = @MeritOrder WHERE Id = @Id";
                        sqlCommand.Transaction = sqlTransaction;

                        sqlCommand.Parameters.Add("@MeritOrder", SqlDbType.Int);
                        sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
                        sqlCommand.Prepare();

                        for (var i = 0; i < academicSummaries.Count; i++)
                        {
                            var academicSummary = academicSummaries[i];

                            if (academicSummary.CareerId != careerId || academicSummary.TermId != termId)
                            {
                                if (academicSummary.CareerId != careerId)
                                {
                                    careerId = academicSummary.CareerId;
                                }

                                if (academicSummary.TermId != termId)
                                {
                                    termId = academicSummary.TermId;
                                }

                                meritOrder = 0;
                            }

                            sqlCommand.Parameters["@MeritOrder"].Value = ++meritOrder;
                            sqlCommand.Parameters["@Id"].Value = academicSummary.Id;

                            await sqlCommand.ExecuteNonQueryAsync();
                        }

                        sqlTransaction.Commit();
                    }
                }
            }
        }

        public async Task CreateAcademicSummariesOldJob()
        {
            var careers = await _context.Careers
                .ToListAsync();
            var terms = await _context.Terms.OrderBy(x => x.Name).ToListAsync();

            var careerCount = 0;

            var countUpdate = 0;

            foreach (var career in careers)
            {
                var students = await _context.Students.Where(x => x.CareerId == career.Id).ToListAsync();

                var academicHistories = await _context.AcademicHistories
                         .Where(x => x.Student.CareerId == career.Id)
                         .Include(x => x.Course)
                         .ToListAsync();

                var academicSummaries = new List<AcademicSummary>();

                var termCount = 0;

                foreach (var term in terms)
                {
                    var studentCount = 0;

                    foreach (var student in students)
                    {
                        var studentHistories = academicHistories
                            .Where(x => x.TermId == term.Id && x.StudentId == student.Id)
                            .ToList();

                        if (studentHistories.Count == 0) continue;

                        var averageGrade = studentHistories.Sum(x => x.Grade) / (1.0M * studentHistories.Count);
                        var cumulativeGrade = (academicSummaries.Where(x => x.StudentId == student.Id).Sum(x => x.WeightedAverageGrade) + averageGrade)
                            / (academicSummaries.Where(x => x.StudentId == student.Id).Count() + 1);

                        academicSummaries.Add(new AcademicSummary
                        {
                            CareerId = student.CareerId,
                            StudentId = student.Id,
                            TermId = term.Id,
                            WeightedAverageGrade = averageGrade,
                            WeightedAverageCumulative = cumulativeGrade,
                            Observations = "",
                            MeritOrder = 1,
                            TotalCredits = studentHistories.Sum(x => x.Course.Credits),
                            ApprovedCredits = studentHistories.Where(x => x.Approved).Sum(x => x.Course.Credits),
                            StudentAcademicYear = 1,
                            TermHasFinished = term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED ? true : false
                        });

                        studentCount++;
                    }

                    var summaries = academicSummaries.Where(x => x.CareerId == career.Id && x.TermId == term.Id).OrderByDescending(x => x.WeightedAverageGrade);
                    var order = 1;
                    var total = summaries.Count();

                    foreach (var summary in summaries)
                    {
                        summary.MeritOrder = order;
                        summary.MeritType = order <= (total * 0.1) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH
                        : order <= (total * 0.20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                        : order <= (total * 0.33) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                        : order <= (total * 0.5) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF
                        : CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE;

                        order++;
                    }

                    termCount++;
                    countUpdate++;

                    await _context.AcademicSummaries.AddRangeAsync(academicSummaries);
                    await _context.SaveChangesAsync();
                    academicSummaries.Clear();
                }

                await _context.AcademicSummaries.AddRangeAsync(academicSummaries);
                await _context.SaveChangesAsync();

                careerCount++;
            }
        }
        public async Task UpdateAcademicSummariesOrderJob()
        {
            var terms = await _context.Terms.ToListAsync();
            var academicSummaries = await _context.AcademicSummaries
                .OrderByDescending(x => x.WeightedAverageGrade)
                .OrderByDescending(x => x.CareerId)
                .OrderByDescending(x => x.TermId)
                .ToListAsync();
            var careers = await _context.Careers.ToListAsync();

            foreach (var term in terms)
            {
                foreach (var career in careers)
                {
                    var academicSummaryIndex = 0;

                    foreach (var academicSummary in academicSummaries)
                    {
                        if (academicSummary.CareerId == career.Id && academicSummary.TermId == term.Id)
                        {
                            academicSummary.MeritOrder = ++academicSummaryIndex;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAcademicSummariesOrder2Job()
        {
            var academicSummaries = await _context.AcademicSummaries
                .OrderByDescending(x => x.WeightedAverageGrade)
                .OrderByDescending(x => x.CareerId)
                .OrderByDescending(x => x.TermId)
                .ToListAsync();
            var terms = await _context.Terms.ToListAsync();
            var careers = await _context.Careers.ToListAsync();

            foreach (var term in terms)
            {
                foreach (var career in careers)
                {
                    var academicSummary = academicSummaries.BinarySearch((x) => x.CareerId == career.Id && x.TermId == term.Id, true);
                    var academicSummaryIndex = 0;

                    academicSummary.MeritOrder = ++academicSummaryIndex;
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<object> CalculateMeritOrder2Job(Guid termId, Guid curriculumId, int academicYear)
        {
            var curriculum = await _context.Curriculums.FindAsync(curriculumId);
            var career = await _context.Careers.FindAsync(curriculum.CareerId);
            var term = await _context.Terms.FindAsync(termId);

            var studentMerits = new List<StudentMeritTemplate>();

            var academicYearCredits = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculum.Id && x.AcademicYear == academicYear && !x.IsElective)
                .SumAsync(x => x.Course.Credits);

            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.Student.CurriculumId == curriculum.Id
                && x.StudentAcademicYear == academicYear
                && x.TermId == termId)
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .ToListAsync();

            foreach (var academicSummary in academicSummaries)
            {
                //var academicHistories = await _context.AcademicHistories
                //    .Where(x => x.StudentId == academicSummary.StudentId && x.TermId == term.Id 
                //    && x.Type == CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR)
                //    .ToListAsync();

                var studentMerit = new StudentMeritTemplate
                {
                    StudentId = academicSummary.StudentId,
                    Code = academicSummary.Student.User.UserName,
                    Name = academicSummary.Student.User.FullName,
                    Grade = academicSummary.WeightedAverageGrade,
                    ApprovedCredits = academicSummary.ApprovedCredits,
                    DisapprovedCredits = academicSummary.TotalCredits - academicSummary.ApprovedCredits,
                    ApprovedAllCredits = academicSummary.TotalCredits == academicSummary.ApprovedCredits ? 1 : 0,
                    ApprovedCreditsIsGreater = academicSummary.ApprovedCredits >= academicYearCredits ? 1 : 0,
                    //EnrolledCreditsIsGreater = academicSummary.TotalCredits >= academicYearCredits ? 1 : 0,
                    EnrolledCreditsIsGreaterThanTwelve = academicSummary.TotalCredits >= 12 ? 1 : 0
                };

                studentMerit.PriorityLevel = studentMerit.ApprovedAllCredits
                    + studentMerit.ApprovedCreditsIsGreater
                    + studentMerit.EnrolledCreditsIsGreater
                    + studentMerit.EnrolledCreditsIsGreaterThanTwelve;
                studentMerits.Add(studentMerit);
            }

            studentMerits = studentMerits
                .OrderByDescending(x => x.PriorityLevel)
                .ThenBy(x => x.DisapprovedCredits)
                .ThenByDescending(x => x.Grade)
                .ToList();

            var meritOrder = 1;
            var total = 1;
            var previousGrade = 0.00M;

            var studentTotal = studentMerits.Count;

            foreach (var item in studentMerits)
            {
                if (total != 1 && previousGrade != item.Grade)
                    meritOrder++;

                item.MeritOrder = meritOrder;
                item.TotalOrder = total;

                item.MeritType = meritOrder <= (studentTotal * 0.1) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH
                 : meritOrder <= (studentTotal * 0.20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                 : meritOrder <= (studentTotal * 0.33) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                 : CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE;

                previousGrade = item.Grade;
                total++;
            }

            foreach (var academicSummary in academicSummaries)
            {
                var merit = studentMerits.Where(x => x.StudentId == academicSummary.StudentId).FirstOrDefault();
                academicSummary.MeritOrder = merit.MeritOrder;
                academicSummary.TotalOrder = merit.TotalOrder;
                academicSummary.MeritType = merit.MeritType;
            }

            await _context.SaveChangesAsync();
            return studentMerits;
        }
        public async Task UpdateAcademicSummariesYearJob(Guid termId, string careerCode)
        {
            var count = 0;

            var career = await _context.Careers.FirstOrDefaultAsync(x => x.Code == careerCode);
            var term = await _context.Terms.FindAsync(termId);

            var curriculums = await _context.Curriculums
                .Where(x => x.CareerId == career.Id)
                .Select(x => new CurriculumTemplate
                {
                    Id = x.Id,
                    AcademicYears = x.AcademicYearCourses
                    .GroupBy(ay => ay.AcademicYear)
                    .Select(ay => new AcademicYearTemplate
                    {
                        Number = ay.Key,
                        Credits = ay
                        .Where(ayc => !ayc.IsElective).Sum(ayc => ayc.Course.Credits),
                        PrevCredits = 0,
                        RequiredCredits = 0
                    }).ToList()
                })
                .ToListAsync();

            foreach (var curriculum in curriculums)
            {
                var prevCredits = 0.0M;

                foreach (var academicYear in curriculum.AcademicYears.OrderBy(x => x.Number))
                {
                    academicYear.PrevCredits = prevCredits;
                    academicYear.RequiredCredits = academicYear.Number <= 1 ? 0 : prevCredits + (1.0M * academicYear.Credits / 2.0M) + 1;

                    prevCredits += academicYear.Credits;
                }
            }

            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.Student.CareerId == career.Id
                && x.TermId == term.Id)
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .ToListAsync();

            foreach (var academicSummary in academicSummaries)
            {
                var approvedCredits = await _context.AcademicHistories
                    .Where(x => x.StudentId == academicSummary.StudentId
                    && x.Term.StartDate <= term.StartDate
                    //&& x.Type == CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR
                    && x.Approved)
                    .SumAsync(x => x.Course.Credits);

                var curriculum = curriculums.FirstOrDefault(x => x.Id == academicSummary.Student.CurriculumId);
                var currentYear = curriculum.AcademicYears
                    .Where(x => x.RequiredCredits <= approvedCredits)
                    .OrderByDescending(x => x.Number)
                    .FirstOrDefault();

                academicSummary.StudentAcademicYear = currentYear.Number;
                count++;
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateAcademicSummariesYearJob(Guid termId)
        {
            var count = 0;

            var careers = await _context.Careers.ToListAsync();
            var term = await _context.Terms.FindAsync(termId);

            foreach (var career in careers)
            {
                var curriculums = await _context.Curriculums
             .Where(x => x.CareerId == career.Id)
             .Select(x => new CurriculumTemplate
             {
                 Id = x.Id,
                 AcademicYears = x.AcademicYearCourses
                 .GroupBy(ay => ay.AcademicYear)
                 .Select(ay => new AcademicYearTemplate
                 {
                     Number = ay.Key,
                     Credits = ay
                     .Where(ayc => !ayc.IsElective).Sum(ayc => ayc.Course.Credits),
                     PrevCredits = 0,
                     RequiredCredits = 0
                 }).ToList()
             })
             .ToListAsync();

                foreach (var curriculum in curriculums)
                {
                    var prevCredits = 0.0M;

                    foreach (var academicYear in curriculum.AcademicYears.OrderBy(x => x.Number))
                    {
                        academicYear.PrevCredits = prevCredits;
                        academicYear.RequiredCredits = academicYear.Number <= 1 ? 0 : prevCredits + (1.0M * academicYear.Credits / 2.0M) + 1;

                        prevCredits += academicYear.Credits;
                    }
                }

                var academicSummaries = await _context.AcademicSummaries
                    .Where(x => x.Student.CareerId == career.Id
                    && x.TermId == term.Id)
                    .Include(x => x.Student)
                    .ThenInclude(x => x.User)
                    .ToListAsync();

                foreach (var academicSummary in academicSummaries)
                {
                    var approvedCredits = await _context.AcademicHistories
                        .Where(x => x.StudentId == academicSummary.StudentId
                        && x.Term.StartDate <= term.StartDate
                        //&& x.Type == CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR
                        && x.Approved)
                        .SumAsync(x => x.Course.Credits);

                    var curriculum = curriculums.FirstOrDefault(x => x.Id == academicSummary.Student.CurriculumId);
                    var currentYear = curriculum.AcademicYears
                        .Where(x => x.RequiredCredits <= approvedCredits)
                        .OrderByDescending(x => x.Number)
                        .FirstOrDefault();

                    academicSummary.StudentAcademicYear = currentYear.Number;
                    count++;
                }

                await _context.SaveChangesAsync();
            }
        }
        public async Task<object> CalculateMeritOrderJob(Guid termId, Guid curriculumId, int academicYear)
        {
            var curriculum = await _context.Curriculums.FindAsync(curriculumId);
            var career = await _context.Careers.FindAsync(curriculum.CareerId);
            var term = await _context.Terms.FindAsync(termId);

            var studentMerits = new List<StudentMeritTemplate>();

            var academicYearCredits = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculum.Id && x.AcademicYear == academicYear && !x.IsElective)
                .SumAsync(x => x.Course.Credits);

            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.Student.CurriculumId == curriculum.Id
                && x.StudentAcademicYear == academicYear
                && x.TermId == termId)
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.Student.CurriculumId == curriculum.Id
                && x.TermId == termId)
                .ToListAsync();

            var approvedCreditsWeight = new int[] {
                100,
                99,
                98,
                97,
                96,
                95,
                94,
                93,
                92,
                91,
                90,
                89,
                88,
                87,
                86,
                85,
                84,
                83,
                82,
                81,
                80,
                79,
                78,
                77,
                76,
                75,
                74,
                73,
                72,
                71,
                70,
                69,
                68,
                67,
                66,
                65,
                64,
                63,
                62,
                61,
                60,
                59,
                58,
                57,
                56,
                55,
                54,
                53,
                52,
                51,
                50
            };

            foreach (var academicSummary in academicSummaries)
            {
                //var academicHistories = await _context.AcademicHistories
                //    .Where(x => x.StudentId == academicSummary.StudentId && x.TermId == term.Id 
                //    && x.Type == CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR)
                //    .ToListAsync();

                var text = "";

                var studentMerit = new StudentMeritTemplate
                {
                    StudentId = academicSummary.StudentId,
                    Code = academicSummary.Student.User.UserName,
                    Name = academicSummary.Student.User.FullName,
                    Grade = academicSummary.WeightedAverageGrade,
                    ApprovedCredits = academicSummary.ApprovedCredits,
                    DisapprovedCredits = academicSummary.TotalCredits - academicSummary.ApprovedCredits,
                    ApprovedAllCredits = academicSummary.TotalCredits == academicSummary.ApprovedCredits ? 1 : 0,
                    ApprovedCreditsIsGreater = academicSummary.ApprovedCredits >= academicYearCredits ? 1 : 0,
                    //EnrolledCreditsIsGreater = academicSummary.TotalCredits >= academicYearCredits ? 1 : 0,
                    EnrolledCreditsIsGreaterThanTwelve = academicSummary.TotalCredits >= 12 ? 1 : 0
                };


                /***********************************************************************************************
				 ***********************************************************************************************
				 * 2 -XX- aprobados > 0 y promedio > 0
				 ***********************************************************************************************
				 ***********************************************************************************************/

                if (studentMerit.ApprovedCredits > 0) text = text + "0";
                else text = text + "1";

                if (studentMerit.Grade > 0) text = text + "0";
                else text = text + "1";

                /***********************************************************************************************
				 ***********************************************************************************************
				 * 3 -X- CONTROL DEL 100% DE CREDITO SEMESTRAL APROBADO
				 ***********************************************************************************************
				 ***********************************************************************************************/

                if (academicSummary.ApprovedCredits >= academicYearCredits) text = text + "0";
                else text = text + "1";

                text = text + "--";

                /***********************************************************************************************
				 ***********************************************************************************************
				 * 4 -X- 2018-I Para priorizar por cantidad de creditos en caso de que no tenga el creditaje minimo del semestre (2018-09-26)
				 ***********************************************************************************************
				 ***********************************************************************************************/

                if (academicSummary.ApprovedCredits >= academicYearCredits) text = text + "000";
                else if (academicSummary.ApprovedCredits >= 12) text = text + "001";
                else text = text + approvedCreditsWeight[int.Parse(Math.Round(academicSummary.ApprovedCredits, 0, MidpointRounding.AwayFromZero).ToString())].ToString().PadLeft(3, '0');

                text = text + "....";

                /***********************************************************************************************
				 ***********************************************************************************************
				 * 5 -XX-  Crd des = Crd. Mat - Crd. Aprb
				 ***********************************************************************************************
				 ***********************************************************************************************/

                text = text + studentMerit.DisapprovedCredits.ToString().PadLeft(2, '0');

                text = text + "-";

                /***********************************************************************************************
				 ***********************************************************************************************
				 * 6 -XXX-  MODALIDAD DE MATRICULA
				 ***********************************************************************************************
				 ***********************************************************************************************/

                //PESO DE LAS MODALIDADES
                var canmat = 99;

                var studentAcademicHistories = academicHistories.Where(x => x.StudentId == academicSummary.StudentId).ToList();

                if (studentAcademicHistories.Any()) canmat = studentAcademicHistories.OrderByDescending(x => x.Try).FirstOrDefault().Try;

                if (canmat == 2) canmat = 1;

                if (canmat == 99)
                {
                    var TEST1234 = "";
                }

                studentMerit.MaxTry = canmat;

                text = text + canmat.ToString().PadLeft(2, '0');

                text = text + "...";


                studentMerit.OrderPosition = text;

                studentMerit.PriorityLevel = studentMerit.ApprovedAllCredits
                    + studentMerit.ApprovedCreditsIsGreater
                    + studentMerit.EnrolledCreditsIsGreater
                    + studentMerit.EnrolledCreditsIsGreaterThanTwelve;
                studentMerits.Add(studentMerit);
            }

            /***********************************************************************************************
			***********************************************************************************************
			* 6 -XXX- Orden de Nota
			***********************************************************************************************
			***********************************************************************************************/

            var orderGrade = 1;

            studentMerits = studentMerits
               .OrderByDescending(x => x.Grade)
               .ToList();

            foreach (var item in studentMerits)
            {
                item.OrderPosition = item.OrderPosition + orderGrade.ToString().PadLeft(3, '0');
                orderGrade++;
            }

            studentMerits = studentMerits
             .OrderBy(x => x.OrderPosition)
             .ToList();

            //studentMerits = studentMerits
            //    .OrderByDescending(x => x.PriorityLevel)
            //    .ThenBy(x => x.DisapprovedCredits)
            //    .ThenByDescending(x => x.Grade)
            //    .ToList();

            var meritOrder = 1;
            var total = 1;
            var previousGrade = 0.00M;

            var studentTotal = studentMerits.Count;

            foreach (var item in studentMerits)
            {
                if (total != 1 && previousGrade != item.Grade)
                    meritOrder++;

                item.MeritOrder = meritOrder;
                item.TotalOrder = total;

                item.MeritType = meritOrder <= (studentTotal * 0.1) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH
                 : meritOrder <= (studentTotal * 0.20) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                 : meritOrder <= (studentTotal * 0.33) ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                 : CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE;

                previousGrade = item.Grade;
                total++;
            }

            foreach (var academicSummary in academicSummaries)
            {
                var merit = studentMerits.Where(x => x.StudentId == academicSummary.StudentId).FirstOrDefault();
                academicSummary.MeritOrder = merit.MeritOrder;
                academicSummary.TotalOrder = merit.TotalOrder;
                academicSummary.MeritType = merit.MeritType;
            }

            await _context.SaveChangesAsync();
            return studentMerits;
        }

        #endregion

        #region Reports

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudents(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, string search)
        {
            Expression<Func<AcademicSummary, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Term.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Faculty.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.WeightedAverageGrade);
                    break;
                default:
                    orderByPredicate = ((x) => x.Student.User.Name);
                    break;
            }

            var query = _context.AcademicSummaries.AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            query = query.Where(x => x.WeightedAverageGrade < x.Term.MinGrade);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    term = x.Term.Name,
                    score = (x.WeightedAverageGrade < 0 ? "0.00" : x.WeightedAverageGrade.ToString("0.00")),
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    student = x.Student.User.FullName,
                    code = x.Student.User.UserName
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<DisapprovedStudentsTemplate>> GetDisapprovedStudentsTemplate(Guid? termId, Guid? careerId, Guid? facultyId)
        {
            var query = _context.AcademicSummaries.AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            query = query.Where(x => x.WeightedAverageGrade < x.Term.MinGrade);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new DisapprovedStudentsTemplate
                {
                    Term = x.Term.Name,
                    Score = (x.WeightedAverageGrade < 0 ? "0.00" : x.WeightedAverageGrade.ToString("0.00")),
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    FullName = x.Student.User.FullName,
                    UserName = x.Student.User.UserName
                }).ToListAsync();

            return data;

        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetTermsByStudent(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user)
        {

            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Faculty.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.StudentSections.Select(y => y.Section.CourseTerm.TermId).Distinct().Count());
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }

            var query = _context.Students.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
                else
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.CareerDirectorId == userId).Select(x => x.Id).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    student = x.User.FullName,
                    code = x.User.UserName,
                    terms = x.StudentSections.Select(y => y.Section.CourseTerm.TermId).Distinct().Count()
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<TermByStudentTemplate>> GetTermsByStudentTemplate(Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user)
        {
            var query = _context.Students.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
                else
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.CareerDirectorId == userId).Select(x => x.Id).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            var data = await query
                .Select(x => new TermByStudentTemplate
                {
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Terms = x.StudentSections.Select(y => y.Section.CourseTerm.TermId).Distinct().Count()
                }).ToListAsync();

            return data;
        }

        public async Task<List<AcademicSummary>> GetMeritChartAcademicSummaries(Guid termId, Guid facultyId, Guid careerId, Guid? curriculumId = null, int? year = null, int? academicOrder = null, ClaimsPrincipal user = null)
        {
            var query = _context.AcademicSummaries.Where(x => x.TermId == termId).AsNoTracking();

            if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.CurriculumId == curriculumId);

            if (year.HasValue) query = query.Where(x => x.StudentAcademicYear == year);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Any(y => y == x.CareerId));
                }
            }

            if (academicOrder.HasValue)
            {
                switch (academicOrder.Value)
                {
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF);
                        break;
                    default:
                        break;
                }
            }

            var data = await query
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Career)
                .Include(x => x.Student.AcademicProgram)
                .Include(x => x.Student.AcademicHistories)
                .ToListAsync();

            return data;
        }

        public async Task<List<RowChild4>> GetReportStudentByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var competences = await _context.Competencies.ToListAsync();
            var listFinal = new List<RowChild4>();

            var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id &&
                                        x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
                                        x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();

            var lstStudentListAcademicSummaries = await queryAcademicSummaries.Select(x => x.StudentId).ToListAsync();

            var lst1 = await queryAcademicSummaries.Select(x => new
            {
                x.StudentId,
                x.Student.User.FullName
            }).ToListAsync();


            //limpia repetidos
            var studentsId = lst1.Select(x => x.StudentId).ToHashSet();


            foreach (var item in competences)
            {
                var query = _context.AcademicHistories.Where(x => studentsId.Contains(x.StudentId) &&
                            x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == item.Id)
                            && x.Term.StartDate <= term.StartDate).AsQueryable();

                var dataQuery = await query
                    .Select(x => new
                    {
                        CourseId = x.CourseId,
                        CourseName = x.Course.Name,
                        Grade = (decimal)x.Grade,
                        Credits = x.Course.Credits,
                        StudentId = x.StudentId,
                        StudentFullName = x.Student.User.FullName,
                        UserName = x.Student.User.UserName,
                        Try = x.Try,
                        Term = x.Term.Name,
                        Number = x.Term.Number
                    })
                    .ToListAsync();

                var objData = dataQuery
                   .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild2
                   {
                       StudentId = x.Key.StudentId,
                       CourseName = x.Key.CourseName,
                       Grade = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault(),
                       Credits = x.Key.Credits,
                       StudentFullName = x.Key.StudentFullName,
                       UserName = x.Key.UserName

                   }).ToList();

                if (objData.Count > 0)
                {
                    var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild4
                    {
                        StudentId = x.Key.StudentId,
                        StudentFullName = x.Key.StudentFullName,
                        UserName = x.Key.UserName,
                        Average = Math.Round(x.Sum(s => s.Grade * s.Credits) / x.Sum(s => s.Credits), 2),
                        CompetenceName = item.Name,
                        CompetenceId = item.Id

                    }).ToList();

                    listFinal.AddRange(objDataFinal);

                }
            }
            return listFinal;

        }

        public async Task<object> ReportGradesByStudentCourses(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var competences = await _context.Competencies.Where(x => x.Id == competenceId).ToListAsync();
            var listFinal = new List<RowChild4>();

            var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id && x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
            x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();


            var query = _context.AcademicHistories.Where(x => x.StudentId == studentId &&
                            x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == competenceId)
                            && x.Term.StartDate <= term.StartDate).AsQueryable();

            var dataQuery = await query
                .Select(x => new
                {
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Grade = (decimal)x.Grade,
                    Credits = x.Course.Credits,
                    StudentId = x.StudentId,
                    StudentFullName = x.Student.User.FullName,
                    UserName = x.Student.User.UserName,
                    Try = x.Try,
                    Term = x.Term.Name,
                    Number = x.Term.Number
                })
                .ToListAsync();

            var objData = dataQuery
               .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild2
               {
                   StudentId = x.Key.StudentId,
                   CourseName = x.Key.CourseName,
                   Grade = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault(),
                   Credits = x.Key.Credits,
                   StudentFullName = x.Key.StudentFullName,
                   UserName = x.Key.UserName

               }).ToList();

            return objData;
        }

        public async Task<List<DataReport2>> AchievementLevelAcademicYearCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var competences = await _context.Competencies.ToListAsync();
            var listFinal = new List<RowChild4>();

            var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id &&
                                        x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
                                        x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();

            var lstStudentListAcademicSummaries = await queryAcademicSummaries.Select(x => x.StudentId).ToListAsync();

            var result2 = new List<DataReport2>();

            var lst1 = await queryAcademicSummaries.Select(x => new
            {
                x.StudentId,
                x.Student.User.FullName
            }).ToListAsync();


            //limpia repetidos
            var studentsId = lst1.Select(x => x.StudentId).ToHashSet();


            foreach (var item in competences)
            {
                var RangeLevelList = new List<RangeLevel>();
                RangeLevelList.Add(new RangeLevel
                {
                    Name = "Deficiente",
                    Min = 0,
                    Max = 10,
                    Total = 0,
                    Type = 1
                });
                RangeLevelList.Add(new RangeLevel
                {
                    Name = "Regular",
                    Min = 11,
                    Max = 13,
                    Total = 0,
                    Type = 2
                });
                RangeLevelList.Add(new RangeLevel
                {
                    Name = "Bueno",
                    Min = 14,
                    Max = 16,
                    Total = 0,
                    Type = 3
                });
                RangeLevelList.Add(new RangeLevel
                {
                    Name = "Excelente",
                    Min = 17,
                    Max = 20,
                    Total = 0,
                    Type = 4
                });


                var query = _context.AcademicHistories.Where(x => studentsId.Contains(x.StudentId) &&
                            x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == item.Id)
                            && x.Term.StartDate <= term.StartDate).AsQueryable();

                var dataQuery = await query
                    .Select(x => new
                    {
                        CourseId = x.CourseId,
                        CourseName = x.Course.Name,
                        Grade = (decimal)x.Grade,
                        Credits = x.Course.Credits,
                        StudentId = x.StudentId,
                        StudentFullName = x.Student.User.FullName,
                        UserName = x.Student.User.UserName,
                        Try = x.Try,
                        Term = x.Term.Name,
                        Number = x.Term.Number
                    })
                    .ToListAsync();

                var objData = dataQuery
                   .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild2
                   {
                       StudentId = x.Key.StudentId,
                       CourseName = x.Key.CourseName,
                       Grade = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault(),
                       Credits = x.Key.Credits,
                       StudentFullName = x.Key.StudentFullName,
                       UserName = x.Key.UserName

                   }).ToList();

                if (objData.Count > 0)
                {
                    var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild4
                    {
                        StudentId = x.Key.StudentId,
                        StudentFullName = x.Key.StudentFullName,
                        UserName = x.Key.UserName,
                        Average = Math.Round(x.Sum(s => s.Grade * s.Credits) / x.Sum(s => s.Credits), 0),
                        CompetenceName = item.Name,
                        CompetenceId = item.Id

                    }).ToList();


                    foreach (var i in RangeLevelList)
                    {
                        foreach (var j in objDataFinal)
                        {
                            if (i.Min <= j.Average && j.Average <= i.Max)
                            {
                                i.Total++;
                            }
                        }
                    }
                    result2.Add(new DataReport2
                    {
                        CompetenceName = item.Name,
                        CompetenceId = item.Id,
                        RangeLevels = RangeLevelList
                    });
                }
            }
            return result2;
        }

        public async Task<object> AchievementLevelAcademicYearCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, byte academicYear, List<RangeLevel> rangeLevelList)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            Competencie competence = null;
            if (competenceId != Guid.Empty)
            {
                competence = await _context.Competencies.Where(x => x.Id == competenceId).FirstOrDefaultAsync();
            }
            var typeRange = rangeLevelList.FirstOrDefault().Name;
            var minAvg = rangeLevelList.FirstOrDefault().Min;
            var maxAvg = rangeLevelList.FirstOrDefault().Max;
            var typeRangeList = rangeLevelList.FirstOrDefault().Type;

            var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id &&
                                        x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
                                        x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();

            var lstStudentListAcademicSummaries = await queryAcademicSummaries.Select(x => x.StudentId).ToListAsync();

            var result2 = new List<DataReport2>();

            var lst1 = await queryAcademicSummaries.Select(x => new
            {
                x.StudentId,
                x.Student.User.FullName
            }).ToListAsync();


            //limpia repetidos
            var studentsId = lst1.Select(x => x.StudentId).ToHashSet();

            var query = _context.AcademicHistories.Where(x => studentsId.Contains(x.StudentId) &&
                           x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == competenceId)
                           && x.Term.StartDate <= term.StartDate).AsQueryable();

            var dataQuery = await query
                .Select(x => new
                {
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Grade = (decimal)x.Grade,
                    Credits = x.Course.Credits,
                    StudentId = x.StudentId,
                    StudentFullName = x.Student.User.FullName,
                    UserName = x.Student.User.UserName,
                    Try = x.Try,
                    Term = x.Term.Name,
                    Number = x.Term.Number
                })
                .ToListAsync();

            var objData = dataQuery
               .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild2
               {
                   StudentId = x.Key.StudentId,
                   CourseName = x.Key.CourseName,
                   Grade = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault(),
                   Credits = x.Key.Credits,
                   StudentFullName = x.Key.StudentFullName,
                   UserName = x.Key.UserName

               }).ToList();

            if (objData.Count() > 0)
            {
                var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild3
                {

                    AverageInt = Math.Round(x.Sum(s => s.Grade * s.Credits) / x.Sum(s => s.Credits), 0),
                    Average = Math.Round(x.Sum(s => s.Grade * s.Credits) / x.Sum(s => s.Credits), 2),
                    CompetenceId = competence.Id,
                    StudentFullName = x.Key.StudentFullName,
                    UserName = x.Key.UserName,
                    StudentId = x.Key.StudentId,
                    Type = typeRangeList

                }).ToList();

                var total = 0;
                total = objDataFinal.Count();

                objDataFinal = objDataFinal.Where(x => minAvg <= x.AverageInt && x.AverageInt <= maxAvg).ToList();

                var percentaje = Math.Round(((decimal)objDataFinal.Count() / (decimal)total * 100), 2);

                var finalResult = new DataReport3
                {
                    CompetenceName = $"{competence.Name} - {typeRange} ({percentaje}%)",
                    RowChilds = objDataFinal
                };

                return finalResult;
            }
            else
            {
                var finalResult = new DataReport3
                {
                    CompetenceName = $"{competence.Name} - {typeRange}",
                    RowChilds = new List<RowChild3>()
                };

                return finalResult;
            }


        }

        public async Task<object> AchievementLevelAcademicYearStudentCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear, List<RangeLevel> rangeLevelList)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var studentFullName = await _context.Students.Where(x => x.Id == studentId).Select(x => x.User.FullName).FirstOrDefaultAsync();
            Competencie competence = null;
            if (competenceId != Guid.Empty)
            {
                competence = await _context.Competencies.Where(x => x.Id == competenceId).FirstOrDefaultAsync();
            }
            var typeRange = rangeLevelList.FirstOrDefault().Name;
            var minAvg = rangeLevelList.FirstOrDefault().Min;
            var maxAvg = rangeLevelList.FirstOrDefault().Max;
            var typeRangeList = rangeLevelList.FirstOrDefault().Type;

            //var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id &&
            //                            x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
            //                            x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();

            //var lstStudentListAcademicSummaries = await queryAcademicSummaries.Select(x => x.StudentId).ToListAsync();

            var result2 = new List<DataReport2>();

            //var lst1 = await queryAcademicSummaries.Select(x => new
            //{
            //    x.StudentId,
            //    x.Student.User.FullName
            //}).ToListAsync();


            //limpia repetidos
            //var studentsId = lst1.Select(x => x.StudentId).ToHashSet();

            var query = _context.AcademicHistories.Where(x => x.StudentId == studentId &&
                           x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == competenceId)
                           && x.Term.StartDate <= term.StartDate).AsQueryable();

            var dataQuery = await query
                .Select(x => new
                {
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Grade = (decimal)x.Grade,
                    Credits = x.Course.Credits,
                    StudentId = x.StudentId,
                    StudentFullName = x.Student.User.FullName,
                    UserName = x.Student.User.UserName,
                    Try = x.Try,
                    Term = x.Term.Name,
                    Number = x.Term.Number
                })
                .ToListAsync();

            var objData = dataQuery
               .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild3
               {
                   CourseName = x.Key.CourseName,
                   Credits = x.Key.Credits,
                   Average = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault()

               }).ToList();

            var finalResult = new DataReport3
            {
                StudentFullName = studentFullName,
                RowChilds = objData
            };

            return finalResult;
        }

        public async Task<object> GetReportAchievementLevelAcademicYear(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var competences = await _context.Competencies.ToListAsync();
            var listFinal = new List<RowChild4>();

            var queryAcademicSummaries = _context.AcademicSummaries.Where(x => x.TermId == term.Id &&
                                        x.StudentAcademicYear == academicYear && x.CurriculumId == curriculumId &&
                                        x.Student.Career.FacultyId == facultyId && x.Student.CareerId == careerId).AsQueryable();

            var lstStudentListAcademicSummaries = await queryAcademicSummaries.Select(x => x.StudentId).ToListAsync();

            var result2 = new List<DataReport2>();

            var lst1 = await queryAcademicSummaries.Select(x => new
            {
                x.StudentId,
                x.Student.User.FullName
            }).ToListAsync();


            //limpia repetidos
            var studentsId = lst1.Select(x => x.StudentId).ToHashSet();

            var RangeLevelList = new List<RangeLevel>();
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Deficiente",
                Min = 0,
                Max = 10,
                Total = 0,
                Type = 1,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Regular",
                Min = 11,
                Max = 13,
                Total = 0,
                Type = 2,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Bueno",
                Min = 14,
                Max = 16,
                Total = 0,
                Type = 3,
                Array = new List<int>()
            });
            RangeLevelList.Add(new RangeLevel
            {
                Name = "Excelente",
                Min = 17,
                Max = 20,
                Total = 0,
                Type = 4,
                Array = new List<int>()
            });

            var lstCompetencesStrings = new List<string>();


            foreach (var item in competences.OrderBy(x => x.Name).ToList())
            {

                var query = _context.AcademicHistories.Where(x => studentsId.Contains(x.StudentId) &&
                            x.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId && z.CompetencieId == item.Id)
                            && x.Term.StartDate <= term.StartDate).AsQueryable();

                var dataQuery = await query
                    .Select(x => new
                    {
                        CourseId = x.CourseId,
                        CourseName = x.Course.Name,
                        Grade = (decimal)x.Grade,
                        Credits = x.Course.Credits,
                        StudentId = x.StudentId,
                        StudentFullName = x.Student.User.FullName,
                        UserName = x.Student.User.UserName,
                        Try = x.Try,
                        Term = x.Term.Name,
                        Number = x.Term.Number
                    })
                    .ToListAsync();

                var objData = dataQuery
                   .GroupBy(x => new { x.CourseId, x.CourseName, x.Credits, x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild2
                   {
                       StudentId = x.Key.StudentId,
                       CourseName = x.Key.CourseName,
                       Grade = x.OrderByDescending(r => r.Term).ThenByDescending(r => r.Number).Select(z => z.Grade).FirstOrDefault(),
                       Credits = x.Key.Credits,
                       StudentFullName = x.Key.StudentFullName,
                       UserName = x.Key.UserName

                   }).ToList();

                if (objData.Count > 0)
                {
                    var objDataFinal = objData.GroupBy(x => new { x.StudentId, x.StudentFullName, x.UserName }).Select(x => new RowChild4
                    {
                        StudentId = x.Key.StudentId,
                        StudentFullName = x.Key.StudentFullName,
                        UserName = x.Key.UserName,
                        Average = Math.Round(x.Sum(s => s.Grade * s.Credits) / x.Sum(s => s.Credits), 0),
                        CompetenceName = item.Name,
                        CompetenceId = item.Id

                    }).ToList();

                    lstCompetencesStrings.Add(item.Name);

                    foreach (var i in RangeLevelList)
                    {
                        var totalDisp = 0;
                        foreach (var j in objDataFinal)
                        {
                            if (i.Min <= j.Average && j.Average <= i.Max)
                            {
                                totalDisp++;
                            }
                        }
                        i.Array.Add(totalDisp);
                    }

                }
            }
            lstCompetencesStrings.Sort();

            var result = new DataReport2
            {
                RangeLevels = RangeLevelList,
                CompetencesNames = lstCompetencesStrings
            };
            return result;


        }

        public async Task<List<DisapprovedStudentsByTermTemplate>> GetDisapprrovedStudentsByTerm(int startYear, int endYear)
        {
            var data = await _context.Terms
                .Where(x => !x.IsSummer && startYear <= x.Year && x.Year <= endYear)
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Number)
                .Select(x => new DisapprovedStudentsByTermTemplate
                {
                    Term = x.Name,
                    DisapprovedStudents = x.AcademicSummaries.Where(y => y.ApprovedCredits == 0).Count(),
                    TotalStudents = x.AcademicSummaries.Count()
                }).ToListAsync();

            return data;
        }

        public async Task<StudentDistributionByTimeTemplate> StudentDistributionByEnrolledTime(Guid termId)
        {
            var term = await _context.Terms.FindAsync(termId);

            var students = await _context.Students
                .Where(x => x.AcademicSummaries.Any(y => y.TermId == termId))
                .Select(x => new
                {
                    terms = term.Number == "1" ?
                        x.AcademicSummaries.Where(y => y.Term.Year < term.Year && (y.Term.Number == "1" || y.Term.Number == "2")).Count()
                        : x.AcademicSummaries.Where(y => (y.Term.Year < term.Year && (y.Term.Number == "1" || y.Term.Number == "2")) || (y.Term.Year == term.Year && y.Term.Number == "1")).Count()
                }).ToListAsync();

            var data = new StudentDistributionByTimeTemplate
            {
                Term = term.Name,
                SixYears = students.Where(x => x.terms <= 12).Count(),
                TenYears = students.Where(x => 12 < x.terms && x.terms <= 20).Count(),
                FifteenYears = students.Where(x => 20 < x.terms && x.terms <= 30).Count(),
                TwentyYears = students.Where(x => 30 < x.terms && x.terms <= 40).Count(),
                MoreThanTwentyYears = students.Where(x => 40 < x.terms).Count(),
                Total = students.Count
            };

            return data;
        }

        public async Task<object> GetAcademicSummariesReportByStudent(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).Select(x => new
            {
                x.Id,
                x.CurriculumId,
                x.CareerId
            }).FirstOrDefaultAsync();

            var data = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId /*&& x.CareerId == student.CareerId && x.CurriculumId == student.CurriculumId*/)
                .OrderBy(x => x.Term.Year).ThenBy(x => x.Term.Number)
                .Select(x => new
                {
                    x.TermId,
                    term = x.Term.Name,
                    x.WeightedAverageGrade
                })
                .ToListAsync();

            return new
            {
                terms = data.Select(x => x.term).ToList(),
                averages = data.Select(x => x.WeightedAverageGrade).ToList()
            };
        }

        #endregion
    }
}
