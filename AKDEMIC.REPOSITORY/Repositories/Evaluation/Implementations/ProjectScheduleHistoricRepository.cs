using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectScheduleHistoricRepository : Repository<ProjectScheduleHistoric> , IProjectScheduleHistoricRepository
    {
        public ProjectScheduleHistoricRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<ProjectScheduleHistoric>> GetProjectScheduleHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
        {
            Expression<Func<ProjectScheduleHistoric, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var currentScheduleFileUrl = await _context.EvaluationProjects.Where(x => x.Id == projectId).Select(x => x.ScheduleFile).FirstOrDefaultAsync();

            var query = _context.EvaluationProjectScheduleHistorics
                .Where(x=>x.ProjectId == projectId)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<ProjectScheduleHistoric, ProjectScheduleHistoric>> selectPredicate = (x) => new ProjectScheduleHistoric
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                FileUrl = x.FileUrl,
                Active = x.FileUrl == currentScheduleFileUrl
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

    }
}
