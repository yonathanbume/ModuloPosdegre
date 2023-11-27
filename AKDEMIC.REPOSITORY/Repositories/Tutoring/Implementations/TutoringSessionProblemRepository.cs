using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringSessionProblemRepository : Repository<TutoringSessionProblem>, ITutoringSessionProblemRepository
    {
        public TutoringSessionProblemRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTutoringSessionIdAndProblemId(Guid tutoringSessionId, Guid tutoringProblemId)
            => await _context.TutoringSessionProblems.AnyAsync(x => x.TutoringSessionId == tutoringSessionId && x.TutoringProblemId == tutoringProblemId);
        
        public async Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null, bool? isMultiple = null)
        {
            var query = _context.TutoringSessionProblems.AsQueryable();

            if (tutoringProblemId.HasValue)
                query = query.Where(x => x.TutoringProblemId == tutoringProblemId.Value);
            if (category.HasValue)
                query = query.Where(x => x.TutoringProblem.Category == category.Value);
            if (termId.HasValue)
                query = query.Where(x => x.TutoringSession.TermId == termId.Value);
            if (careerId.HasValue)
                query = query.Where(x => x.TutoringSession.Tutor.CareerId == careerId.Value);
            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutoringSession.TutorId == tutorId);
            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSession.TutoringSessionStudents.Count() > 1 : x.TutoringSession.TutoringSessionStudents.Count() == 1);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TutoringSessionProblem>> GetAllWithInclude()
            => await _context.TutoringSessionProblems
                    .Include(x => x.TutoringProblem)
                    .Include(x => x.TutoringSession).ThenInclude(x => x.Tutor).ToListAsync();
        public async Task<IEnumerable<TutoringSessionProblem>> GetAllByTutoringSessionId(Guid tutoringSessionId)
            => await _context.TutoringSessionProblems
                .Include(x => x.TutoringProblem)
                .Where(x => x.TutoringSessionId == tutoringSessionId)
                .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<TutoringSessionProblem>> GetTutoringSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringSessionId = null)
        {
            var query = _context.TutoringSessionProblems
                .Where(x => x.TutoringSessionId == tutoringSessionId)
                //.WhereSearchValue(x => new[]
                //{
                //    ConstantHelpers.TUTORING.PROBLEM_CATEGORIES.VALUES[x.TutoringProblem.Category],
                //    x.TutoringProblem.Code,
                //    x.TutoringProblem.Description
                //}, searchValue)
                .AsNoTracking();

            Expression<Func<TutoringSessionProblem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.TutoringProblem.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringProblem.Category);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringProblem.Description);

                    break;
                default:
                    orderByPredicate = ((x) => x.TutoringProblem.Code);
                    break;
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == "DESC", orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringSessionProblem
                {
                    Id = x.Id,
                    TutoringSessionId = x.TutoringSessionId,
                    TutoringProblemId = x.TutoringProblemId,
                    TutoringProblem = new TutoringProblem
                    {
                        Code = x.TutoringProblem.Code,
                        Category = x.TutoringProblem.Category,
                        Description = x.TutoringProblem.Description
                    }
                }, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringSessionProblem>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
        {
            Expression<Func<TutoringSessionProblem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.TutoringProblem.Category); break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringProblem.Description); break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringSession.StartTime); break;
                case "3":
                    orderByPredicate = ((x) => x.TutoringSession.Tutor.User.FullName); break;

                default:
                    orderByPredicate = ((x) => x.TutoringSession.StartTime);
                    break;
            }

            var query = _context.TutoringSessionProblems
                .Where(x => x.TutoringSession.TutoringSessionStudents.Any(y => y.TutoringStudentStudentId == studentId) && x.TutoringSession.TermId == termId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {                
                    date = x.TutoringSession.StartTime.ToLocalDateFormat(),
                    category = ConstantHelpers.TUTORING.PROBLEM_CATEGORIES.VALUES[x.TutoringProblem.Category],
                    problem = x.TutoringProblem.Description,
                    tutor = x.TutoringSession.Tutor.User.FullName

                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

    }
}
