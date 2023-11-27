using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class RecordSubjectTypeRepository : Repository<RecordSubjectType>, IRecordSubjectTypeRepository
    {
        public RecordSubjectTypeRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<RecordSubjectType>> GetRecordSubjectTypesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<RecordSubjectType, RecordSubjectType>> selectPredicate = null, Expression<Func<RecordSubjectType, dynamic>> orderByPredicate = null, Func<RecordSubjectType, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RecordSubjectTypes.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<RecordSubjectType>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetRecordSubjectTypesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<RecordSubjectType, Select2Structs.Result>> selectPredicate = null, Func<RecordSubjectType, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RecordSubjectTypes.AsNoTracking();
            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<bool> AnyRecordSubjectTypeByCode(string code)
        {
            var query = _context.RecordSubjectTypes.Where(x => x.Code == code);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<RecordSubjectType>> GetRecordSubjectTypes()
        {
            var query = _context.RecordSubjectTypes.SelectRecordSubjectType();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<RecordSubjectType>> GetRecordSubjectTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<RecordSubjectType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetRecordSubjectTypesDatatable(sentParameters, ExpressionHelpers.SelectRecordSubjectType(), orderByPredicate, (x) => new[] { x.Code, x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetRecordSubjectTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetRecordSubjectTypesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion

























        public async Task<bool> HasRelatedUserProcedures(Guid recordSubjectTypeId)
        {
            return await _context.UserExternalProcedureRecords.AnyAsync(x => x.RecordSubjectTypeId == recordSubjectTypeId) ||
                   await _context.UserProcedureRecords.AnyAsync(x => x.RecordSubjectTypeId == recordSubjectTypeId);
        }

        public async Task<bool> IsCodeDuplicated(string code)
        {
            return await _context.RecordSubjectTypes.IgnoreQueryFilters().AnyAsync(x => x.Code == code);
        }

        public async Task<bool> IsCodeDuplicated(string code, Guid recordSubjectTypeId)
        {
            return await _context.RecordSubjectTypes.AnyAsync(x => x.Id != recordSubjectTypeId && x.Code == code);
        }

        public async Task<Tuple<int, List<RecordSubjectType>>> GetRecordSubjectTypes(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.RecordSubjectTypes
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.Code.Contains(sentParameters.SearchValue) ||
                            x.Name.Contains(sentParameters.SearchValue))
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new RecordSubjectType
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    HasRelatedUserProcedures = x.UserExternalProcedureRecords.Any() || x.UserProcedureRecords.Any()
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<RecordSubjectType>>(records, pagedList);
        }
    }
}
