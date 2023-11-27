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
    public class InternalProcedureReferenceRepository : Repository<InternalProcedureReference>, IInternalProcedureReferenceRepository
    {
        public InternalProcedureReferenceRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<InternalProcedureReference>> GetInternalProcedureReferencesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? internalProcedureId, Expression<Func<InternalProcedureReference, InternalProcedureReference>> selectPredicate = null, Expression<Func<InternalProcedureReference, dynamic>> orderByPredicate = null, Func<InternalProcedureReference, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InternalProcedureReferences.AsNoTracking();

            if (internalProcedureId != null)
            {
                query = query.Where(x => x.InternalProcedureId == internalProcedureId);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<InternalProcedureReference>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesByInternalProcedure(Guid internalProcedureId)
        {
            var query = _context.InternalProcedureReferences
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectInternalProcedureReference();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null)
        {
            Expression<Func<InternalProcedureReference, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Reference);

                    break;
            }

            return await GetInternalProcedureReferencesDatatable(sentParameters, internalProcedureId, ExpressionHelpers.SelectInternalProcedureReference(), orderByPredicate, (x) => new[] { x.Reference }, searchValue);
        }

        #endregion

        public async Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureReference, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedureReferences
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .Where(x => searchValuePredicate != null ? searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2) : true)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedureReference();

            return await query.ToListAsync();
        }
    }
}
