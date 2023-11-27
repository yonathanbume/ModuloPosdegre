using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class AcademicExchangeNewsRepository : Repository<AcademicExchangeNews> , IAcademicExchangeNewsRepository
    {
        public AcademicExchangeNewsRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<AcademicExchangeNews>> GetAcademicExchangeNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<AcademicExchangeNews, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Title);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.AcademicExchangeNews.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Title.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<AcademicExchangeNews, AcademicExchangeNews>> selectPredicate = (x) => new AcademicExchangeNews
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                Title = x.Title,
                Url = x.Url
            };

            return await query.ToDataTables(sentParameters, selectPredicate);

        }

        public async Task<IEnumerable<AcademicExchangeNews>> GetAllServerSide(int rowsPerPage ,int page)
            => await _context.AcademicExchangeNews.Skip(rowsPerPage * page).Take(rowsPerPage).ToArrayAsync();
    }
}
