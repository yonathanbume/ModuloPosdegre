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
    public class ProcedureRequirementRepository : Repository<ProcedureRequirement>, IProcedureRequirementRepository
    {
        public ProcedureRequirementRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? procedureId = null, Guid? userProcedureId = null, Expression<Func<ProcedureRequirement, ProcedureRequirement>> selectPredicate = null, Expression<Func<ProcedureRequirement, dynamic>> orderByPredicate = null, Func<ProcedureRequirement, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ProcedureRequirements.AsNoTracking();

            if (procedureId != null)
            {
                query = query.Where(x => x.ProcedureId == procedureId);
            }

            if (userProcedureId != null)
            {
                query = query.Where(x => _context.UserProcedures.Any(y => y.Id == userProcedureId && y.ProcedureId == x.ProcedureId));
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ProcedureRequirement>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<ProcedureRequirement> GetProcedureRequirement(Guid id)
        {
            return await _context.ProcedureRequirements
                .SelectProcedureRequirement()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<decimal> GetProcedureRequirementsCostSumByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureRequirements
                .Where(x => x.ProcedureId == procedureId)
                .SelectProcedureRequirement();

            var result = await query.SumAsync(x => x.Cost);

            return result;
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirements()
        {
            var query = _context.ProcedureRequirements.SelectProcedureRequirement();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByProcedure(Guid procedureId)
        {
            var query = _context.ProcedureRequirements
                .Where(x => x.ProcedureId == procedureId)
                .SelectProcedureRequirement();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedure(Guid userProcedureId)
        {
            var userProcedure = await _context.UserProcedures.FirstOrDefaultAsync(x => x.Id == userProcedureId);

            var query = _context.ProcedureRequirements
                .Where(x => x.ProcedureId == userProcedure.ProcedureId)
                .SelectProcedureRequirement();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ProcedureRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureRequirementsDatatable(sentParameters, null, null, ExpressionHelpers.SelectProcedureRequirement(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            Expression<Func<ProcedureRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureRequirementsDatatable(sentParameters, procedureId, null, ExpressionHelpers.SelectProcedureRequirement(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureRequirement>> GetProcedureRequirementsDatatableByUserProcedure(DataTablesStructs.SentParameters sentParameters, Guid userProcedureId, string searchValue = null)
        {
            Expression<Func<ProcedureRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureRequirementsDatatable(sentParameters, null, userProcedureId, ExpressionHelpers.SelectProcedureRequirement(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }

        #endregion

        public async Task<IEnumerable<ProcedureRequirement>> GetProcedureRequirementsByUserProcedureRecord(Guid userProcedureRecordId)
        {
            var userProcedureRecord = await _context.UserProcedureRecords.FirstOrDefaultAsync(x => x.Id == userProcedureRecordId);

            var query = _context.ProcedureRequirements
                .Where(x => x.ProcedureId == userProcedureRecord.UserProcedure.ProcedureId)
                .Select(x => new ProcedureRequirement
                {
                    Code = x.Code,
                    Cost = x.Cost,
                    Name = x.Name,
                    HasUserProcedureRecordRequirement = _context.UserProcedureRecordRequirements.Any(y => y.ProcedureRequirementId == x.Id && y.UserProcedureRecordId == userProcedureRecordId)
                });

            return await query.ToListAsync();
        }
        public async Task<decimal> GetProcedureAmount(Guid id)
        {
            var result = await _context.ProcedureRequirements
                .Where(pr => pr.ProcedureId == id)
                .SumAsync(pr => pr.Cost);

            return result;
        }
    }
}
