using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutorWorkingPlanRepository: Repository<TutorWorkingPlan>, ITutorWorkingPlanRepository
    {
        public TutorWorkingPlanRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByTermTutor(string tutorId, Guid termId)
        {
            return await _context.TutorWorkingPlans.AnyAsync(x => x.TutorId == tutorId && x.TermId == termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByTutorDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId)
        {
            Expression<Func<TutorWorkingPlan, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name);
                    break;
            }

            var query = _context.TutorWorkingPlans
                .Where(x => x.TutorId == tutorId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    termName = x.Term.Name,
                    x.WorkingPlanPath
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

        public async Task<TutorWorkingPlanTemplate> GetInfo(Guid id)
        {
            var data = await _context.TutorWorkingPlans
                .Where(x => x.Id == id)
                .Select(x => new TutorWorkingPlanTemplate
                {
                    Id = x.Id,
                    TutorId = x.TutorId,
                    TutorFullName = x.Tutor.User.FullName,
                    TermId = x.TermId,
                    TermName = x.Term.Name,
                    WorkingPlanPath = x.WorkingPlanPath
                })
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
