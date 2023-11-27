using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectActivityRepository : Repository<ProjectActivity>, IProjectActivityRepository
    {
        public ProjectActivityRepository(AkdemicContext context) : base(context) { }

        public async Task<int> Count(Guid projectId)
        {
            return await _context.EvaluationProjectActivities.CountAsync(x => x.ProjectId == projectId);
        }
        public async Task<bool> AnyProjectActivityByName(string name, Guid? id, Guid projectId)
        {
            if (id == null)
                return await _context.EvaluationProjectActivities.Where(x => x.Name == name && x.ProjectId == projectId).AnyAsync();
            else
                return await _context.EvaluationProjectActivities.Where(x => x.Name == name && x.ProjectId == projectId && x.Id != id).AnyAsync();
        }

        public async Task<object> GetProjectActivity(Guid id)
        {
            var query = _context.EvaluationProjectActivities.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                budget = x.Budget,
                startDate = x.StartDate.ToString("dd/MM/yyyy"),
                endDate = x.EndDate.ToString("dd/MM/yyyy")
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetProjectActivities(Guid projectId)
        {
            var query = _context.EvaluationProjectActivities
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    budget = "S/. " + x.Budget.ToString("0.00"),
                    startDate = x.StartDate.ToString("dd/MM/yyyy"),
                    endDate = x.EndDate.ToString("dd/MM/yyyy")
                });
            return await query.ToListAsync();
        }

        public async Task<List<ProjectActivityTemplate>> GetProjectActivitiesTemplate(Guid projectId)
        {
            List<ProjectActivityTemplate> result = await _context.EvaluationProjectActivities.Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectActivityTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate.ToString("dd/MM/yyyy"),
                    EndDate = x.EndDate.ToString("dd/MM/yyyy"),
                    Budget = x.Budget.ToString("0.00"),
                    State = "-"
                }).ToListAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null)
        {
            Expression<Func<ProjectActivity, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Budget); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<ProjectActivity> query = _context.EvaluationProjectActivities.AsNoTracking();

            query = query.Where(x => x.ProjectId == projectId);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    budget = "S/. " + x.Budget.ToString("0.00"),
                    startDate = x.StartDate.ToString("dd/MM/yyyy"),
                    endDate = x.EndDate.ToString("dd/MM/yyyy")
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<decimal> GetProjectActivitiesTotalBudget(Guid projectId, Guid? id = null)
        {
            if (id.HasValue)
            {
                return await _context.EvaluationProjectActivities.Where(x => x.ProjectId == projectId && x.Id != id.Value).Select(x => x.Budget).SumAsync();
            }
            return await _context.EvaluationProjectActivities.Where(x => x.ProjectId == projectId).Select(x => x.Budget).SumAsync();
        }
    }
}
