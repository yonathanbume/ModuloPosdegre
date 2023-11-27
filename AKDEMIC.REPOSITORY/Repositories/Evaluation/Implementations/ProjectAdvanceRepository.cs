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
    public class ProjectAdvanceRepository : Repository<ProjectAdvance>, IProjectAdvanceRepository
    {
        public ProjectAdvanceRepository(AkdemicContext context) : base(context) { }

        public async Task<int> Count(Guid projectId)
        {
            return await _context.EvaluationProjectAdvances.CountAsync(x => x.ProjectId == projectId);
        }
        public async Task<bool> AnyProjectAdvanceByName(string name, Guid? id, Guid projectId)
        {
            if (id == null)
            {
                return await _context.EvaluationProjectAdvances.Where(x => x.Name == name && x.ProjectId == projectId).AnyAsync();
            }
            else
            {
                return await _context.EvaluationProjectAdvances.Where(x => x.Name == name && x.ProjectId == projectId && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetProjectAdvance(Guid id)
        {
            var query = _context.EvaluationProjectAdvances.Select(x => new
            {
                id = x.Id,
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetProjectAdvances(Guid projectId)
        {
            var query = _context.EvaluationProjectAdvances.Select(x => new
            {
                id = x.Id,
            });
            return await query.ToListAsync();
        }

        public async Task<List<ProjectAdvanceTemplate>> GetProjectAdvancesTemplate(Guid projectId)
        {
            List<ProjectAdvanceTemplate> result = await _context.EvaluationProjectAdvances.Where(x => x.ProjectId == projectId).Select(x => new ProjectAdvanceTemplate
            {
                Name = x.Name,
                Qualification = x.Qualification.ToString("0.00")
            }).ToListAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProjectAdvancesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string search = null)
        {
            Expression<Func<ProjectAdvance, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            IQueryable<ProjectAdvance> query = _context.EvaluationProjectAdvances;

            query = query.Where(x => x.ProjectId == projectId);

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(search));
            }

            try
            {
                var recordsFiltered = await query.CountAsync();

                var data = query
                               .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                               .Skip(sentParameters.PagingFirstRecord)
                               .Take(sentParameters.RecordsPerDraw)
                               .Select(x => new
                               {
                                   id = x.Id,
                                   name = x.Name,
                                   status = x.Status,
                                   isFinal = x.IsFinal,
                                   presentation = x.ProjectAdvanceHistorics.OrderByDescending(y => y.Update).FirstOrDefault()
                               }).Select(x => new
                               {
                                   x.id,
                                   x.name,
                                   x.status,
                                   x.isFinal,
                                   update = x.presentation == null ? "Sin datos" : x.presentation.Update.ToString("dd/MM/yyyy hh:mm"),
                                   file = x.presentation == null ? "Sin datos" : x.presentation.File
                               }).AsEnumerable();
                int recordsTotal = data.Count();
                //if (!string.IsNullOrEmpty(search))
                //{
                //    data = data.Where(x => x.name.Contains(search) ||
                //                    x.file.Contains(search) ||
                //                    x.update.Contains(search)).ToList();
                //}


                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Project> GetProjectByProjectAdvanceId(Guid id)
        {
            return await _context.EvaluationProjectAdvances.Where(x => x.Id == id).Select(x => x.Project).FirstAsync();
        }

        public async Task<ProjectAdvance> IsFinal(Guid projectId)
        {
            return await _context.EvaluationProjectAdvances.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsFinal);
        }

        public async Task<ProjectAdvance> GetWithProjectItemScores(Guid advanceId)
        {
            return await _context.EvaluationProjectAdvances.Where(x => x.Id == advanceId).FirstOrDefaultAsync();
        }

        public async Task<string> GetAdvanceHistoryUrl(Guid id)
        {
            var test = await _context.EvaluationProjectAdvanceHistorics.Where(x => x.ProjectAdvanceId == id).OrderByDescending(x => x.Update).FirstOrDefaultAsync();
            return test.File;
        }
    }
}
