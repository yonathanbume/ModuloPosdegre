using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectAdvanceRepository : Repository<ProjectAdvance>, IProjectAdvanceRepository
    {
        public ProjectAdvanceRepository(AkdemicContext context) : base(context) { }

        public async Task<int> Count(Guid projectId)
        {
            return await _context.InvestigationProjectAdvances.CountAsync(x => x.ProjectId == projectId);
        }
        public async Task<bool> AnyProjectAdvanceByName(string name, Guid? id, Guid projectId)
        {
            if (id == null)
            {
                return await _context.InvestigationProjectAdvances.Where(x => x.Name == name && x.ProjectId == projectId).AnyAsync();
            }
            else
            {
                return await _context.InvestigationProjectAdvances.Where(x => x.Name == name && x.ProjectId == projectId && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetProjectAdvance(Guid id)
        {
            var query = _context.InvestigationProjectAdvances.Select(x => new
            {
                id = x.Id,
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetProjectAdvances(Guid projectId)
        {
            var query = _context.InvestigationProjectAdvances.Select(x => new
            {
                id = x.Id,
            });
            return await query.ToListAsync();
        }

        public async Task<List<ProjectAdvanceTemplate>> GetProjectAdvancesTemplate(Guid projectId)
        {
            List<ProjectAdvanceTemplate> result = await _context.InvestigationProjectAdvances.Where(x => x.ProjectId == projectId).Select(x => new ProjectAdvanceTemplate
            {
                Name = x.Name,
                Qualification = x.Qualification.ToString("0.00")
            }).ToListAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectAdvancesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null)
        {
            Expression<Func<ProjectAdvance, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<ProjectAdvance> query = _context.InvestigationProjectAdvances.AsNoTracking();

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
                    description = x.Description,
                    status = x.Status,
                    isFinal = x.IsFinal,
                    presentation = x.ProjectAdvanceHistorics.OrderByDescending(y => y.Update).FirstOrDefault(),
                    qualified = x.ProjectItemScores.Count() == 0 ? false : true,
                    score = x.ProjectItemScores.Select(y => (int)y.Score).Sum().ToString("0.00")
                }).Select(x => new
                {
                    x.id,
                    x.name,
                    x.description,
                    x.status,
                    x.isFinal,
                    x.qualified,
                    x.score,
                    update = x.presentation == null ? "Sin datos" : x.presentation.Update.ToString("dd/MM/yyyy hh:mm"),
                    file = x.presentation == null ? "Sin datos" : x.presentation.File
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

        public async Task<ProjectAdvance> GetByProjectId(Guid projectId, bool? isFinal = null)
        {
            IQueryable<ProjectAdvance> query = _context.InvestigationProjectAdvances.Where(x => x.ProjectId == projectId).AsQueryable();

            if (isFinal.HasValue)
            {
                query = query.Where(x => x.IsFinal == isFinal);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
