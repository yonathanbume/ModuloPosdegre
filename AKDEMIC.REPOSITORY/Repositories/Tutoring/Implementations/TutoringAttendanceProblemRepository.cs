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
    public class TutoringAttendanceProblemRepository : Repository<TutoringAttendanceProblem>, ITutoringAttendanceProblemRepository
    {
        public TutoringAttendanceProblemRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTutoringAttendanceIdAndProblemId(Guid tutoringAttendanceId, Guid tutoringProblemId)
            => await _context.TutoringAttendanceProblems.AnyAsync(x => x.TutoringAttendanceId == tutoringAttendanceId && x.TutoringProblemId == tutoringProblemId);

        public async Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null)
        {
            var query = _context.TutoringAttendanceProblems.AsQueryable();

            if (tutoringProblemId.HasValue)
                query = query.Where(x => x.TutoringProblemId == tutoringProblemId.Value);
            if (category.HasValue)
                query = query.Where(x => x.TutoringProblem.Category == category.Value);
            if (termId.HasValue)
                query = query.Where(x => x.TutoringAttendance.TutoringStudentTermId == termId.Value);
            if (careerId.HasValue)
                query = query.Where(x => x.TutoringAttendance.Tutor.CareerId == careerId.Value);
            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutoringAttendance.TutorId == tutorId);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TutoringAttendanceProblem>> GetAllByTutoringAttendanceId(Guid tutoringAttendanceId)
            => await _context.TutoringAttendanceProblems
                .Include(x => x.TutoringProblem)
                .Where(x => x.TutoringAttendanceId == tutoringAttendanceId)
                .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<TutoringAttendanceProblem>> GetTutoringAttendanceProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringAttendanceId = null)
        {
            var query = _context.TutoringAttendanceProblems
                .Where(x => x.TutoringAttendanceId == tutoringAttendanceId)
                .AsNoTracking();
            
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
                    orderByPredicate = ((x) => x.TutoringProblem.Category);

                    break;
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == "DESC", orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringAttendanceProblem
                {
                    Id = x.Id,
                    TutoringAttendanceId = x.TutoringAttendanceId,
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

            return new DataTablesStructs.ReturnedData<TutoringAttendanceProblem>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
