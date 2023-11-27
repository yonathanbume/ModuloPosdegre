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
    public class InternalProcedureFileRepository : Repository<InternalProcedureFile>, IInternalProcedureFileRepository
    {
        public InternalProcedureFileRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<InternalProcedureFile>> GetInternalProcedureFilesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? internalProcedureId, Expression<Func<InternalProcedureFile, InternalProcedureFile>> selectPredicate = null, Expression<Func<InternalProcedureFile, dynamic>> orderByPredicate = null, Func<InternalProcedureFile, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.InternalProcedureFiles.AsNoTracking();

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

            return new DataTablesStructs.ReturnedData<InternalProcedureFile>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesByInternalProcedure(Guid internalProcedureId)
        {
            var query = _context.InternalProcedureFiles
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectInternalProcedureFile();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null)
        {
            Expression<Func<InternalProcedureFile, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FileName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Size);

                    break;
                default:
                    orderByPredicate = ((x) => x.FileName);

                    break;
            }

            return await GetInternalProcedureFilesDatatable(sentParameters, internalProcedureId, ExpressionHelpers.SelectInternalProcedureFile(), orderByPredicate, (x) => new[] { x.FileName }, searchValue);
        }

        #endregion

        public async Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureFile, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedureFiles
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .Where(x => searchValuePredicate != null ? searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2) : true)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedureFile();

            return await query.ToListAsync();
        }
    }
}
