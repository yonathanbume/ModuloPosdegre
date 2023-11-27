using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleExtraPerformanceEvaluationFieldRepository : Repository<ScaleExtraPerformanceEvaluationField>, IScaleExtraPerformanceEvaluationFieldRepository
    {
        public ScaleExtraPerformanceEvaluationFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<ScaleExtraPerformanceEvaluationField, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ScaleResolution.ResolutionNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolution.Observation); break;
                case "2":
                    orderByPredicate = ((x) => x.ScaleResolution.BeginDate); break;
                case "3":
                    orderByPredicate = ((x) => x.ScaleResolution.EndDate); break;
                case "4":
                    orderByPredicate = ((x) => x.EvaluationType); break;
                case "5":
                    orderByPredicate = ((x) => x.Qualification); break;
                case "6":
                    orderByPredicate = ((x) => x.ScaleResolution.Observation); break;
                default:
                    orderByPredicate = ((x) => x.ScaleResolution.ResolutionNumber); break;
            }

            var query = _context.ScaleExtraPerformanceEvaluationFields
                .Where(x => x.ScaleResolution.UserId == userId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    evaluationType = x.EvaluationType ?? "-",
                    beginDate = x.ScaleResolution.BeginDate.ToLocalDateFormat() ?? "-",
                    scaleResolutionType = x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name ?? "-",
                    endDate = x.ScaleResolution.EndDate.ToLocalDateFormat() ?? "-",
                    resolutionNumber = x.ScaleResolution.ResolutionNumber ?? "-",
                    observations = x.ScaleResolution.Observation,
                    qualification = x.Qualification,
                    baseScore = x.BaseScore
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

        public async Task<ScaleExtraPerformanceEvaluationField> GetScaleExtraPerformanceEvaluationFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraPerformanceEvaluationFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }

        public async Task<TeachingPerformanceResultTemplate> GetPerformanceEvaluationResult(Guid? academicDeparmentId, List<Guid> listTerms, string search)
        {
            var terms = await _context.Terms.Where(x => listTerms.Contains(x.Id))
                .OrderBy(x=>x.Year)
                .ThenBy(x=>x.Number)
                .Select(x=> new
                {
                    x.Id,
                    x.Name,
                    evaluations = x.PerformanceEvaluations.OrderBy(y=>y.StartDate).Select(y=> new
                    {
                        y.Code,
                        y.Id
                    }).ToList()
                })
                .ToListAsync();

            var query = _context.Teachers
                .Where(x => x.TeacherSections.Any(y => listTerms.Contains(y.Section.CourseTerm.TermId)))
                .AsNoTracking();

            if (academicDeparmentId.HasValue && academicDeparmentId != Guid.Empty)
            {
                query = query.Where(x => x.AcademicDepartmentId == academicDeparmentId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.User.UserName.ToLower().Trim().Contains(search) || x.User.FullName.ToLower().Trim().Contains(search));
            }


            var dataDB = await query
                .Select(x => new TeachingPerformanceTemplate
                {
                    Terms = terms.Select(y => new TeachingPerformanceTermTemplate
                    {
                        Id = y.Id,
                        Term = y.Name
                    }).ToList(),
                    UserName = x.User.UserName,
                    AcademicDepartment = x.AcademicDepartment.Name,
                    Teacher = x.User.FullName,
                    UserId = x.UserId
                })
                .ToListAsync();

            var teachersId = dataDB.Select(x => x.UserId).ToHashSet();

            var performanceEvaluations = await _context.ScaleExtraPerformanceEvaluationFields
                .Where(x => x.TermId.HasValue && teachersId.Contains(x.ScaleResolution.UserId) && listTerms.Contains(x.TermId.Value))
                .Select(x => new
                {
                    x.EvaluationType,
                    beginDate = x.ScaleResolution.BeginDate,
                    x.Qualification,
                    userId = x.ScaleResolution.UserId,
                    x.TermId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new TeachingPerformanceTemplate
                {
                    Terms = terms.Select(t => new TeachingPerformanceTermTemplate
                    {
                        Id = t.Id,
                        Term = t.Name,
                        Evaluations = performanceEvaluations.Where(y=>y.userId == x.UserId).Select(y=> new TeachingPerformanceEvaluationTemplate
                        {
                            EvaluationCode = y.EvaluationType,
                            Qualification = y.Qualification
                        }).ToList(),
                    }).ToList(),
                    UserName = x.UserName,
                    AcademicDepartment = x.AcademicDepartment ?? "Sin Asignar",
                    Teacher = x.Teacher,
                    UserId = x.UserId
                })
                .ToList();

            var result = new TeachingPerformanceResultTemplate
            {
                Terms = terms.Select(x=> new TeachingPerformanceTermTemplate
                {
                    Id = x.Id,
                    Term = x.Name,
                    Evaluations = x.evaluations.Select(y=> new TeachingPerformanceEvaluationTemplate 
                    { 
                        EvaluationCode = y.Code
                    }).ToList()
                }).ToList(),
                Teachers = data
            };

            return result;
        }

        public async Task<bool> AnyByCodeAndTerm(string code, Guid termId)
            => await _context.ScaleExtraPerformanceEvaluationFields.AnyAsync(x => x.EvaluationType == code && x.TermId == termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPerformanceEvaluationPublishedResultsDatatable(DataTablesStructs.SentParameters parameters, Guid performanceEvaluationId, string search)
        {
            Expression<Func<ScaleExtraPerformanceEvaluationField, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ScaleResolution.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolution.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.ScaleResolution.User.Teachers.Select(y => y.AcademicDepartment.Name).FirstOrDefault()); break;
                case "3":
                    orderByPredicate = ((x) => x.BaseScore); break;
                case "4":
                    orderByPredicate = ((x) => x.Qualification); break;
                default:
                    orderByPredicate = ((x) => x.Qualification); break;
            }

            var performanceEvaluation = await _context.PerformanceEvaluations.Where(x => x.Id == performanceEvaluationId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    term = x.Term.Name,
                    x.TermId
                })
                .FirstOrDefaultAsync();

            var query = _context.ScaleExtraPerformanceEvaluationFields
                .Where(x => x.EvaluationType == performanceEvaluation.Code && x.TermId == performanceEvaluation.TermId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.ScaleResolution.User.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.ScaleResolution.User.UserName.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(parameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.ScaleResolution.User.UserName,
                    x.ScaleResolution.User.FullName,
                    academicDepartment = x.ScaleResolution.User.Teachers.Select(y => y.AcademicDepartment.Name ?? "Sin Asignar").FirstOrDefault(),
                    x.BaseScore,
                    x.Qualification
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };

        }

        public async Task<List<PerformanceEvaluationResultTemplate>> GetPerformanceEvaluationPublishedResults(Guid performanceEvaluationId)
        {
            var performanceEvaluation = await _context.PerformanceEvaluations.Where(x => x.Id == performanceEvaluationId)
              .Select(x => new
              {
                  x.Id,
                  x.Code,
                  x.Name,
                  term = x.Term.Name,
                  x.TermId
              })
              .FirstOrDefaultAsync();

            var query = _context.ScaleExtraPerformanceEvaluationFields
                .Where(x => x.EvaluationType == performanceEvaluation.Code && x.TermId == performanceEvaluation.TermId)
                .AsNoTracking();

            var data = await query
                .OrderByDescending(x => x.Qualification)
                .Select(x => new PerformanceEvaluationResultTemplate
                {
                    UserName = x.ScaleResolution.User.UserName,
                    FullName = x.ScaleResolution.User.FullName,
                    AcademicDepartment = x.ScaleResolution.User.Teachers.Select(y => y.AcademicDepartment.Name ?? "Sin Asignar").FirstOrDefault(),
                    BaseScore = x.BaseScore,
                    Qualification = x.Qualification,
                    VigesimalScaleQuaiflication = x.Qualification.HasValue && x.BaseScore != 0 ? Math.Round((x.Qualification.Value * 20M / x.BaseScore.Value), 2, MidpointRounding.AwayFromZero) : 0
                })
                .ToListAsync();

            return data;
        }
    }
}
