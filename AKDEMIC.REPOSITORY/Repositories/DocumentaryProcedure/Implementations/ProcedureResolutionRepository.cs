using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureResolutionRepository : Repository<ProcedureResolution>, IProcedureResolutionRepository
    {
        public ProcedureResolutionRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? procedureId = null, Expression<Func<ProcedureResolution, ProcedureResolution>> selectPredicate = null, Expression<Func<ProcedureResolution, dynamic>> orderByPredicate = null, Func<ProcedureResolution, object[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureResolutions.AsNoTracking();

            if (procedureId != null)
            {
                query = query.Where(x => x.ProcedureId == procedureId);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProcedureResolution>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<ProcedureResolution> GetProcedureResolution(Guid id)
        {
            return await _context.ProcedureResolutions
                .SelectProcedureResolution()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ProcedureResolution>> GetProcedureResolutionsByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureResolutions
                .Where(x => x.ProcedureId == procedureId)
                .SelectProcedureResolution();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ProcedureResolution, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResolutionType);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.PresentationTerm);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ResolutionTerm);

                    break;
                default:
                    orderByPredicate = ((x) => x.ResolutionType);

                    break;
            }

            return await GetProcedureResolutionsDatatable(sentParameters, null, ExpressionHelpers.SelectProcedureResolution(), orderByPredicate, (x) => new[] { x.PresentationTerm + "", x.ResolutionTerm + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureResolution>> GetProcedureResolutionsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            Expression<Func<ProcedureResolution, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResolutionType);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.PresentationTerm);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.ResolutionTerm);

                    break;
                default:
                    orderByPredicate = ((x) => x.ResolutionType);

                    break;
            }

            return await GetProcedureResolutionsDatatable(sentParameters, procedureId, ExpressionHelpers.SelectProcedureResolution(), orderByPredicate, (x) => new[] { x.PresentationTerm + "", x.ResolutionTerm + "" }, searchValue);
        }

        #endregion
    }
}
