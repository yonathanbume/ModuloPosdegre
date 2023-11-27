using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordDocumentRepository : Repository<UserProcedureRecordDocument>, IUserProcedureRecordDocumentRepository
    {
        public UserProcedureRecordDocumentRepository(AkdemicContext context) : base(context) { }

        public async Task<Tuple<int, List<UserProcedureRecordDocument>>> GetUserProcedureRecordDocuments(Guid userProcedureRecordId, DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.UserProcedureRecordDocuments
                .Where(x => x.UserProcedureRecordId == userProcedureRecordId)
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
                .Select(x => new UserProcedureRecordDocument
                {
                    Id = x.Id,
                    Name = x.Name,
                    DocumentBytesSize = x.DocumentBytesSize,
                    DocumentUrl = x.DocumentUrl
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<UserProcedureRecordDocument>>(records, pagedList);
        }
    }
}
