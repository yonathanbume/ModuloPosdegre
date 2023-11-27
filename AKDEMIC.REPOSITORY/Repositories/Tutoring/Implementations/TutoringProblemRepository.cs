using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringProblemRepository : Repository<TutoringProblem>, ITutoringProblemRepository
    {
        public TutoringProblemRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<TutoringProblem> FindByCode(string code)
            => await _context.TutoringProblems.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<IEnumerable<TutoringProblem>> GetAllByCategory(byte category)
            => await _context.TutoringProblems.Where(x => x.Category == category).ToListAsync();

        public async Task<IEnumerable<TutoringProblem>> GetAllByCategoryNu(byte? category = null, string search = null)
        {
            var query =  _context.TutoringProblems.AsQueryable();
            if (category.HasValue)
                query = query.Where(x => x.Category == category);

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Code.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search));

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, byte? category = null)
        {
            Expression<Func<TutoringProblem, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Category);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Description);

                    break;
                default:
                    orderByPredicate = ((x) => x.Code);

                    break;
            }

            var query = _context.TutoringProblems
                .AsNoTracking();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value);


            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Code.ToUpper().Contains(searchValue)
                            || x.Description.ToUpper().Contains(searchValue)
                            || ConstantHelpers.TUTORING.PROBLEM_CATEGORIES.VALUES[x.Category].ToUpper().Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringProblem
                {
                    Id = x.Id,
                    Code = x.Code,
                    Category = x.Category,
                    Description = x.Description
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringProblem>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatableByTutoring(DataTablesStructs.SentParameters sentParameters, Guid tutoringId, string searchValue = null, byte? category = null)
        {
            var query = _context.TutoringAttendanceProblems
                .Where(x => x.TutoringAttendance.TutoringStudentStudentId == tutoringId)
                .AsNoTracking();

            if (category.HasValue)
                query = query.Where(x => x.TutoringProblem.Category == category.Value);

            Expression<Func<TutoringAttendanceProblem, dynamic>> orderByPredicate = null;

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
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringProblem
                {
                    Id = x.TutoringProblem.Id,
                    Code = x.TutoringProblem.Code,
                    Category = x.TutoringProblem.Category,
                    Description = x.TutoringProblem.Description
                }, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringProblem>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
