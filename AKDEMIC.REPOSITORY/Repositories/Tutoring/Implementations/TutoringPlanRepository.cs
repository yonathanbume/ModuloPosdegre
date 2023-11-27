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
    public class TutoringPlanRepository : Repository<TutoringPlan>, ITutoringPlanRepository
    {
        public TutoringPlanRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TutoringPlan>> GetAll()
            => await _context.TutoringPlans
                .Include(x => x.Career)
                .ToListAsync();

        public async Task<TutoringPlan> GetByCareerId(Guid careerId)
            => await _context.TutoringPlans
                .Include(x => x.Career)
                .FirstOrDefaultAsync(x => x.CareerId == careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<TutoringPlan, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.TutoringCoordinator.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                
                default:
                    orderByPredicate = ((x) => x.TutoringCoordinator.User.FullName);

                    break;
            }
            var query = _context.TutoringPlans.Include(x => x.TutoringCoordinator).Include(x => x.Career)
                    .AsNoTracking();


            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.TutoringCoordinator.User.FullName.ToUpper().Contains(search)
                            || x.Career.Name.ToUpper().Contains(search));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    fullname = x.TutoringCoordinator.User.FullName,
                    career = x.Career.Name,
                    url = x.Url

                })
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
        public async Task<TutoringPlan> GetWithData(Guid id)
            => await _context.TutoringPlans.Include(x => x.Career).Where(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<bool> AnyByCoordinator(string tutorCoordinatorId)
            => await _context.TutoringPlans.AnyAsync(x => x.TutoringCoordinatorId == tutorCoordinatorId);
        public async Task<TutoringPlan> GetByCoordinator(string tutorCoordinatorId)
            => await _context.TutoringPlans.Where(x => x.TutoringCoordinatorId == tutorCoordinatorId).FirstOrDefaultAsync();

        public async Task<bool> AnyByCareer(Guid careerId)
        {
            return await _context.TutoringPlans.AnyAsync(x => x.CareerId == careerId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
