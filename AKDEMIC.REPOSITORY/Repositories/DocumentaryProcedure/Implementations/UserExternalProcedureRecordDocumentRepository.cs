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
    public class UserExternalProcedureRecordDocumentRepository : Repository<UserExternalProcedureRecordDocument>, IUserExternalProcedureRecordDocumentRepository
    {
        public UserExternalProcedureRecordDocumentRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? userExternalProcedureRecordId = null, Expression<Func<UserExternalProcedureRecordDocument, UserExternalProcedureRecordDocument>> selectPredicate = null, Expression<Func<UserExternalProcedureRecordDocument, dynamic>> orderByPredicate = null, Func<UserExternalProcedureRecordDocument, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserExternalProcedureRecordDocuments.AsNoTracking();

            if (userExternalProcedureRecordId != null)
            {
                query = query.Where(x => x.UserExternalProcedureRecordId == userExternalProcedureRecordId);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocuments()
        {
            var query = _context.UserExternalProcedureRecordDocuments.SelectUserExternalProcedureRecordDocument();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<UserExternalProcedureRecordDocument, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetUserExternalProcedureRecordDocumentsDatatable(sentParameters, null, ExpressionHelpers.SelectUserExternalProcedureRecordDocument(), orderByPredicate, (x) => new[] { x.Name }, searchValue);
        }


        #endregion




        public async Task<DataTablesStructs.ReturnedData<object>> GetUserExternalProcedureRecordDocumentsDatatableByuserExternalProcedureRecord(DataTablesStructs.SentParameters sentParameters, Guid userExternalProcedureRecordId)
        {

            Expression<Func<UserExternalProcedureRecordDocument, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.DocumentBytesSize;
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.UserExternalProcedureRecordDocuments
                .Where(x => x.UserExternalProcedureRecordId == userExternalProcedureRecordId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    filename = x.Name,
                    size = x.DocumentBytesSize,
                    path = x.DocumentUrl
                }).ToListAsync();


            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsByuserExternalProcedureRecord(Guid userExternalProcedureRecordId)
            => await _context.UserExternalProcedureRecordDocuments
                        .Where(x => x.UserExternalProcedureRecordId == userExternalProcedureRecordId).ToListAsync();

        public async Task<Tuple<int, List<UserExternalProcedureRecordDocument>>> GetUserExternalProcedureRecordDocuments(Guid userExternalProcedureRecordId, DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.UserExternalProcedureRecordDocuments
                .Where(x => x.UserExternalProcedureRecordId == userExternalProcedureRecordId)
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.DocumentBytesSize) : query.OrderBy(q => q.DocumentBytesSize);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new UserExternalProcedureRecordDocument
                {
                    Id = x.Id,
                    Name = x.Name,
                    DocumentBytesSize = x.DocumentBytesSize,
                    DocumentUrl = x.DocumentUrl
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedureRecordDocument>>(records, pagedList);
        }
    }
}
