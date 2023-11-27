using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupFileRepository : Repository<InterestGroupFile> , IInterestGroupFileRepository
    {
        public InterestGroupFileRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<InterestGroupFile>> GetInterestGroupFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid interestGroupId, string searchValue = null)
        {
            Expression<Func<InterestGroupFile, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.InterestGroupFiles.Where(x => x.InterestGroupId == interestGroupId).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<InterestGroupFile, InterestGroupFile>> selectPredicate = (x) => new InterestGroupFile
            {
                Id = x.Id,
                Name = x.Name,
                UrlFile = x.UrlFile,
                CreatedAt = x.CreatedAt
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }
    }
}
