using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class LanguageRepository: Repository<Language> , ILanguageRepository
    {
        public LanguageRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetLanguageDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Language, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }


            var query = _context.Languages
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Contains(searchValue));
            }

            Expression<Func<Language, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                Description = x.Name
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetSelect2CLientSide()
        {
            var result = await _context.Languages
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return result;
        }
    }
}
